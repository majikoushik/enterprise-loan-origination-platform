using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using LoanApplication.Api.Application.DTOs;
using LoanApplication.Api.Application.Services;
using LoanApplication.Api.Domain.Exceptions;
using LoanApplication.Api.Domain.Models;
using LoanApplication.Api.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace LoanApplication.Api.Tests.Application;

public class LoanApplicationServiceTests
{
    private readonly DbContextOptions<LoanApplicationDbContext> _dbContextOptions;

    public LoanApplicationServiceTests()
    {
        _dbContextOptions = new DbContextOptionsBuilder<LoanApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public async Task UpdateStatusAsync_ValidTransition_ShouldUpdateAndReturnResponse()
    {
        // Arrange
        using var dbContext = new LoanApplicationDbContext(_dbContextOptions);
        var customerLookupServiceMock = new Mock<ICustomerLookupService>();
        
        var app = new LoanApplicationEntity(Guid.NewGuid(), LoanType.PersonalLoan, 100000m, 24, "Test", 50000m, 5000m);
        dbContext.LoanApplications.Add(app);
        await dbContext.SaveChangesAsync();

        var service = new LoanApplicationService(dbContext, customerLookupServiceMock.Object);

        var request = new UpdateApplicationStatusRequest
        {
            NewStatus = "UnderReview",
            Reason = "User triggered review",
            ChangedBy = "Admin"
        };

        // Act
        var result = await service.UpdateStatusAsync(app.Id, request);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(ApplicationStatus.UnderReview);

        var history = await service.GetStatusHistoryAsync(app.Id);
        history.Should().HaveCount(2); // Submitted + UnderReview
        history.First().NewStatus.Should().Be("UnderReview"); // It orders descending
    }

    [Fact]
    public async Task UpdateStatusAsync_InvalidTransition_ShouldThrowException()
    {
        // Arrange
        using var dbContext = new LoanApplicationDbContext(_dbContextOptions);
        var customerLookupServiceMock = new Mock<ICustomerLookupService>();
        
        var app = new LoanApplicationEntity(Guid.NewGuid(), LoanType.PersonalLoan, 100000m, 24, "Test", 50000m, 5000m);
        dbContext.LoanApplications.Add(app);
        await dbContext.SaveChangesAsync();

        var service = new LoanApplicationService(dbContext, customerLookupServiceMock.Object);

        var request = new UpdateApplicationStatusRequest
        {
            NewStatus = "Approved",
            Reason = "Invalid direct approval",
            ChangedBy = "Admin"
        };

        // Act
        Func<Task> act = async () => await service.UpdateStatusAsync(app.Id, request);

        // Assert
        await act.Should().ThrowAsync<LoanApplicationDomainException>()
            .WithMessage("Invalid state transition from Submitted to Approved.");
    }
}
