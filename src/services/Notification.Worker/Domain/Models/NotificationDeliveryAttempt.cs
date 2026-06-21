using System;

namespace Notification.Worker.Domain.Models;

public class NotificationDeliveryAttempt
{
    public Guid Id { get; private set; }
    public Guid NotificationRequestId { get; private set; }
    public int AttemptNumber { get; private set; }
    public NotificationChannel Channel { get; private set; }
    public NotificationStatus Status { get; private set; }
    public DateTimeOffset AttemptedAtUtc { get; private set; }
    public string ProviderResponse { get; private set; }
    public string FailureReason { get; private set; }

    protected NotificationDeliveryAttempt() { }

    internal NotificationDeliveryAttempt(
        Guid id,
        Guid notificationRequestId,
        int attemptNumber,
        NotificationChannel channel,
        NotificationStatus status,
        DateTimeOffset attemptedAtUtc,
        string providerResponse,
        string failureReason)
    {
        Id = id;
        NotificationRequestId = notificationRequestId;
        AttemptNumber = attemptNumber;
        Channel = channel;
        Status = status;
        AttemptedAtUtc = attemptedAtUtc;
        ProviderResponse = providerResponse;
        FailureReason = failureReason;
    }
}
