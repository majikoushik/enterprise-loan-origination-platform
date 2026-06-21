using Eligibility.Api.Domain.Models;
using System;

namespace Eligibility.Api.Domain.Rules;

public class DebtToIncomeRule : IEligibilityRule
{
    private const decimal MaxDtiPercentage = 50.0m;

    public RuleResult Evaluate(LoanApplicationData data)
    {
        var dti = data.DeclaredMonthlyIncome > 0 
            ? (data.ExistingEmiObligations / data.DeclaredMonthlyIncome) * 100 
            : 100m;
        
        var passed = dti <= MaxDtiPercentage;
        var explanation = passed 
            ? "Debt-to-income ratio is within acceptable limits." 
            : $"Debt-to-income ratio ({Math.Round(dti, 2)}%) exceeds the maximum allowed {MaxDtiPercentage}%.";

        return new RuleResult(
            ruleCode: "RUL-DTI-01",
            ruleName: "Maximum Debt-To-Income Ratio",
            passed: passed,
            actualValue: $"{Math.Round(dti, 2)}%",
            expectedValue: $"<= {MaxDtiPercentage}%",
            explanation: explanation
        );
    }
}
