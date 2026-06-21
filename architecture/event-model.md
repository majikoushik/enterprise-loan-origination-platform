# Event Model

## Event Design Principle

Events represent business facts that already happened. They should be named in past tense, contain minimal consumer-needed data, include correlation IDs, and avoid sensitive payloads.

## Core Events

| Event | Producer | Consumers |
| --- | --- | --- |
| `CustomerRegistered` | Customer API | Audit API, future notification workflows |
| `LoanApplicationSubmitted` | Loan Application API | Notification Worker, Audit API |
| `LoanApplicationStatusChanged` | Loan Application API | Notification Worker, Audit API |
| `EligibilityCheckCompleted` | Eligibility API | Notification Worker, Audit API |
| `NotificationRequested` | Notification Worker/API | Audit API |
| `AuditEventRecorded` | Audit API | Future reporting/monitoring consumers |

## Standard Event Fields

```json
{
  "eventId": "uuid",
  "eventType": "LoanApplicationSubmitted",
  "occurredAtUtc": "2026-01-01T10:00:00Z",
  "correlationId": "string",
  "entityId": "uuid",
  "sourceService": "LoanApplication.Api",
  "data": {}
}
```

## Audit Event Record

The audit contract is more operationally explicit:

```json
{
  "eventId": "uuid",
  "correlationId": "string",
  "eventType": "string",
  "category": "string",
  "entityType": "string",
  "entityId": "string",
  "customerId": "uuid",
  "actorType": "System",
  "actorId": "string",
  "action": "Submit",
  "summary": "Loan application submitted",
  "metadataJson": "{}",
  "occurredAtUtc": "datetime",
  "sourceService": "LoanApplication.Api",
  "severity": "Info"
}
```

## MVP Implementation

The MVP uses HTTP-based event simulation for notification and audit requests. This keeps the platform locally runnable while preserving a migration path to Azure Service Bus.

## Azure Service Bus Direction

Production eventing should use Service Bus topics/subscriptions with:

- Idempotent consumers.
- Dead-letter handling.
- Retry policies.
- Event schema versioning.
- Correlation ID propagation.
- Operational monitoring for queue depth and failure rates.
