using System;
using System.Threading;
using System.Threading.Tasks;
using Eligibility.Api.Application.DTOs;

namespace Eligibility.Api.Application.Services;

public interface IEligibilityService
{
    Task<EligibilityResultResponse> EvaluateAsync(EvaluateEligibilityRequest request, CancellationToken cancellationToken = default);
    Task<EligibilityResultResponse?> GetByApplicationIdAsync(Guid applicationId, CancellationToken cancellationToken = default);
    Task<EligibilityResultResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
