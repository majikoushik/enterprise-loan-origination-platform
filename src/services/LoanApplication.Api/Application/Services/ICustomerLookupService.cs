using System;
using System.Threading;
using System.Threading.Tasks;

namespace LoanApplication.Api.Application.Services;

public interface ICustomerLookupService
{
    Task<bool> CustomerExistsAsync(Guid customerId, CancellationToken cancellationToken = default);
}
