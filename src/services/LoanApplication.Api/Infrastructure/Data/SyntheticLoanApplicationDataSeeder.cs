using LoanApplication.Api.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace LoanApplication.Api.Infrastructure.Data;

internal static class SyntheticLoanApplicationDataSeeder
{
    public static readonly Guid PrimaryCustomerId = Guid.Parse("11111111-1111-1111-1111-111111111111");
    public static readonly Guid SecondaryCustomerId = Guid.Parse("22222222-2222-2222-2222-222222222222");
    public static readonly Guid ReviewCustomerId = Guid.Parse("33333333-3333-3333-3333-333333333333");

    public static readonly Guid PassedApplicationId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1");
    public static readonly Guid FailedApplicationId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2");
    public static readonly Guid ReviewApplicationId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3");

    public static async Task SeedAsync(LoanApplicationDbContext dbContext, CancellationToken cancellationToken = default)
    {
        if (await dbContext.LoanApplications.AnyAsync(cancellationToken))
        {
            return;
        }

        var passed = CreateApplication(PassedApplicationId, PrimaryCustomerId, LoanType.PersonalLoan, 185000m, 48, "Debt consolidation and home improvement", 9250m, 1850m, ApplicationStatus.EligibilityPassed, new DateTime(2026, 1, 13, 10, 0, 0, DateTimeKind.Utc));
        var failed = CreateApplication(FailedApplicationId, SecondaryCustomerId, LoanType.PersonalLoan, 420000m, 84, "Business cash flow support", 6400m, 3200m, ApplicationStatus.EligibilityFailed, new DateTime(2026, 1, 13, 12, 20, 0, DateTimeKind.Utc));
        var underReview = CreateApplication(ReviewApplicationId, ReviewCustomerId, LoanType.HomeLoan, 275000m, 60, "Home renovation", 7800m, 900m, ApplicationStatus.UnderReview, new DateTime(2026, 1, 14, 9, 5, 0, DateTimeKind.Utc));

        dbContext.LoanApplications.AddRange(passed, failed, underReview);
        dbContext.ApplicationStatusHistories.AddRange(
            CreateHistory(PassedApplicationId, null, ApplicationStatus.Submitted, "Synthetic application submitted", "System", new DateTime(2026, 1, 13, 10, 0, 0, DateTimeKind.Utc)),
            CreateHistory(PassedApplicationId, ApplicationStatus.Submitted, ApplicationStatus.UnderReview, "Automated completeness check passed", "System", new DateTime(2026, 1, 13, 10, 8, 0, DateTimeKind.Utc)),
            CreateHistory(PassedApplicationId, ApplicationStatus.UnderReview, ApplicationStatus.EligibilityPassed, "Rule-based eligibility passed", "Eligibility.Api", new DateTime(2026, 1, 13, 10, 12, 0, DateTimeKind.Utc)),
            CreateHistory(FailedApplicationId, null, ApplicationStatus.Submitted, "Synthetic application submitted", "System", new DateTime(2026, 1, 13, 12, 20, 0, DateTimeKind.Utc)),
            CreateHistory(FailedApplicationId, ApplicationStatus.Submitted, ApplicationStatus.UnderReview, "Automated completeness check passed", "System", new DateTime(2026, 1, 13, 12, 26, 0, DateTimeKind.Utc)),
            CreateHistory(FailedApplicationId, ApplicationStatus.UnderReview, ApplicationStatus.EligibilityFailed, "Debt-to-income and amount rules failed", "Eligibility.Api", new DateTime(2026, 1, 13, 12, 31, 0, DateTimeKind.Utc)),
            CreateHistory(ReviewApplicationId, null, ApplicationStatus.Submitted, "Synthetic application submitted", "System", new DateTime(2026, 1, 14, 9, 5, 0, DateTimeKind.Utc)),
            CreateHistory(ReviewApplicationId, ApplicationStatus.Submitted, ApplicationStatus.UnderReview, "Awaiting underwriting review", "LoanOfficer", new DateTime(2026, 1, 14, 9, 18, 0, DateTimeKind.Utc)));

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private static LoanApplicationEntity CreateApplication(Guid id, Guid customerId, LoanType loanType, decimal amount, int tenure, string purpose, decimal income, decimal obligations, ApplicationStatus status, DateTime createdAt)
    {
        var application = new LoanApplicationEntity(customerId, loanType, amount, tenure, purpose, income, obligations);
        typeof(LoanApplicationEntity).GetProperty(nameof(LoanApplicationEntity.Id))!.SetValue(application, id);
        typeof(LoanApplicationEntity).GetProperty(nameof(LoanApplicationEntity.Status))!.SetValue(application, status);
        typeof(LoanApplicationEntity).GetProperty(nameof(LoanApplicationEntity.CreatedAt))!.SetValue(application, createdAt);
        typeof(LoanApplicationEntity).GetProperty(nameof(LoanApplicationEntity.UpdatedAt))!.SetValue(application, createdAt.AddMinutes(18));

        var field = typeof(LoanApplicationEntity).GetField("_statusHistory", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        ((List<ApplicationStatusHistory>)field!.GetValue(application)!).Clear();
        return application;
    }

    private static ApplicationStatusHistory CreateHistory(Guid applicationId, ApplicationStatus? previousStatus, ApplicationStatus newStatus, string reason, string changedBy, DateTime changedAt)
    {
        var history = new ApplicationStatusHistory(applicationId, previousStatus, newStatus, reason, changedBy);
        typeof(ApplicationStatusHistory).GetProperty(nameof(ApplicationStatusHistory.ChangedAt))!.SetValue(history, changedAt);
        return history;
    }
}
