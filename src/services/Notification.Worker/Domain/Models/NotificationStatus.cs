namespace Notification.Worker.Domain.Models;

public enum NotificationStatus
{
    Pending = 1,
    Processing = 2,
    Sent = 3,
    Failed = 4,
    PermanentlyFailed = 5,
    Cancelled = 6
}
