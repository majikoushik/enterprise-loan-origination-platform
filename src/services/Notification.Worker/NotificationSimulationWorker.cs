using Notification.Worker.Domain.Models;
using Notification.Worker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Notification.Worker;

public sealed class NotificationSimulationWorker : BackgroundService
{
    private readonly ILogger<NotificationSimulationWorker> _logger;
    private readonly IServiceProvider _serviceProvider;

    public NotificationSimulationWorker(ILogger<NotificationSimulationWorker> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
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

    private async Task ProcessPendingNotificationsAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<NotificationDbContext>();

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
            await Task.Delay(TimeSpan.FromMilliseconds(500), stoppingToken);

            // Simulate 90% success rate
            if (Random.Shared.NextDouble() > 0.1)
            {
                request.MarkSent("250 OK: simulated successful delivery");
                _logger.LogInformation("Notification {RequestId} sent successfully", request.Id);
            }
            else
            {
                request.MarkFailed("Simulated random provider failure", "500 Server Error");
                _logger.LogWarning("Notification {RequestId} failed to send", request.Id);
            }
        }

        await dbContext.SaveChangesAsync(stoppingToken);
    }
}
