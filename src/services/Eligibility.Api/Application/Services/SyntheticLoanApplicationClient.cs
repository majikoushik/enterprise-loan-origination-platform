using Eligibility.Api.Domain.Rules;

namespace Eligibility.Api.Application.Services;

public sealed class SyntheticLoanApplicationClient : ILoanApplicationClient
{
    private static readonly IReadOnlyDictionary<Guid, LoanApplicationData> Applications = new Dictionary<Guid, LoanApplicationData>
    {
        [Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1")] = new(
            Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1"),
            Guid.Parse("11111111-1111-1111-1111-111111111111"),
            185000m,
            48,
            9250m,
            1850m),
        [Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2")] = new(
            Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2"),
            Guid.Parse("22222222-2222-2222-2222-222222222222"),
            420000m,
            84,
            6400m,
            3200m),
        [Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3")] = new(
            Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"),
            Guid.Parse("33333333-3333-3333-3333-333333333333"),
            275000m,
            60,
            7800m,
            900m)
    };

    public Task<LoanApplicationData?> GetApplicationDataAsync(Guid applicationId, CancellationToken cancellationToken = default)
    {
        Applications.TryGetValue(applicationId, out var data);
        return Task.FromResult(data);
    }
}
