# Event Model

Events represent business facts that already happened.

## Event Envelope

Each integration event should include:

- Event ID
- Event type
- Occurred timestamp
- Correlation ID
- Entity ID
- Minimal business payload

## Identified Events

- `CustomerRegistered` (Epic 1 concept)
- `LoanApplicationSubmitted` (Epic 2 concept)

## Future Candidate Events

- `EligibilityCheckCompleted`
- `LoanApplicationStatusChanged`
- `NotificationRequested`
- `AuditEventRecorded`

## Messaging Direction

MVP messaging may be simulated in-memory. The event contracts should remain compatible with Azure Service Bus messages for future deployment.

## Idempotency

Consumers should be designed to tolerate retries and duplicate messages once asynchronous processing is introduced.
