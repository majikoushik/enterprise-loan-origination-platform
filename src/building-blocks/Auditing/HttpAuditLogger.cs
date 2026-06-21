using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Auditing;

public class HttpAuditLogger : IAuditLogger
{
    private readonly HttpClient _httpClient;
    private readonly string _auditApiUrl;
    private readonly ILogger<HttpAuditLogger> _logger;

    public HttpAuditLogger(HttpClient httpClient, string auditApiUrl, ILogger<HttpAuditLogger> logger)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _auditApiUrl = auditApiUrl ?? throw new ArgumentNullException(nameof(auditApiUrl));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task LogAsync(AuditEventRecord record, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync(_auditApiUrl, record, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync(cancellationToken);
                _logger.LogWarning("Failed to log audit event {EventId} via HTTP. Status: {StatusCode}. Response: {Content}", record.EventId, response.StatusCode, content);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception occurred while logging audit event {EventId} to {AuditApiUrl}", record.EventId, _auditApiUrl);
            // In MVP we swallow to avoid failing business transactions on audit delivery issues
            // In prod, use Azure Service Bus and Outbox pattern.
        }
    }
}
