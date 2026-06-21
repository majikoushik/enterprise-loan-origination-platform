using System;

namespace Notification.Worker.Application.DTOs;

public class NotificationRequestResponse
{
    public Guid Id { get; set; }
    public string CorrelationId { get; set; } = string.Empty;
    public string EventType { get; set; } = string.Empty;
    public string EntityType { get; set; } = string.Empty;
    public Guid EntityId { get; set; }
    public Guid? CustomerId { get; set; }
    public string Recipient { get; set; } = string.Empty;
    public string Channel { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string MessageBody { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTimeOffset CreatedAtUtc { get; set; }
    public DateTimeOffset? ProcessedAtUtc { get; set; }
    public string? FailureReason { get; set; }
    public int RetryCount { get; set; }
}
