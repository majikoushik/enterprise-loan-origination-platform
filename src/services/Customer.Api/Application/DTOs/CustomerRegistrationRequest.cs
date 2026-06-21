using System;
using Customer.Api.Domain;

namespace Customer.Api.Application.DTOs;

public record CustomerRegistrationRequest(
    string FullName,
    string Email,
    string MobileNumber,
    DateTime DateOfBirth,
    EmploymentType EmploymentType,
    decimal MonthlyIncome,
    decimal ExistingMonthlyObligations
);
