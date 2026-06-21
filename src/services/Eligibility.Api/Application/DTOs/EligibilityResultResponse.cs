using System;
using System.Collections.Generic;

namespace Eligibility.Api.Application.DTOs;

public record EligibilityResultResponse(
    Guid Id,
    Guid ApplicationId,
    Guid CustomerId,
    string Decision,
    string RuleVersion,
    DateTime EvaluatedAt,
    decimal RequestedAmount,
    decimal DeclaredMonthlyIncome,
    decimal ExistingEmiObligations,
    decimal DebtToIncomeRatio,
    string DecisionSummary,
    IEnumerable<RuleResultResponse> RuleResults
);
