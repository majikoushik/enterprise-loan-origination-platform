using System;
using LoanApplication.Api.Domain.Models;

namespace LoanApplication.Api.Application.DTOs;

public record LoanApplicationRequest(
    Guid CustomerId,
    LoanType LoanType,
    decimal RequestedAmount,
    int RequestedTenureInMonths,
    string Purpose,
    decimal DeclaredMonthlyIncome,
    decimal ExistingEmiObligations
);
