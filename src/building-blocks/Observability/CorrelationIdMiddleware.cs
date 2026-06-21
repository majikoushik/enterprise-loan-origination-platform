using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Observability;

public sealed class CorrelationIdMiddleware(RequestDelegate next, ILogger<CorrelationIdMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var correlationId = context.Request.Headers.TryGetValue(CorrelationIdOptions.HeaderName, out var value) &&
            !string.IsNullOrWhiteSpace(value)
                ? value.ToString()
                : Guid.NewGuid().ToString("N");

        context.Items[CorrelationIdOptions.HeaderName] = correlationId;
        context.Response.Headers[CorrelationIdOptions.HeaderName] = correlationId;

        using (logger.BeginScope(new Dictionary<string, object> { ["CorrelationId"] = correlationId }))
        {
            await next(context);
        }
    }
}
