using System;

namespace Audit.Api.Domain.Models;

public class AuditEvent
{
    public Guid Id { get; private set; }
    public Guid EventId { get; private set; }
    public string CorrelationId { get; private set; }
    public string EventType { get; private set; }
    public string Category { get; private set; }
    public string EntityType { get; private set; }
    public string EntityId { get; private set; }
    public Guid? CustomerId { get; private set; }
    public string ActorType { get; private set; }
    public string ActorId { get; private set; }
    public string Action { get; private set; }
    public string Summary { get; private set; }
    public string MetadataJson { get; private set; }
    public DateTimeOffset OccurredAtUtc { get; private set; }
    public string SourceService { get; private set; }
    public string Severity { get; private set; }
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
