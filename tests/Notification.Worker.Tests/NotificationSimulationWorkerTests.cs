using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Auditing;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Notification.Worker.Domain.Models;
using Notification.Worker.Infrastructure.Data;
using Xunit;

namespace Notification.Worker.Tests;

public class NotificationSimulationWorkerTests
{
    [Fact]
    public async Task ProcessPendingNotificationsAsync_ShouldMarkPendingRequestAsSent_WhenProviderSimulationSucceeds()
    {
        var databaseName = Guid.NewGuid().ToString();
        var databaseRoot = new InMemoryDatabaseRoot();
        var requestId = await SeedRequestAsync(databaseName, databaseRoot);
        var worker = CreateWorker(databaseName, databaseRoot, () => 0.9);

        await worker.ProcessPendingNotificationsAsync(CancellationToken.None);

        await using var dbContext = CreateDbContext(databaseName, databaseRoot);
        var request = await dbContext.NotificationRequests
            .Include(r => r.DeliveryAttempts)
            .SingleAsync(r => r.Id == requestId);

        request.Status.Should().Be(NotificationStatus.Sent);
        request.RetryCount.Should().Be(0);
        request.DeliveryAttempts.Should().ContainSingle(a => a.Status == NotificationStatus.Sent);
    }

    [Fact]
    public async Task ProcessPendingNotificationsAsync_ShouldScheduleRetry_WhenProviderSimulationFailsBelowRetryLimit()
    {
        var databaseName = Guid.NewGuid().ToString();
        var databaseRoot = new InMemoryDatabaseRoot();
        var requestId = await SeedRequestAsync(databaseName, databaseRoot);
        var worker = CreateWorker(databaseName, databaseRoot, () => 0.01);

        await worker.ProcessPendingNotificationsAsync(CancellationToken.None);

        await using var dbContext = CreateDbContext(databaseName, databaseRoot);
        var request = await dbContext.NotificationRequests
            .Include(r => r.DeliveryAttempts)
            .SingleAsync(r => r.Id == requestId);

        request.Status.Should().Be(NotificationStatus.Pending);
        request.RetryCount.Should().Be(1);
        request.FailureReason.Should().Be("Simulated transient provider failure");
        request.DeliveryAttempts.Should().ContainSingle(a => a.Status == NotificationStatus.Failed);
    }

    [Fact]
    public async Task ProcessPendingNotificationsAsync_ShouldMarkPermanentlyFailed_WhenRetryLimitReached()
    {
        var databaseName = Guid.NewGuid().ToString();
        var databaseRoot = new InMemoryDatabaseRoot();
        var requestId = await SeedRequestAsync(databaseName, databaseRoot, retryCount: NotificationSimulationWorker.MaxRetryCount);
        var worker = CreateWorker(databaseName, databaseRoot, () => 0.01);

        await worker.ProcessPendingNotificationsAsync(CancellationToken.None);

        await using var dbContext = CreateDbContext(databaseName, databaseRoot);
        var request = await dbContext.NotificationRequests
            .Include(r => r.DeliveryAttempts)
            .SingleAsync(r => r.Id == requestId);

        request.Status.Should().Be(NotificationStatus.PermanentlyFailed);
        request.RetryCount.Should().Be(NotificationSimulationWorker.MaxRetryCount);
        request.FailureReason.Should().Be("Simulated random provider failure");
        request.DeliveryAttempts.Should().Contain(a => a.Status == NotificationStatus.PermanentlyFailed);
    }

    private static NotificationSimulationWorker CreateWorker(string databaseName, InMemoryDatabaseRoot databaseRoot, Func<double> randomValueFactory)
    {
        var services = new ServiceCollection();
        services.AddDbContext<NotificationDbContext>(options => options.UseInMemoryDatabase(databaseName, databaseRoot));

        var auditLogger = new Mock<IAuditLogger>();
        auditLogger
            .Setup(a => a.LogAsync(It.IsAny<AuditEventRecord>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        services.AddSingleton(auditLogger.Object);

        var serviceProvider = services.BuildServiceProvider();

        return new NotificationSimulationWorker(
            new Mock<ILogger<NotificationSimulationWorker>>().Object,
            serviceProvider,
            randomValueFactory,
            TimeSpan.Zero);
    }

    private static async Task<Guid> SeedRequestAsync(string databaseName, InMemoryDatabaseRoot databaseRoot, int retryCount = 0)
    {
        await using var dbContext = CreateDbContext(databaseName, databaseRoot);
        var request = new NotificationRequest(
            Guid.NewGuid(),
            "corr-test",
            "LoanApplicationStatusChanged",
            "LoanApplication",
            Guid.NewGuid(),
            Guid.NewGuid(),
            "customer@example.com",
            NotificationChannel.Email,
            "Loan Application Update",
            "Your loan application status changed.");

        for (var retry = 0; retry < retryCount; retry++)
        {
            request.MarkForRetry("Pre-existing retry", "500 Server Error");
        }

        dbContext.NotificationRequests.Add(request);
        await dbContext.SaveChangesAsync();
        return request.Id;
    }

    private static NotificationDbContext CreateDbContext(string databaseName, InMemoryDatabaseRoot databaseRoot)
    {
        var options = new DbContextOptionsBuilder<NotificationDbContext>()
            .UseInMemoryDatabase(databaseName, databaseRoot)
            .Options;

        return new NotificationDbContext(options);
    }
}
