using System;
using System.Threading;
using System.Threading.Tasks;

namespace LoanApplication.Api.Application.Services;

public class StubCustomerLookupService : ICustomerLookupService
{
    public Task<bool> CustomerExistsAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        // For MVP Epic 2, we assume the customer exists if the ID format is valid.
        // In a real implementation, this would make an HTTP call to Customer.Api
        // or check a replicated Read Model / Cache.
        return Task.FromResult(true);
    }
}
