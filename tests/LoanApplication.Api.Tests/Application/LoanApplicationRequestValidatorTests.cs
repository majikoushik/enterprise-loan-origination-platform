using System;
using FluentValidation.TestHelper;
using LoanApplication.Api.Application.DTOs;
using LoanApplication.Api.Application.Validators;
using LoanApplication.Api.Domain.Models;
using Xunit;

namespace LoanApplication.Api.Tests.Application;

public class LoanApplicationRequestValidatorTests
{
    private readonly LoanApplicationRequestValidator _validator;

    public LoanApplicationRequestValidatorTests()
    {
        _validator = new LoanApplicationRequestValidator();
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
    public void Should_NotHaveError_When_RequestIsValid()
    {
        var request = CreateValidRequest();
        var result = _validator.TestValidate(request);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Should_HaveError_When_CustomerIdIsEmpty()
    {
        var request = CreateValidRequest() with { CustomerId = Guid.Empty };
        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.CustomerId);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1000)]
    public void Should_HaveError_When_RequestedAmountIsInvalid(decimal invalidAmount)
    {
        var request = CreateValidRequest() with { RequestedAmount = invalidAmount };
        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.RequestedAmount);
    }

    [Theory]
    [InlineData(5)]
    [InlineData(85)]
    public void Should_HaveError_When_RequestedTenureIsInvalid(int invalidTenure)
    {
        var request = CreateValidRequest() with { RequestedTenureInMonths = invalidTenure };
        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.RequestedTenureInMonths);
    }

    [Fact]
    public void Should_HaveError_When_PurposeIsEmpty()
    {
        var request = CreateValidRequest() with { Purpose = "" };
        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Purpose);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-500)]
    public void Should_HaveError_When_DeclaredMonthlyIncomeIsInvalid(decimal invalidIncome)
    {
        var request = CreateValidRequest() with { DeclaredMonthlyIncome = invalidIncome };
        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.DeclaredMonthlyIncome);
    }

    [Fact]
    public void Should_HaveError_When_ExistingEmiObligationsIsNegative()
    {
        var request = CreateValidRequest() with { ExistingEmiObligations = -100m };
        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.ExistingEmiObligations);
    }

    [Fact]
    public void Should_HaveError_When_ExistingEmiObligationsExceedsIncome()
    {
        var request = CreateValidRequest() with 
        { 
            DeclaredMonthlyIncome = 5000m,
            ExistingEmiObligations = 6000m 
        };
        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.ExistingEmiObligations);
    }
}
