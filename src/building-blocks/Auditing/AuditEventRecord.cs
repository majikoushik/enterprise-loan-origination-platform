using System;

namespace Auditing;

public record AuditEventRecord(
    Guid EventId,
    string CorrelationId,
    string EventType,
    string Category,
    string EntityType,
    string EntityId,
    Guid? CustomerId,
    string ActorType,
    string ActorId,
    string Action,
    string Summary,
    string MetadataJson,
    DateTimeOffset OccurredAtUtc,
    string SourceService,
    string Severity
);
