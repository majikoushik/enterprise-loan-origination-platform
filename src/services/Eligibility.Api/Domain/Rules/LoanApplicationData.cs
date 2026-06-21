using System;

namespace Eligibility.Api.Domain.Rules;

public record LoanApplicationData(
    Guid ApplicationId,
    Guid CustomerId,
    decimal RequestedAmount,
    int RequestedTenureInMonths,
    decimal DeclaredMonthlyIncome,
    decimal ExistingEmiObligations
);
