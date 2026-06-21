using System;
using System.Threading;
using System.Threading.Tasks;
using Eligibility.Api.Domain.Rules;

namespace Eligibility.Api.Application.Services;

public interface ILoanApplicationClient
{
    Task<LoanApplicationData?> GetApplicationDataAsync(Guid applicationId, CancellationToken cancellationToken = default);
}
