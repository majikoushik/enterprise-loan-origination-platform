using Eligibility.Api.Domain.Models;

namespace Eligibility.Api.Domain.Rules;

public class TenureRule : IEligibilityRule
{
    private const int MinTenureMonths = 6;
    private const int MaxTenureMonths = 84;

    public RuleResult Evaluate(LoanApplicationData data)
    {
        var passed = data.RequestedTenureInMonths >= MinTenureMonths && data.RequestedTenureInMonths <= MaxTenureMonths;
        var explanation = passed 
            ? "Requested tenure is within the allowed range." 
            : $"Requested tenure must be between {MinTenureMonths} and {MaxTenureMonths} months.";

        return new RuleResult(
            ruleCode: "RUL-TEN-01",
            ruleName: "Tenure Range",
            passed: passed,
            actualValue: data.RequestedTenureInMonths.ToString(),
            expectedValue: $"{MinTenureMonths} - {MaxTenureMonths}",
            explanation: explanation
        );
    }
}
