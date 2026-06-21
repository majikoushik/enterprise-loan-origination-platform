using Microsoft.AspNetCore.Mvc;
using Notification.Worker.Domain.Models;
using Notification.Worker.Infrastructure.Data;
using Auditing;

namespace Notification.Worker.Controllers;

[ApiController]
[Route("api/v1/internal/events")]
public class InternalEventsController : ControllerBase
{
    private readonly NotificationDbContext _dbContext;
    private readonly ILogger<InternalEventsController> _logger;
    private readonly IAuditLogger _auditLogger;

    public InternalEventsController(NotificationDbContext dbContext, ILogger<InternalEventsController> logger, IAuditLogger auditLogger)
    {
        _dbContext = dbContext;
        _logger = logger;
        _auditLogger = auditLogger;
    }

    [HttpPost]
    public async Task<IActionResult> ReceiveEvent([FromBody] JsonElement eventPayload)
    {
        try
        {
            var eventType = eventPayload.GetProperty("eventType").GetString();
            var correlationId = eventPayload.TryGetProperty("correlationId", out var cId) ? cId.GetString() : Guid.NewGuid().ToString();
            var entityId = eventPayload.TryGetProperty("entityId", out var eId) ? eId.GetGuid() : Guid.Empty;

            if (eventType == "LoanApplicationStatusChanged")
            {
                var newStatus = eventPayload.GetProperty("newStatus").GetString();
                var customerId = eventPayload.TryGetProperty("customerId", out var c) ? c.GetGuid() : (Guid?)null;

                var subject = $"Loan Application Update: {newStatus}";
                var body = $"Your loan application {entityId} status is now {newStatus}.";

                var notificationRequest = new NotificationRequest(
                    Guid.NewGuid(),
                    correlationId,
                    eventType,
                    "LoanApplication",
                    entityId,
                    customerId,
                    "customer@example.com", // Simulated recipient
                    NotificationChannel.Email,
                    subject,
                    body
                );

                _dbContext.NotificationRequests.Add(notificationRequest);
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation("Stored pending NotificationRequest {RequestId} for event {EventType}", notificationRequest.Id, eventType);

                await _auditLogger.LogAsync(new AuditEventRecord(
                    Guid.NewGuid(),
                    correlationId,
                    "NotificationRequested",
                    "Notification",
                    "LoanApplication",
                    entityId.ToString(),
                    customerId,
                    "System",
                    "System",
                    "RequestNotification",
                    $"Notification requested for {eventType}",
                    System.Text.Json.JsonSerializer.Serialize(new { channel = "Email", eventType = eventType }),
                    DateTimeOffset.UtcNow,
                    "Notification.Worker",
                    "Info"
                ));
            }
            // Future events like CustomerRegistered can be handled here

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to process internal event payload");
            return BadRequest(new { Error = "Invalid event payload" });
        }
    }
}
