using Microsoft.AspNetCore.Builder;

namespace Observability;

public static class ObservabilityExtensions
{
    public static IApplicationBuilder UseCorrelationId(this IApplicationBuilder app) =>
        app.UseMiddleware<CorrelationIdMiddleware>();
}
