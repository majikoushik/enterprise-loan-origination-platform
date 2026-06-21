# Event Model

## Overview
The platform uses event-driven communication to decouple services. Events are grouped into categories for routing.

### Core Events
These are standard integration events published via Service Bus (simulated for MVP).

- `CustomerRegistered` (Customer.Api)
- `LoanApplicationSubmitted` (LoanApplication.Api)
- `LoanApplicationStatusChanged` (LoanApplication.Api)
- `EligibilityCheckCompleted` (Eligibility.Api)
- `NotificationRequested` (Notification.Worker)

### Audit Events
A standardized subset of business facts that are logged to the `Audit.Api` service via the `Auditing` building block. 

Payload structure (`AuditEventRecord`):
```json
{
  "eventId": "uuid",
  "correlationId": "string",
  "eventType": "string",
  "category": "string",
  "entityType": "string",
  "entityId": "string",
  "customerId": "uuid (optional)",
  "actorType": "string",
  "actorId": "string",
  "action": "string",
  "summary": "string",
  "metadataJson": "{}",
  "occurredAtUtc": "datetime",
  "sourceService": "string",
  "severity": "string"
}
```
