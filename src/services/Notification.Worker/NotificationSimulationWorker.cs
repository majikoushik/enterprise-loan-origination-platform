using Notification.Worker.Domain.Models;
using Notification.Worker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Auditing;

namespace Notification.Worker;

public sealed class NotificationSimulationWorker : BackgroundService
{
    internal const int MaxRetryCount = 3;

    private readonly ILogger<NotificationSimulationWorker> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly Func<double> _randomValueFactory;
    private readonly TimeSpan _processingDelay;

    public NotificationSimulationWorker(ILogger<NotificationSimulationWorker> logger, IServiceProvider serviceProvider)
        : this(logger, serviceProvider, () => Random.Shared.NextDouble(), TimeSpan.FromMilliseconds(500))
    {
    }

    internal NotificationSimulationWorker(
        ILogger<NotificationSimulationWorker> logger,
        IServiceProvider serviceProvider,
        Func<double> randomValueFactory,
        TimeSpan processingDelay)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _randomValueFactory = randomValueFactory;
        _processingDelay = processingDelay;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Notification simulation worker started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessPendingNotificationsAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred processing notifications");
            }

            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
    }

    internal async Task ProcessPendingNotificationsAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<NotificationDbContext>();
        var auditLogger = scope.ServiceProvider.GetRequiredService<IAuditLogger>();

        var pendingRequests = await dbContext.NotificationRequests
            .Where(r => r.Status == NotificationStatus.Pending)
            .Take(10)
            .ToListAsync(stoppingToken);

        if (!pendingRequests.Any()) return;

        foreach (var request in pendingRequests)
        {
            request.MarkProcessing();
        }
        await dbContext.SaveChangesAsync(stoppingToken);

        foreach (var request in pendingRequests)
        {
            _logger.LogInformation("Simulating delivery of {Channel} notification {RequestId} to {Recipient}", request.Channel, request.Id, request.Recipient);
            
            // Simulate processing time
            await Task.Delay(_processingDelay, stoppingToken);

            // Simulate 90% success rate
            if (_randomValueFactory() > 0.1)
            {
                request.MarkSent("250 OK: simulated successful delivery");
                _logger.LogInformation("Notification {RequestId} sent successfully", request.Id);
                
                await auditLogger.LogAsync(new AuditEventRecord(
                    Guid.NewGuid(),
                    request.CorrelationId,
                    "NotificationDeliveryCompleted",
                    "Notification",
                    "NotificationRequest",
                    request.Id.ToString(),
                    request.CustomerId,
                    "System",
                    "System",
                    "DeliverNotification",
                    "Notification delivered successfully",
                    "{}",
                    DateTimeOffset.UtcNow,
                    "Notification.Worker",
                    "Info"
                ), stoppingToken);
            }
            else
            {
                if (request.RetryCount < MaxRetryCount)
                {
                    request.MarkForRetry("Simulated transient provider failure", "500 Server Error");
                    _logger.LogWarning(
                        "Notification {RequestId} failed, scheduled for retry {RetryCount}/{MaxRetryCount}",
                        request.Id,
                        request.RetryCount,
                        MaxRetryCount);
                }
                else
                {
                    request.MarkPermanentlyFailed("Simulated random provider failure", "500 Server Error");
                    _logger.LogError(
                        "Notification {RequestId} permanently failed after {RetryCount} retries",
                        request.Id,
                        request.RetryCount);
                }

                await auditLogger.LogAsync(new AuditEventRecord(
                    Guid.NewGuid(),
                    request.CorrelationId,
                    "NotificationDeliveryFailed",
                    "Notification",
                    "NotificationRequest",
                    request.Id.ToString(),
                    request.CustomerId,
                    "System",
                    "System",
                    "DeliverNotification",
                    request.Status == NotificationStatus.Pending ? "Notification delivery failed; retry scheduled" : "Notification delivery permanently failed",
                    System.Text.Json.JsonSerializer.Serialize(new { reason = request.FailureReason, retryCount = request.RetryCount, status = request.Status.ToString() }),
                    DateTimeOffset.UtcNow,
                    "Notification.Worker",
                    "Warning"
                ), stoppingToken);
            }
        }

        await dbContext.SaveChangesAsync(stoppingToken);
    }
}
