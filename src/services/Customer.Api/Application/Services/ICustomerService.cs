using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Customer.Api.Application.DTOs;

namespace Customer.Api.Application.Services;

public interface ICustomerService
{
    Task<CustomerResponse> RegisterCustomerAsync(CustomerRegistrationRequest request, CancellationToken cancellationToken = default);
    Task<CustomerResponse?> GetCustomerByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<CustomerResponse>> GetAllCustomersAsync(CancellationToken cancellationToken = default);
}
