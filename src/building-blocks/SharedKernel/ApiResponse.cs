namespace SharedKernel;

public sealed record ApiResponse<T>(T Data, string CorrelationId, DateTimeOffset Timestamp)
{
    public static ApiResponse<T> Create(T data, string correlationId) =>
        new(data, correlationId, DateTimeOffset.UtcNow);
}
