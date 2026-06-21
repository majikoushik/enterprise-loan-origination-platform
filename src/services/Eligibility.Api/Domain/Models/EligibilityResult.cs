using System;
using System.Collections.Generic;
using System.Linq;

namespace Eligibility.Api.Domain.Models;

public class EligibilityResult
{
    public Guid Id { get; private set; }
    public Guid ApplicationId { get; private set; }
    public Guid CustomerId { get; private set; }
    public EligibilityDecision Decision { get; private set; }
    public string RuleVersion { get; private set; } = string.Empty;
    public DateTime EvaluatedAt { get; private set; }
    
    // Snapshot of evaluated values
    public decimal RequestedAmount { get; private set; }
    public decimal DeclaredMonthlyIncome { get; private set; }
    public decimal ExistingEmiObligations { get; private set; }
    public decimal DebtToIncomeRatio { get; private set; }
    
    public string DecisionSummary { get; private set; } = string.Empty;

    public ICollection<RuleResult> RuleResults { get; private set; }

    private EligibilityResult()
    {
        RuleResults = new List<RuleResult>();
    }

    public EligibilityResult(
        Guid applicationId,
        Guid customerId,
        string ruleVersion,
        decimal requestedAmount,
        decimal declaredMonthlyIncome,
        decimal existingEmiObligations,
        IEnumerable<RuleResult> ruleResults)
    {
        Id = Guid.NewGuid();
        ApplicationId = applicationId;
        CustomerId = customerId;
        RuleVersion = ruleVersion ?? throw new ArgumentNullException(nameof(ruleVersion));
        RequestedAmount = requestedAmount;
        DeclaredMonthlyIncome = declaredMonthlyIncome;
        ExistingEmiObligations = existingEmiObligations;
        EvaluatedAt = DateTime.UtcNow;

        DebtToIncomeRatio = declaredMonthlyIncome > 0 
            ? Math.Round((existingEmiObligations / declaredMonthlyIncome) * 100, 2) 
            : 0;

        RuleResults = ruleResults?.ToList() ?? new List<RuleResult>();

        // Decision Logic
        if (RuleResults.All(r => r.Passed))
        {
            Decision = EligibilityDecision.Passed;
            DecisionSummary = "All eligibility rules passed.";
        }
        else
        {
            Decision = EligibilityDecision.Failed;
            var failedRulesCount = RuleResults.Count(r => !r.Passed);
            DecisionSummary = $"{failedRulesCount} eligibility rule(s) failed.";
        }
    }
}
