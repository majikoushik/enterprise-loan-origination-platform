using Eligibility.Api.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Eligibility.Api.Infrastructure.Data;

internal static class SyntheticEligibilityDataSeeder
{
    public static readonly Guid PrimaryCustomerId = Guid.Parse("11111111-1111-1111-1111-111111111111");
    public static readonly Guid SecondaryCustomerId = Guid.Parse("22222222-2222-2222-2222-222222222222");
    public static readonly Guid PassedApplicationId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1");
    public static readonly Guid FailedApplicationId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2");

    public static async Task SeedAsync(EligibilityDbContext dbContext, CancellationToken cancellationToken = default)
    {
        if (await dbContext.EligibilityResults.AnyAsync(cancellationToken))
        {
            return;
        }

        dbContext.EligibilityResults.AddRange(
            CreatePassedResult(),
            CreateFailedResult());

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private static EligibilityResult CreatePassedResult()
    {
        return new EligibilityResult(
            PassedApplicationId,
            PrimaryCustomerId,
            "1.0.0",
            185000m,
            9250m,
            1850m,
            new[]
            {
                new RuleResult("MIN_INCOME", "Minimum monthly income", true, "9250", ">= 3000", "Declared income meets the minimum threshold."),
                new RuleResult("DTI", "Debt-to-income ratio", true, "20.00%", "<= 40.00%", "Existing obligations are within the accepted ratio."),
                new RuleResult("MAX_AMOUNT", "Requested amount limit", true, "185000", "<= 250000", "Requested amount is within the automated eligibility cap."),
                new RuleResult("TENURE", "Tenure limit", true, "48 months", "6-84 months", "Requested tenure is within the supported range.")
            });
    }

    private static EligibilityResult CreateFailedResult()
    {
        return new EligibilityResult(
            FailedApplicationId,
            SecondaryCustomerId,
            "1.0.0",
            420000m,
            6400m,
            3200m,
            new[]
            {
                new RuleResult("MIN_INCOME", "Minimum monthly income", true, "6400", ">= 3000", "Declared income meets the minimum threshold."),
                new RuleResult("DTI", "Debt-to-income ratio", false, "50.00%", "<= 40.00%", "Existing obligations exceed the accepted debt-to-income ratio."),
                new RuleResult("MAX_AMOUNT", "Requested amount limit", false, "420000", "<= 250000", "Requested amount exceeds the automated eligibility cap."),
                new RuleResult("TENURE", "Tenure limit", true, "84 months", "6-84 months", "Requested tenure is within the supported range.")
            });
    }
}
