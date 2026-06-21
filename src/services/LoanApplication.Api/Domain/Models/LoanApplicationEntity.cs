using System;
using System.Collections.Generic;
using LoanApplication.Api.Domain.Exceptions;

namespace LoanApplication.Api.Domain.Models;

public class LoanApplicationEntity
{
    public Guid Id { get; private set; }
    public Guid CustomerId { get; private set; }
    public LoanType LoanType { get; private set; }
    public decimal RequestedAmount { get; private set; }
    public int RequestedTenureInMonths { get; private set; }
    public string Purpose { get; private set; } = string.Empty;
    public decimal DeclaredMonthlyIncome { get; private set; }
    public decimal ExistingEmiObligations { get; private set; }
    public ApplicationStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private readonly List<ApplicationStatusHistory> _statusHistory;
    public IReadOnlyCollection<ApplicationStatusHistory> StatusHistory => _statusHistory.AsReadOnly();

    private LoanApplicationEntity()
    {
        // Required by EF Core
        _statusHistory = new List<ApplicationStatusHistory>();
    }

    public LoanApplicationEntity(
        Guid customerId,
        LoanType loanType,
        decimal requestedAmount,
        int requestedTenureInMonths,
        string purpose,
        decimal declaredMonthlyIncome,
        decimal existingEmiObligations)
    {
        Id = Guid.NewGuid();
        CustomerId = customerId;
        LoanType = loanType;
        RequestedAmount = requestedAmount;
        RequestedTenureInMonths = requestedTenureInMonths;
        Purpose = purpose ?? throw new ArgumentNullException(nameof(purpose));
        DeclaredMonthlyIncome = declaredMonthlyIncome;
        ExistingEmiObligations = existingEmiObligations;
        Status = ApplicationStatus.Submitted; // Initial status upon valid submission
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        
        _statusHistory = new List<ApplicationStatusHistory>
        {
            new ApplicationStatusHistory(Id, null, ApplicationStatus.Submitted, "Application initial submission", "System")
        };

        ValidateDomainRules();
    }

    private void ValidateDomainRules()
    {
        if (RequestedAmount <= 0)
        {
            throw new LoanApplicationDomainException("Requested amount must be greater than zero.");
        }

        if (RequestedTenureInMonths < 6 || RequestedTenureInMonths > 84)
        {
            throw new LoanApplicationDomainException("Requested tenure must be between 6 and 84 months.");
        }

        if (DeclaredMonthlyIncome <= 0)
        {
            throw new LoanApplicationDomainException("Declared monthly income must be greater than zero.");
        }

        if (ExistingEmiObligations < 0)
        {
            throw new LoanApplicationDomainException("Existing EMI obligations cannot be negative.");
        }

        if (ExistingEmiObligations > DeclaredMonthlyIncome)
        {
            throw new LoanApplicationDomainException("Existing EMI obligations should not be greater than declared monthly income.");
        }
    }

    public void ChangeStatus(ApplicationStatus newStatus, string reason, string changedBy)
    {
        if (Status == newStatus) return;

        if (!IsValidTransition(Status, newStatus))
        {
            throw new LoanApplicationDomainException($"Invalid state transition from {Status} to {newStatus}.");
        }

        var history = new ApplicationStatusHistory(Id, Status, newStatus, reason, changedBy);
        _statusHistory.Add(history);

        Status = newStatus;
        UpdatedAt = DateTime.UtcNow;
    }

    private static bool IsValidTransition(ApplicationStatus current, ApplicationStatus next)
    {
        return current switch
        {
            ApplicationStatus.Draft => next == ApplicationStatus.Submitted,
            ApplicationStatus.Submitted => next == ApplicationStatus.UnderReview || next == ApplicationStatus.Cancelled,
            ApplicationStatus.UnderReview => next == ApplicationStatus.EligibilityPassed || next == ApplicationStatus.EligibilityFailed,
            ApplicationStatus.EligibilityPassed => next == ApplicationStatus.Approved || next == ApplicationStatus.Rejected,
            ApplicationStatus.EligibilityFailed => next == ApplicationStatus.Rejected || next == ApplicationStatus.Cancelled,
            ApplicationStatus.Approved => false,
            ApplicationStatus.Rejected => false,
            ApplicationStatus.Cancelled => false,
            _ => false
        };
    }
}
