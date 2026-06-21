using Eligibility.Api.Domain.Models;

namespace Eligibility.Api.Domain.Rules;

public class MaximumAmountRule : IEligibilityRule
{
    private const int IncomeMultiplier = 20;

    public RuleResult Evaluate(LoanApplicationData data)
    {
        var maxAllowedAmount = data.DeclaredMonthlyIncome * IncomeMultiplier;
        var passed = data.RequestedAmount <= maxAllowedAmount;
        
        var explanation = passed 
            ? "Requested amount is within the allowed multiple of monthly income." 
            : $"Requested amount exceeds the maximum allowed multiple of monthly income ({IncomeMultiplier}x).";

        return new RuleResult(
            ruleCode: "RUL-AMT-01",
            ruleName: "Maximum Requested Amount",
            passed: passed,
            actualValue: data.RequestedAmount.ToString("F2"),
            expectedValue: $"<= {maxAllowedAmount:F2}",
            explanation: explanation
        );
    }
}
