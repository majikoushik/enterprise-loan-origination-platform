using System;
using FluentValidation;
using Customer.Api.Application.DTOs;

namespace Customer.Api.Application.Validators;

public class CustomerRegistrationRequestValidator : AbstractValidator<CustomerRegistrationRequest>
{
    public CustomerRegistrationRequestValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Full name is required.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email must be valid.");

        RuleFor(x => x.MobileNumber)
            .NotEmpty().WithMessage("Mobile number is required.")
            .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Mobile number must be valid."); // Simple E.164-like validation for demo

        RuleFor(x => x.DateOfBirth)
            .NotEmpty().WithMessage("Date of birth is required.");

        RuleFor(x => x.EmploymentType)
            .IsInEnum().WithMessage("Employment type is required and must be valid.");

        RuleFor(x => x.MonthlyIncome)
            .GreaterThan(0).WithMessage("Monthly income must be greater than zero.");

        RuleFor(x => x.ExistingMonthlyObligations)
            .GreaterThanOrEqualTo(0).WithMessage("Existing monthly obligations cannot be negative.")
            .LessThanOrEqualTo(x => x.MonthlyIncome).WithMessage("Existing monthly obligations should not be greater than monthly income.");
        
        RuleFor(x => x.DateOfBirth)
            .Must(BeAtLeast18YearsOld).WithMessage("Customer must be at least 18 years old.");
    }

    private bool BeAtLeast18YearsOld(DateTime dateOfBirth)
    {
        var age = DateTime.UtcNow.Year - dateOfBirth.Year;
        if (dateOfBirth.Date > DateTime.UtcNow.AddYears(-age)) age--;
        return age >= 18;
    }
}
