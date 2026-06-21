using System;

namespace Eligibility.Api.Domain.Models;

public class RuleResult
{
    public Guid Id { get; private set; }
    public string RuleCode { get; private set; }
    public string RuleName { get; private set; }
    public bool Passed { get; private set; }
    public string ActualValue { get; private set; }
    public string ExpectedValue { get; private set; }
    public string Explanation { get; private set; }

    private RuleResult() { } // EF Core

    public RuleResult(string ruleCode, string ruleName, bool passed, string actualValue, string expectedValue, string explanation)
    {
        Id = Guid.NewGuid();
        RuleCode = ruleCode ?? throw new ArgumentNullException(nameof(ruleCode));
        RuleName = ruleName ?? throw new ArgumentNullException(nameof(ruleName));
        Passed = passed;
        ActualValue = actualValue ?? throw new ArgumentNullException(nameof(actualValue));
        ExpectedValue = expectedValue ?? throw new ArgumentNullException(nameof(expectedValue));
        Explanation = explanation ?? throw new ArgumentNullException(nameof(explanation));
    }
}
