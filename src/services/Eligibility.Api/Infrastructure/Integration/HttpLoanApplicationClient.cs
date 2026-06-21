using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Eligibility.Api.Application.Services;
using Eligibility.Api.Domain.Rules;

namespace Eligibility.Api.Infrastructure.Integration;

public class HttpLoanApplicationClient : ILoanApplicationClient
{
    private readonly HttpClient _httpClient;

    public HttpLoanApplicationClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<LoanApplicationData?> GetApplicationDataAsync(Guid applicationId, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync($"/api/v1/loan-applications/{applicationId}", cancellationToken);
        
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();

        var dto = await response.Content.ReadFromJsonAsync<LoanApplicationResponseDto>(cancellationToken: cancellationToken);
        
        if (dto == null)
            return null;

        return new LoanApplicationData(
            dto.Id,
            dto.CustomerId,
            dto.RequestedAmount,
            dto.RequestedTenureInMonths,
            dto.DeclaredMonthlyIncome,
            dto.ExistingEmiObligations
        );
    }

    // Internal DTO to map the response from LoanApplication.Api
    private record LoanApplicationResponseDto(
        Guid Id,
        Guid CustomerId,
        decimal RequestedAmount,
        int RequestedTenureInMonths,
        decimal DeclaredMonthlyIncome,
        decimal ExistingEmiObligations
    );
}
