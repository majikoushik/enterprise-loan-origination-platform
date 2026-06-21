using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Eligibility.Api.Application.DTOs;
using Eligibility.Api.Domain.Exceptions;
using Eligibility.Api.Domain.Models;
using Eligibility.Api.Domain.Rules;
using Eligibility.Api.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Eligibility.Api.Application.Services;

public class EligibilityService : IEligibilityService
{
    private readonly EligibilityDbContext _dbContext;
    private readonly ILoanApplicationClient _loanApplicationClient;
    private readonly RuleEngine _ruleEngine;

    public EligibilityService(
        EligibilityDbContext dbContext,
        ILoanApplicationClient loanApplicationClient,
        RuleEngine ruleEngine)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _loanApplicationClient = loanApplicationClient ?? throw new ArgumentNullException(nameof(loanApplicationClient));
        _ruleEngine = ruleEngine ?? throw new ArgumentNullException(nameof(ruleEngine));
    }

    public async Task<EligibilityResultResponse> EvaluateAsync(EvaluateEligibilityRequest request, CancellationToken cancellationToken = default)
    {
        // 1. Fetch Loan Application Data
        var applicationData = await _loanApplicationClient.GetApplicationDataAsync(request.ApplicationId, cancellationToken);
        if (applicationData == null)
        {
            throw new EligibilityDomainException($"Loan application with ID {request.ApplicationId} could not be found.");
        }

        // 2. Evaluate rules
        var result = _ruleEngine.Evaluate(applicationData);

        // 3. Save result
        _dbContext.EligibilityResults.Add(result);
        await _dbContext.SaveChangesAsync(cancellationToken);

        // TODO: In a future epic, publish an integration event (e.g. EligibilityCheckCompleted)
        // so that LoanApplication.Api can update the application status to EligibilityPassed/Failed.

        return MapToResponse(result);
    }

    public async Task<EligibilityResultResponse?> GetByApplicationIdAsync(Guid applicationId, CancellationToken cancellationToken = default)
    {
        var result = await _dbContext.EligibilityResults
            .Include("_ruleResults")
            .AsNoTracking()
            .OrderByDescending(r => r.EvaluatedAt)
            .FirstOrDefaultAsync(r => r.ApplicationId == applicationId, cancellationToken);

        return result == null ? null : MapToResponse(result);
    }

    public async Task<EligibilityResultResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _dbContext.EligibilityResults
            .Include("_ruleResults")
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

        return result == null ? null : MapToResponse(result);
    }

    private static EligibilityResultResponse MapToResponse(EligibilityResult result)
    {
        return new EligibilityResultResponse(
            result.Id,
            result.ApplicationId,
            result.CustomerId,
            result.Decision.ToString(),
            result.RuleVersion,
            result.EvaluatedAt,
            result.RequestedAmount,
            result.DeclaredMonthlyIncome,
            result.ExistingEmiObligations,
            result.DebtToIncomeRatio,
            result.DecisionSummary,
            result.RuleResults.Select(r => new RuleResultResponse(
                r.RuleCode,
                r.RuleName,
                r.Passed,
                r.ActualValue,
                r.ExpectedValue,
                r.Explanation
            ))
        );
    }
}
