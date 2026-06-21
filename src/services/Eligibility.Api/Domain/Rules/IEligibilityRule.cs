using Eligibility.Api.Domain.Models;

namespace Eligibility.Api.Domain.Rules;

public interface IEligibilityRule
{
    RuleResult Evaluate(LoanApplicationData data);
}
