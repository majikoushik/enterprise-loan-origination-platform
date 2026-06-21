using Microsoft.AspNetCore.Http;

namespace Observability;

public class CorrelationIdProvider(IHttpContextAccessor httpContextAccessor)
{
    public virtual string Get()
    {
        var context = httpContextAccessor.HttpContext;

        if (context?.Items.TryGetValue(CorrelationIdOptions.HeaderName, out var itemValue) == true &&
            itemValue is not null &&
            !string.IsNullOrWhiteSpace(itemValue.ToString()))
        {
            return itemValue.ToString()!;
        }

        if (context?.Request.Headers.TryGetValue(CorrelationIdOptions.HeaderName, out var headerValue) == true &&
            !string.IsNullOrWhiteSpace(headerValue))
        {
            return headerValue.ToString();
        }

        return Guid.NewGuid().ToString("N");
    }
}
