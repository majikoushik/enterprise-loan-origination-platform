using Eligibility.Api.Domain.Models;

namespace Eligibility.Api.Domain.Rules;

public class EmiObligationRule : IEligibilityRule
{
    public RuleResult Evaluate(LoanApplicationData data)
    {
        var passed = data.ExistingEmiObligations <= data.DeclaredMonthlyIncome;
        var explanation = passed 
            ? "Existing EMI obligations do not exceed monthly income." 
            : "Existing EMI obligations are greater than the declared monthly income.";

        return new RuleResult(
            ruleCode: "RUL-EMI-01",
            ruleName: "EMI Obligation vs Income",
            passed: passed,
            actualValue: data.ExistingEmiObligations.ToString("F2"),
            expectedValue: $"<= {data.DeclaredMonthlyIncome:F2}",
            explanation: explanation
        );
    }
}
