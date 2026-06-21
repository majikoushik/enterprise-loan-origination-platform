namespace Messaging;

public record LoanApplicationStatusChangedEvent(
    Guid EventId,
    DateTimeOffset OccurredAtUtc,
    string CorrelationId,
    Guid EntityId,
    string PreviousStatus,
    string NewStatus,
    Guid CustomerId
) : IntegrationEvent(EventId, "LoanApplicationStatusChanged", OccurredAtUtc, CorrelationId, EntityId);
