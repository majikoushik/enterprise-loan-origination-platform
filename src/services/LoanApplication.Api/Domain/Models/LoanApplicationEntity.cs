using System;
using LoanApplication.Api.Domain.Exceptions;

namespace LoanApplication.Api.Domain.Models;

public class LoanApplicationEntity
{
    public Guid Id { get; private set; }
    public Guid CustomerId { get; private set; }
    public LoanType LoanType { get; private set; }
    public decimal RequestedAmount { get; private set; }
    public int RequestedTenureInMonths { get; private set; }
    public string Purpose { get; private set; }
    public decimal DeclaredMonthlyIncome { get; private set; }
    public decimal ExistingEmiObligations { get; private set; }
    public ApplicationStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private LoanApplicationEntity()
    {
        // Required by EF Core
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

    public void ChangeStatus(ApplicationStatus newStatus)
    {
        // Status transition logic can be complex in real scenarios
        // For MVP, just allow transition and update timestamp
        Status = newStatus;
        UpdatedAt = DateTime.UtcNow;
    }
}
