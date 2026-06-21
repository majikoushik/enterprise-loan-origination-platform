using System;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Auditing;

public static class AuditingExtensions
{
    public static IServiceCollection AddHttpAuditLogging(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient(nameof(HttpAuditLogger));
        services.AddScoped<IAuditLogger>(sp =>
        {
            var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
            var httpClient = httpClientFactory.CreateClient(nameof(HttpAuditLogger));
            var auditUrl = configuration["ServiceUrls:AuditApi"] ?? "http://localhost:5005/api/v1/audit/events";
            var logger = sp.GetRequiredService<Microsoft.Extensions.Logging.ILogger<HttpAuditLogger>>();
            return new HttpAuditLogger(httpClient, auditUrl, logger);
        });

        return services;
    }
}
