using System;
using FluentAssertions;
using LoanApplication.Api.Domain.Exceptions;
using LoanApplication.Api.Domain.Models;
using Xunit;

namespace LoanApplication.Api.Tests.Domain;

public class LoanApplicationEntityTests
{
    private static LoanApplicationEntity CreateValidApplication()
    {
        return new LoanApplicationEntity(
            Guid.NewGuid(),
            LoanType.PersonalLoan,
            100000m,
            24,
            "Home Renovation",
            50000m,
            5000m
        );
    }

    [Fact]
    public void ChangeStatus_ValidTransition_ShouldUpdateStatusAndAddHistory()
    {
        // Arrange
        var app = CreateValidApplication();
        app.Status.Should().Be(ApplicationStatus.Submitted);

        // Act
        app.ChangeStatus(ApplicationStatus.UnderReview, "Starting review", "System");

        // Assert
        app.Status.Should().Be(ApplicationStatus.UnderReview);
        app.StatusHistory.Should().HaveCount(2); // Initial (Submitted) + UnderReview
    }

    [Fact]
    public void ChangeStatus_InvalidTransition_ShouldThrowException()
    {
        // Arrange
        var app = CreateValidApplication();
        app.Status.Should().Be(ApplicationStatus.Submitted);

        // Act
        Action act = () => app.ChangeStatus(ApplicationStatus.Approved, "Direct approval not allowed", "System");

        // Assert
        act.Should().Throw<LoanApplicationDomainException>()
           .WithMessage("Invalid state transition from Submitted to Approved.");
        app.Status.Should().Be(ApplicationStatus.Submitted);
        app.StatusHistory.Should().HaveCount(1);
    }

    [Fact]
    public void ChangeStatus_SameStatus_ShouldIgnore()
    {
        // Arrange
        var app = CreateValidApplication();

        // Act
        app.ChangeStatus(ApplicationStatus.Submitted, "No op", "System");

        // Assert
        app.Status.Should().Be(ApplicationStatus.Submitted);
        app.StatusHistory.Should().HaveCount(1);
    }

    [Theory]
    [InlineData(ApplicationStatus.Submitted, ApplicationStatus.UnderReview)]
    [InlineData(ApplicationStatus.Submitted, ApplicationStatus.Cancelled)]
    public void ValidTransitionsFromSubmitted_ShouldSucceed(ApplicationStatus current, ApplicationStatus next)
    {
        var app = CreateValidApplication();
        // Since CreateValidApplication returns an app with Submitted status
        app.ChangeStatus(next, "Testing", "System");
        app.Status.Should().Be(next);
    }
}
