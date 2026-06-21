namespace Messaging;

public abstract record IntegrationEvent(
    Guid EventId,
    string EventType,
    DateTimeOffset OccurredAtUtc,
    string CorrelationId,
    Guid EntityId);
