using System;

namespace Customer.Api.Domain;

public class CustomerProfile
{
    public Guid Id { get; private set; }
    public string FullName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string MobileNumber { get; private set; } = string.Empty;
    public DateTime DateOfBirth { get; private set; }
    public EmploymentType EmploymentType { get; private set; }
    public decimal MonthlyIncome { get; private set; }
    public decimal ExistingMonthlyObligations { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private CustomerProfile()
    {
        // Required by EF Core
    }

    public CustomerProfile(
        string fullName,
        string email,
        string mobileNumber,
        DateTime dateOfBirth,
        EmploymentType employmentType,
        decimal monthlyIncome,
        decimal existingMonthlyObligations)
    {
        Id = Guid.NewGuid();
        FullName = fullName ?? throw new ArgumentNullException(nameof(fullName));
        Email = email ?? throw new ArgumentNullException(nameof(email));
        MobileNumber = mobileNumber ?? throw new ArgumentNullException(nameof(mobileNumber));
        DateOfBirth = dateOfBirth;
        EmploymentType = employmentType;
        MonthlyIncome = monthlyIncome;
        ExistingMonthlyObligations = existingMonthlyObligations;
        CreatedAt = DateTime.UtcNow;

        ValidateDomainRules();
    }

    private void ValidateDomainRules()
    {
        if (MonthlyIncome <= 0)
        {
            throw new CustomerDomainException("Monthly income must be greater than zero.");
        }

        if (ExistingMonthlyObligations < 0)
        {
            throw new CustomerDomainException("Existing monthly obligations cannot be negative.");
        }

        if (ExistingMonthlyObligations > MonthlyIncome)
        {
            throw new CustomerDomainException("Existing monthly obligations should not be greater than monthly income.");
        }

        var age = DateTime.UtcNow.Year - DateOfBirth.Year;
        if (DateOfBirth.Date > DateTime.UtcNow.AddYears(-age)) age--;
        
        if (age < 18)
        {
            throw new CustomerDomainException("Customer must be at least 18 years old.");
        }
    }
}
