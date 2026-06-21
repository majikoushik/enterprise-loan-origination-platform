using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using LoanApplication.Api.Application.Services;
using Microsoft.Extensions.Logging;

namespace LoanApplication.Api.Infrastructure.Integration;

public sealed class HttpCustomerLookupService : ICustomerLookupService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<HttpCustomerLookupService> _logger;

    public HttpCustomerLookupService(HttpClient httpClient, ILogger<HttpCustomerLookupService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<bool> CustomerExistsAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        using var response = await _httpClient.GetAsync($"/api/v1/customers/{customerId}", cancellationToken);

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return false;
        }

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning(
                "Customer lookup for {CustomerId} returned {StatusCode}",
                customerId,
                response.StatusCode);
        }

        response.EnsureSuccessStatusCode();
        return true;
    }
}
