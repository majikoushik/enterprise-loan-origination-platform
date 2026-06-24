using Customer.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace Customer.Api.Infrastructure.Data;

internal static class SyntheticCustomerDataSeeder
{
    public static readonly Guid PrimaryCustomerId = Guid.Parse("11111111-1111-1111-1111-111111111111");
    public static readonly Guid SecondaryCustomerId = Guid.Parse("22222222-2222-2222-2222-222222222222");
    public static readonly Guid ReviewCustomerId = Guid.Parse("33333333-3333-3333-3333-333333333333");

    public static async Task SeedAsync(CustomerDbContext dbContext, CancellationToken cancellationToken = default)
    {
        var customerSpecs = new[]
        {
            new CustomerSpec(PrimaryCustomerId, "Anika Rao", "anika.rao@example.com", "+1-555-0101", new DateTime(1988, 4, 12, 0, 0, 0, DateTimeKind.Utc), EmploymentType.Salaried, 9250m, 1850m, new DateTime(2026, 1, 10, 9, 15, 0, DateTimeKind.Utc)),
            new CustomerSpec(SecondaryCustomerId, "Marcus Lee", "marcus.lee@example.com", "+1-555-0102", new DateTime(1982, 11, 3, 0, 0, 0, DateTimeKind.Utc), EmploymentType.SelfEmployed, 6400m, 3200m, new DateTime(2026, 1, 11, 11, 30, 0, DateTimeKind.Utc)),
            new CustomerSpec(ReviewCustomerId, "Priya Shah", "priya.shah@example.com", "+1-555-0103", new DateTime(1993, 7, 22, 0, 0, 0, DateTimeKind.Utc), EmploymentType.Salaried, 7800m, 900m, new DateTime(2026, 1, 12, 14, 45, 0, DateTimeKind.Utc))
        };

        foreach (var spec in customerSpecs)
        {
            var existing = await dbContext.Customers.FirstOrDefaultAsync(c => c.Id == spec.Id, cancellationToken);
            if (existing is null)
            {
                dbContext.Customers.Add(CreateCustomer(spec));
            }
            else
            {
                ApplySpec(existing, spec);
            }
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private static CustomerProfile CreateCustomer(CustomerSpec spec)
    {
        var customer = new CustomerProfile(spec.FullName, spec.Email, spec.MobileNumber, spec.DateOfBirth, spec.EmploymentType, spec.MonthlyIncome, spec.ExistingMonthlyObligations);
        ApplySpec(customer, spec);
        return customer;
    }

    private static void ApplySpec(CustomerProfile customer, CustomerSpec spec)
    {
        typeof(CustomerProfile).GetProperty(nameof(CustomerProfile.Id))!.SetValue(customer, spec.Id);
        typeof(CustomerProfile).GetProperty(nameof(CustomerProfile.FullName))!.SetValue(customer, spec.FullName);
        typeof(CustomerProfile).GetProperty(nameof(CustomerProfile.Email))!.SetValue(customer, spec.Email);
        typeof(CustomerProfile).GetProperty(nameof(CustomerProfile.MobileNumber))!.SetValue(customer, spec.MobileNumber);
        typeof(CustomerProfile).GetProperty(nameof(CustomerProfile.DateOfBirth))!.SetValue(customer, spec.DateOfBirth);
        typeof(CustomerProfile).GetProperty(nameof(CustomerProfile.EmploymentType))!.SetValue(customer, spec.EmploymentType);
        typeof(CustomerProfile).GetProperty(nameof(CustomerProfile.MonthlyIncome))!.SetValue(customer, spec.MonthlyIncome);
        typeof(CustomerProfile).GetProperty(nameof(CustomerProfile.ExistingMonthlyObligations))!.SetValue(customer, spec.ExistingMonthlyObligations);
        typeof(CustomerProfile).GetProperty(nameof(CustomerProfile.CreatedAt))!.SetValue(customer, spec.CreatedAt);
    }

    private sealed record CustomerSpec(Guid Id, string FullName, string Email, string MobileNumber, DateTime DateOfBirth, EmploymentType EmploymentType, decimal MonthlyIncome, decimal ExistingMonthlyObligations, DateTime CreatedAt);
}
