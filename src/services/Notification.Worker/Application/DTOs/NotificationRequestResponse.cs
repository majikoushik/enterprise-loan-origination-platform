using System;

namespace Notification.Worker.Application.DTOs;

public class NotificationRequestResponse
{
    public Guid Id { get; set; }
    public string CorrelationId { get; set; }
    public string EventType { get; set; }
    public string EntityType { get; set; }
    public Guid EntityId { get; set; }
    public Guid? CustomerId { get; set; }
    public string Recipient { get; set; }
    public string Channel { get; set; }
    public string Subject { get; set; }
    public string MessageBody { get; set; }
    public string Status { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }
    public DateTimeOffset? ProcessedAtUtc { get; set; }
    public string FailureReason { get; set; }
}
