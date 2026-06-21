using System;

namespace Eligibility.Api.Application.DTOs;

public record RuleResultResponse(
    string RuleCode,
    string RuleName,
    bool Passed,
    string ActualValue,
    string ExpectedValue,
    string Explanation
);
