using System;
using System.Threading;
using System.Threading.Tasks;
using Auditing;
using Eligibility.Api.Application.DTOs;
using Eligibility.Api.Application.Services;
using Eligibility.Api.Domain.Exceptions;
using Eligibility.Api.Domain.Rules;
using Eligibility.Api.Infrastructure.Data;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Observability;
using Xunit;

namespace Eligibility.Api.Tests.Application;

public class EligibilityServiceTests
{
    private readonly DbContextOptions<EligibilityDbContext> _dbContextOptions;

    public EligibilityServiceTests()
    {
        _dbContextOptions = new DbContextOptionsBuilder<EligibilityDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    private static EligibilityService CreateService(EligibilityDbContext dbContext, ILoanApplicationClient loanApplicationClient, IRuleEngine ruleEngine)
    {
        var auditLoggerMock = new Mock<IAuditLogger>();
        auditLoggerMock
            .Setup(a => a.LogAsync(It.IsAny<AuditEventRecord>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        return new EligibilityService(
            dbContext,
            loanApplicationClient,
            new Mock<ILogger<EligibilityService>>().Object,
            new CorrelationIdProvider(new Mock<IHttpContextAccessor>().Object),
            auditLoggerMock.Object,
            ruleEngine);
    }

    [Fact]
    public async Task EvaluateAsync_ShouldPass_WhenAllRulesPass()
    {
        // Arrange
        var mockClient = new Mock<ILoanApplicationClient>();
        var appId = Guid.NewGuid();
        var data = new LoanApplicationData(appId, Guid.NewGuid(), 100000, 24, 30000, 5000);
        
        mockClient.Setup(c => c.GetApplicationDataAsync(appId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(data);

        var rules = new IEligibilityRule[]
        {
            new MinimumIncomeRule(),
            new DebtToIncomeRule(),
            new MaximumAmountRule(),
            new TenureRule(),
            new EmiObligationRule()
        };
        var engine = new RuleEngine(rules);

        using var dbContext = new EligibilityDbContext(_dbContextOptions);
        var service = CreateService(dbContext, mockClient.Object, engine);

        var request = new EvaluateEligibilityRequest(appId);

        // Act
        var result = await service.EvaluateAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Decision.Should().Be("Passed");
        
        // Ensure it was saved to DB
        var savedResult = await dbContext.EligibilityResults.FirstOrDefaultAsync(r => r.ApplicationId == appId);
        savedResult.Should().NotBeNull();
        savedResult!.Decision.ToString().Should().Be("Passed");
    }

    [Fact]
    public async Task EvaluateAsync_ShouldFail_WhenOneRuleFails()
    {
        // Arrange
        var mockClient = new Mock<ILoanApplicationClient>();
        var appId = Guid.NewGuid();
        // Income is 10k, DTI is fine, but Minimum Income Rule requires 25k -> Fail
        var data = new LoanApplicationData(appId, Guid.NewGuid(), 50000, 24, 10000, 1000);
        
        mockClient.Setup(c => c.GetApplicationDataAsync(appId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(data);

        var rules = new IEligibilityRule[]
        {
            new MinimumIncomeRule(),
            new DebtToIncomeRule(),
            new MaximumAmountRule(),
            new TenureRule(),
            new EmiObligationRule()
        };
        var engine = new RuleEngine(rules);

        using var dbContext = new EligibilityDbContext(_dbContextOptions);
        var service = CreateService(dbContext, mockClient.Object, engine);

        var request = new EvaluateEligibilityRequest(appId);

        // Act
        var result = await service.EvaluateAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Decision.Should().Be("Failed");
    }

    [Fact]
    public async Task EvaluateAsync_ShouldThrowException_WhenApplicationNotFound()
    {
        // Arrange
        var mockClient = new Mock<ILoanApplicationClient>();
        var appId = Guid.NewGuid();
        
        mockClient.Setup(c => c.GetApplicationDataAsync(appId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((LoanApplicationData?)null);

        var engine = new RuleEngine(Array.Empty<IEligibilityRule>());
        using var dbContext = new EligibilityDbContext(_dbContextOptions);
        var service = CreateService(dbContext, mockClient.Object, engine);

        var request = new EvaluateEligibilityRequest(appId);

        // Act
        Func<Task> act = async () => await service.EvaluateAsync(request);

        // Assert
        await act.Should().ThrowAsync<SharedKernel.Exceptions.NotFoundException>();
    }
}
