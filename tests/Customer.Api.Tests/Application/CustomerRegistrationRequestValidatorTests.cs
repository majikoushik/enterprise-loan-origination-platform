using System;
using Customer.Api.Application.DTOs;
using Customer.Api.Application.Validators;
using Customer.Api.Domain;
using FluentAssertions;
using Xunit;

namespace Customer.Api.Tests.Application;

public class CustomerRegistrationRequestValidatorTests
{
    private readonly CustomerRegistrationRequestValidator _validator;

    public CustomerRegistrationRequestValidatorTests()
    {
        _validator = new CustomerRegistrationRequestValidator();
    }

    [Fact]
    public void Should_Have_Error_When_FullName_Is_Empty()
    {
        var request = CreateValidRequest() with { FullName = string.Empty };
        var result = _validator.Validate(request);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "FullName");
    }

    [Fact]
    public void Should_Have_Error_When_Email_Is_Invalid()
    {
        var request = CreateValidRequest() with { Email = "not-an-email" };
        var result = _validator.Validate(request);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Email");
    }

    [Fact]
    public void Should_Have_Error_When_Under_18()
    {
        var request = CreateValidRequest() with { DateOfBirth = DateTime.UtcNow.AddYears(-17) };
        var result = _validator.Validate(request);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "DateOfBirth");
    }

    [Fact]
    public void Should_Have_Error_When_Income_Is_Zero()
    {
        var request = CreateValidRequest() with { MonthlyIncome = 0 };
        var result = _validator.Validate(request);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "MonthlyIncome");
    }

    [Fact]
    public void Should_Have_Error_When_Obligations_Greater_Than_Income()
    {
        var request = CreateValidRequest() with { MonthlyIncome = 5000, ExistingMonthlyObligations = 6000 };
        var result = _validator.Validate(request);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "ExistingMonthlyObligations");
    }

    [Fact]
    public void Should_Be_Valid_When_All_Rules_Met()
    {
        var request = CreateValidRequest();
        var result = _validator.Validate(request);
        result.IsValid.Should().BeTrue();
    }

    private static CustomerRegistrationRequest CreateValidRequest()
    {
        return new CustomerRegistrationRequest(
            "John Doe",
            "john@example.com",
            "+1234567890",
            new DateTime(1990, 1, 1),
            EmploymentType.Salaried,
            5000,
            1000
        );
    }
}
