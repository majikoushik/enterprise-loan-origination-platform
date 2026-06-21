using Eligibility.Api.Domain.Models;

namespace Eligibility.Api.Domain.Rules;

public class MinimumIncomeRule : IEligibilityRule
{
    private const decimal RequiredIncome = 25000m;

    public RuleResult Evaluate(LoanApplicationData data)
    {
        var passed = data.DeclaredMonthlyIncome >= RequiredIncome;
        var explanation = passed 
            ? "Income meets the minimum requirement." 
            : $"Income is below the minimum required {RequiredIncome}.";

        return new RuleResult(
            ruleCode: "RUL-INC-01",
            ruleName: "Minimum Monthly Income",
            passed: passed,
            actualValue: data.DeclaredMonthlyIncome.ToString("F2"),
            expectedValue: $">= {RequiredIncome}",
            explanation: explanation
        );
    }
}
