using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Auditing;

public static class AuditingExtensions
{
    public static IServiceCollection AddHttpAuditLogging(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient<IAuditLogger, HttpAuditLogger>((sp, client) =>
        {
            // By default try to find a URL, otherwise default to localhost 5005
            var auditUrl = configuration["ServiceUrls:AuditApi"] ?? "http://localhost:5005/api/v1/audit/events";
            var logger = sp.GetRequiredService<Microsoft.Extensions.Logging.ILogger<HttpAuditLogger>>();
            return new HttpAuditLogger(client, auditUrl, logger);
        });

        return services;
    }
}
