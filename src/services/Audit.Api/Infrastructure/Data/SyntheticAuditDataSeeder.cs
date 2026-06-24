using Audit.Api.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Audit.Api.Infrastructure.Data;

internal static class SyntheticAuditDataSeeder
{
    private static readonly Guid PrimaryCustomerId = Guid.Parse("11111111-1111-1111-1111-111111111111");
    private static readonly Guid SecondaryCustomerId = Guid.Parse("22222222-2222-2222-2222-222222222222");
    private static readonly Guid ReviewCustomerId = Guid.Parse("33333333-3333-3333-3333-333333333333");
    private static readonly Guid PassedApplicationId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1");
    private static readonly Guid FailedApplicationId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2");
    private static readonly Guid ReviewApplicationId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3");

    public static async Task SeedAsync(AuditDbContext dbContext, CancellationToken cancellationToken = default)
    {
        if (await dbContext.AuditEvents.AnyAsync(cancellationToken))
        {
            return;
        }

        dbContext.AuditEvents.AddRange(
            Create("CustomerRegistered", "Customer", "Customer", PrimaryCustomerId.ToString(), PrimaryCustomerId, "Register", "Customer registered: Anika Rao", "Customer.Api", new DateTimeOffset(2026, 1, 10, 9, 15, 0, TimeSpan.Zero)),
            Create("CustomerRegistered", "Customer", "Customer", SecondaryCustomerId.ToString(), SecondaryCustomerId, "Register", "Customer registered: Marcus Lee", "Customer.Api", new DateTimeOffset(2026, 1, 11, 11, 30, 0, TimeSpan.Zero)),
            Create("CustomerRegistered", "Customer", "Customer", ReviewCustomerId.ToString(), ReviewCustomerId, "Register", "Customer registered: Priya Shah", "Customer.Api", new DateTimeOffset(2026, 1, 12, 14, 45, 0, TimeSpan.Zero)),
            Create("LoanApplicationSubmitted", "LoanApplication", "LoanApplication", PassedApplicationId.ToString(), PrimaryCustomerId, "Submit", "Loan application submitted for 185000", "LoanApplication.Api", new DateTimeOffset(2026, 1, 13, 10, 0, 0, TimeSpan.Zero)),
            Create("EligibilityCheckCompleted", "Eligibility", "LoanApplication", PassedApplicationId.ToString(), PrimaryCustomerId, "Evaluate", "Eligibility check completed. Result: Passed", "Eligibility.Api", new DateTimeOffset(2026, 1, 13, 10, 12, 0, TimeSpan.Zero)),
            Create("LoanApplicationSubmitted", "LoanApplication", "LoanApplication", FailedApplicationId.ToString(), SecondaryCustomerId, "Submit", "Loan application submitted for 420000", "LoanApplication.Api", new DateTimeOffset(2026, 1, 13, 12, 20, 0, TimeSpan.Zero)),
            Create("EligibilityCheckCompleted", "Eligibility", "LoanApplication", FailedApplicationId.ToString(), SecondaryCustomerId, "Evaluate", "Eligibility check completed. Result: Failed", "Eligibility.Api", new DateTimeOffset(2026, 1, 13, 12, 31, 0, TimeSpan.Zero)),
            Create("LoanApplicationStatusChanged", "StatusTransition", "LoanApplication", ReviewApplicationId.ToString(), ReviewCustomerId, "ChangeStatus", "Loan application moved to UnderReview", "LoanApplication.Api", new DateTimeOffset(2026, 1, 14, 9, 18, 0, TimeSpan.Zero)));

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private static AuditEvent Create(string eventType, string category, string entityType, string entityId, Guid customerId, string action, string summary, string sourceService, DateTimeOffset occurredAt)
    {
        return new AuditEvent(
            Guid.NewGuid(),
            "synthetic-demo-correlation",
            eventType,
            category,
            entityType,
            entityId,
            customerId,
            "System",
            "SyntheticSeed",
            action,
            summary,
            "{}",
            occurredAt,
            sourceService,
            "Info");
    }
}
