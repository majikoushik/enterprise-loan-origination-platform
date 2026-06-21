using System;

namespace LoanApplication.Api.Domain.Models;

public class ApplicationStatusHistory
{
    public Guid Id { get; private set; }
    public Guid ApplicationId { get; private set; }
    public ApplicationStatus? PreviousStatus { get; private set; }
    public ApplicationStatus NewStatus { get; private set; }
    public string Reason { get; private set; }
    public string ChangedBy { get; private set; }
    public DateTime ChangedAt { get; private set; }

    private ApplicationStatusHistory() { } // EF Core

    public ApplicationStatusHistory(
        Guid applicationId,
        ApplicationStatus? previousStatus,
        ApplicationStatus newStatus,
        string reason,
        string changedBy)
    {
        Id = Guid.NewGuid();
        ApplicationId = applicationId;
        PreviousStatus = previousStatus;
        NewStatus = newStatus;
        Reason = reason ?? throw new ArgumentNullException(nameof(reason));
        ChangedBy = changedBy ?? throw new ArgumentNullException(nameof(changedBy));
        ChangedAt = DateTime.UtcNow;
    }
}
