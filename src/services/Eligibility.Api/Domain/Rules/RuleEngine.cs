using System.Collections.Generic;
using Eligibility.Api.Domain.Models;

namespace Eligibility.Api.Domain.Rules;

public class RuleEngine
{
    private readonly IEnumerable<IEligibilityRule> _rules;

    public string Version => "1.0.0";

    public RuleEngine(IEnumerable<IEligibilityRule> rules)
    {
        _rules = rules;
    }

    public EligibilityResult Evaluate(LoanApplicationData data)
    {
        var results = new List<RuleResult>();
        
        foreach (var rule in _rules)
        {
            results.Add(rule.Evaluate(data));
        }

        return new EligibilityResult(
            applicationId: data.ApplicationId,
            customerId: data.CustomerId,
            ruleVersion: Version,
            requestedAmount: data.RequestedAmount,
            declaredMonthlyIncome: data.DeclaredMonthlyIncome,
            existingEmiObligations: data.ExistingEmiObligations,
            ruleResults: results
        );
    }
}
