using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LoanApplication.Api.Application.DTOs;

namespace LoanApplication.Api.Application.Services;

public interface ILoanApplicationService
{
    Task<LoanApplicationResponse> SubmitApplicationAsync(LoanApplicationRequest request, CancellationToken cancellationToken = default);
    Task<LoanApplicationResponse?> GetApplicationByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<LoanApplicationResponse>> GetApplicationsByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default);
    Task<IEnumerable<LoanApplicationResponse>> GetAllApplicationsAsync(CancellationToken cancellationToken = default);
}
