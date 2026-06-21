export interface AuditEvent {
  id: string;
  eventId: string;
  correlationId: string;
  eventType: string;
  category: string;
  entityType: string;
  entityId: string;
  customerId?: string;
  actorType: string;
  actorId: string;
  action: string;
  summary: string;
  metadataJson: string;
  occurredAtUtc: string;
  sourceService: string;
  severity: string;
  recordedAtUtc: string;
}

export interface AuditApiResponse {
  data: AuditEvent[];
  correlationId: string;
}
