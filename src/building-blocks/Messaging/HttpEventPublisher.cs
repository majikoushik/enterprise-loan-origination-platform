using System.Net.Http.Json;

namespace Messaging;

public class HttpEventPublisher : IMessagePublisher
{
    private readonly HttpClient _httpClient;
    private readonly string _webhookUrl;

    public HttpEventPublisher(HttpClient httpClient, string webhookUrl)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _webhookUrl = webhookUrl ?? throw new ArgumentNullException(nameof(webhookUrl));
    }

    public async Task PublishAsync(IntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
    {
        // Serialize the event to JSON and post it to the internal webhook
        var response = await _httpClient.PostAsJsonAsync(_webhookUrl, integrationEvent, cancellationToken);
        response.EnsureSuccessStatusCode();
    }
}
