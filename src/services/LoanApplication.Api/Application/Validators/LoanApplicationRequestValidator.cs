using FluentValidation;
using LoanApplication.Api.Application.DTOs;

namespace LoanApplication.Api.Application.Validators;

public class LoanApplicationRequestValidator : AbstractValidator<LoanApplicationRequest>
{
    public LoanApplicationRequestValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("Customer ID is required.");

        RuleFor(x => x.LoanType)
            .IsInEnum().WithMessage("Loan type is required and must be valid.");

        RuleFor(x => x.RequestedAmount)
            .GreaterThan(0).WithMessage("Requested amount must be greater than zero.");

        RuleFor(x => x.RequestedTenureInMonths)
            .InclusiveBetween(6, 84).WithMessage("Requested tenure must be between 6 and 84 months.");

        RuleFor(x => x.Purpose)
            .NotEmpty().WithMessage("Loan purpose is required.");

        RuleFor(x => x.DeclaredMonthlyIncome)
            .GreaterThan(0).WithMessage("Declared monthly income must be greater than zero.");

        RuleFor(x => x.ExistingEmiObligations)
            .GreaterThanOrEqualTo(0).WithMessage("Existing EMI obligations cannot be negative.")
            .LessThanOrEqualTo(x => x.DeclaredMonthlyIncome).WithMessage("Existing EMI obligations should not be greater than declared monthly income.");
    }
}
