using System;
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

public class LoanApplicationServiceTests : IDisposable
{
    private readonly LoanApplicationDbContext _dbContext;
    private readonly Mock<ICustomerLookupService> _mockCustomerLookup;
    private readonly LoanApplicationService _service;

    public LoanApplicationServiceTests()
    {
        var options = new DbContextOptionsBuilder<LoanApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new LoanApplicationDbContext(options);
        _mockCustomerLookup = new Mock<ICustomerLookupService>();
        
        _service = new LoanApplicationService(_dbContext, _mockCustomerLookup.Object);
    }

    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    private LoanApplicationRequest CreateValidRequest() =>
        new LoanApplicationRequest(
            CustomerId: Guid.NewGuid(),
            LoanType: LoanType.PersonalLoan,
            RequestedAmount: 10000m,
            RequestedTenureInMonths: 24,
            Purpose: "Home Renovation",
            DeclaredMonthlyIncome: 5000m,
            ExistingEmiObligations: 500m
        );

    [Fact]
    public async Task SubmitApplicationAsync_ShouldCreateApplication_WhenValid()
    {
        // Arrange
        var request = CreateValidRequest();
        _mockCustomerLookup
            .Setup(x => x.CustomerExistsAsync(request.CustomerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _service.SubmitApplicationAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().NotBeEmpty();
        result.Status.Should().Be(ApplicationStatus.Submitted);
        
        var dbEntity = await _dbContext.LoanApplications.FirstOrDefaultAsync(x => x.Id == result.Id);
        dbEntity.Should().NotBeNull();
    }

    [Fact]
    public async Task SubmitApplicationAsync_ShouldThrowException_WhenCustomerDoesNotExist()
    {
        // Arrange
        var request = CreateValidRequest();
        _mockCustomerLookup
            .Setup(x => x.CustomerExistsAsync(request.CustomerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        Func<Task> action = async () => await _service.SubmitApplicationAsync(request);

        // Assert
        await action.Should().ThrowAsync<LoanApplicationDomainException>()
            .WithMessage($"Customer with ID {request.CustomerId} does not exist.");
    }

    [Fact]
    public async Task GetApplicationByIdAsync_ShouldReturnApplication_WhenExists()
    {
        // Arrange
        var request = CreateValidRequest();
        _mockCustomerLookup
            .Setup(x => x.CustomerExistsAsync(request.CustomerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var created = await _service.SubmitApplicationAsync(request);

        // Act
        var result = await _service.GetApplicationByIdAsync(created.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(created.Id);
    }

    [Fact]
    public async Task GetApplicationByIdAsync_ShouldReturnNull_WhenDoesNotExist()
    {
        // Act
        var result = await _service.GetApplicationByIdAsync(Guid.NewGuid());

        // Assert
        result.Should().BeNull();
    }
}
