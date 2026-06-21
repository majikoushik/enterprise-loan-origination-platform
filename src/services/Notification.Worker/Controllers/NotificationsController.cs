using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Notification.Worker.Application.DTOs;
using Notification.Worker.Infrastructure.Data;

namespace Notification.Worker.Controllers;

[ApiController]
[Route("api/v1/notifications")]
public class NotificationsController : ControllerBase
{
    private readonly NotificationDbContext _dbContext;

    public NotificationsController(NotificationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<NotificationRequestResponse>>> GetNotifications()
    {
        var notifications = await _dbContext.NotificationRequests
            .OrderByDescending(n => n.CreatedAtUtc)
            .Take(50)
            .Select(n => new NotificationRequestResponse
            {
                Id = n.Id,
                CorrelationId = n.CorrelationId,
                EventType = n.EventType,
                EntityType = n.EntityType,
                EntityId = n.EntityId,
                CustomerId = n.CustomerId,
                Recipient = n.Recipient,
                Channel = n.Channel.ToString(),
                Subject = n.Subject,
                MessageBody = n.MessageBody,
                Status = n.Status.ToString(),
                CreatedAtUtc = n.CreatedAtUtc,
                ProcessedAtUtc = n.ProcessedAtUtc,
                FailureReason = n.FailureReason,
                RetryCount = n.RetryCount
            })
            .ToListAsync();

        return Ok(notifications);
    }
}
