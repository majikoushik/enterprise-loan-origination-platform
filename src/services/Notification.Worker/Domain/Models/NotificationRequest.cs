using System;
using System.Collections.Generic;

namespace Notification.Worker.Domain.Models;

public class NotificationRequest
{
    public Guid Id { get; private set; }
    public string CorrelationId { get; private set; }
    public string EventType { get; private set; }
    public string EntityType { get; private set; }
    public Guid EntityId { get; private set; }
    public Guid? CustomerId { get; private set; }
    public string Recipient { get; private set; }
    public NotificationChannel Channel { get; private set; }
    public string Subject { get; private set; }
    public string MessageBody { get; private set; }
    public NotificationStatus Status { get; private set; }
    public DateTimeOffset CreatedAtUtc { get; private set; }
    public DateTimeOffset? ProcessedAtUtc { get; private set; }
    public string FailureReason { get; private set; }

    private readonly List<NotificationDeliveryAttempt> _deliveryAttempts = new();
    public IReadOnlyCollection<NotificationDeliveryAttempt> DeliveryAttempts => _deliveryAttempts.AsReadOnly();

    protected NotificationRequest() { }

    public NotificationRequest(
        Guid id,
        string correlationId,
        string eventType,
        string entityType,
        Guid entityId,
        Guid? customerId,
        string recipient,
        NotificationChannel channel,
        string subject,
        string messageBody)
    {
        Id = id;
        CorrelationId = correlationId;
        EventType = eventType;
        EntityType = entityType;
        EntityId = entityId;
        CustomerId = customerId;
        Recipient = recipient;
        Channel = channel;
        Subject = subject;
        MessageBody = messageBody;
        Status = NotificationStatus.Pending;
        CreatedAtUtc = DateTimeOffset.UtcNow;
    }

    public void MarkProcessing()
    {
        Status = NotificationStatus.Processing;
    }

    public void MarkSent(string providerResponse)
    {
        Status = NotificationStatus.Sent;
        ProcessedAtUtc = DateTimeOffset.UtcNow;
        AddAttempt(NotificationStatus.Sent, providerResponse, null);
    }

    public void MarkFailed(string reason, string providerResponse = null)
    {
        Status = NotificationStatus.Failed;
        FailureReason = reason;
        ProcessedAtUtc = DateTimeOffset.UtcNow;
        AddAttempt(NotificationStatus.Failed, providerResponse, reason);
    }

    private void AddAttempt(NotificationStatus status, string providerResponse, string failureReason)
    {
        _deliveryAttempts.Add(new NotificationDeliveryAttempt(
            Guid.NewGuid(),
            Id,
            _deliveryAttempts.Count + 1,
            Channel,
            status,
            DateTimeOffset.UtcNow,
            providerResponse,
            failureReason
        ));
    }
}
