using Microsoft.EntityFrameworkCore;
using Notification.Worker.Domain.Models;

namespace Notification.Worker.Infrastructure.Data;

internal static class SyntheticNotificationDataSeeder
{
    private static readonly Guid PrimaryCustomerId = Guid.Parse("11111111-1111-1111-1111-111111111111");
    private static readonly Guid SecondaryCustomerId = Guid.Parse("22222222-2222-2222-2222-222222222222");
    private static readonly Guid PassedApplicationId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1");
    private static readonly Guid FailedApplicationId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2");

    public static async Task SeedAsync(NotificationDbContext dbContext, CancellationToken cancellationToken = default)
    {
        if (await dbContext.NotificationRequests.AnyAsync(cancellationToken))
        {
            return;
        }

        var passedEmail = new NotificationRequest(Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb1"), "synthetic-demo-correlation", "EligibilityCheckCompleted", "LoanApplication", PassedApplicationId, PrimaryCustomerId, "anika.rao@example.com", NotificationChannel.Email, "Eligibility check completed", "Your synthetic demo loan application passed the rule-based eligibility check.");
        passedEmail.MarkSent("Synthetic email notification delivered.");

        var failedSms = new NotificationRequest(Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb2"), "synthetic-demo-correlation", "EligibilityCheckCompleted", "LoanApplication", FailedApplicationId, SecondaryCustomerId, "+1-555-0102", NotificationChannel.SMS, "Eligibility check completed", "Your synthetic demo loan application requires further review after eligibility evaluation.");
        failedSms.MarkSent("Synthetic SMS notification delivered.");

        dbContext.NotificationRequests.AddRange(passedEmail, failedSms);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
