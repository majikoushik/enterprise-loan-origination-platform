using System;

namespace Audit.Api.Domain.Models;

public class AuditEvent
{
    public Guid Id { get; private set; }
    public Guid EventId { get; private set; }
    public string CorrelationId { get; private set; } = string.Empty;
    public string EventType { get; private set; } = string.Empty;
    public string Category { get; private set; } = string.Empty;
    public string EntityType { get; private set; } = string.Empty;
    public string EntityId { get; private set; } = string.Empty;
    public Guid? CustomerId { get; private set; }
    public string ActorType { get; private set; } = string.Empty;
    public string ActorId { get; private set; } = string.Empty;
    public string Action { get; private set; } = string.Empty;
    public string Summary { get; private set; } = string.Empty;
    public string MetadataJson { get; private set; } = string.Empty;
    public DateTimeOffset OccurredAtUtc { get; private set; }
    public string SourceService { get; private set; } = string.Empty;
    public string Severity { get; private set; } = string.Empty;
    public DateTimeOffset RecordedAtUtc { get; private set; }

    protected AuditEvent() { }

    public AuditEvent(
        Guid eventId,
        string correlationId,
        string eventType,
        string category,
        string entityType,
        string entityId,
        Guid? customerId,
        string actorType,
        string actorId,
        string action,
        string summary,
        string metadataJson,
        DateTimeOffset occurredAtUtc,
        string sourceService,
        string severity)
    {
        Id = Guid.NewGuid();
        EventId = eventId;
        CorrelationId = correlationId;
        EventType = eventType;
        Category = category;
        EntityType = entityType;
        EntityId = entityId;
        CustomerId = customerId;
        ActorType = actorType;
        ActorId = actorId;
        Action = action;
        Summary = summary;
        MetadataJson = metadataJson;
        OccurredAtUtc = occurredAtUtc;
        SourceService = sourceService;
        Severity = severity;
        RecordedAtUtc = DateTimeOffset.UtcNow;
    }
}
