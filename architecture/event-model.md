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

## Implemented Events (Epic 5 MVP)

### `LoanApplicationStatusChangedEvent`
- **Publisher:** `LoanApplication.Api`
- **Subscriber:** `Notification.Worker`
- **Payload:** `PreviousStatus`, `NewStatus`, `CustomerId`
- **Purpose:** Triggers a notification request indicating to the customer that their application status has changed.

## Local MVP Simulation (Epic 5)

For local development and MVP portfolio purposes, we simulate the Azure Service Bus using an `HttpEventPublisher`.
- `IMessagePublisher.PublishAsync()` serializes the event and sends an HTTP POST to `http://localhost:5004/api/v1/internal/events`.
- `Notification.Worker` receives the HTTP POST, stores a `Pending` `NotificationRequest`, and processes it in a background loop.
- This preserves the exact asynchronous programming model required for Azure, avoiding tightly coupled synchronous SDK calls between business services and the notification layer.

## Identified Events

- `CustomerRegistered` (Epic 1 concept)
- `LoanApplicationSubmitted` (Epic 2 concept)
- `EligibilityCheckCompleted` (Epic 3 concept - emitted when eligibility evaluation completes)

## Future Candidate Events

- `LoanApplicationStatusChanged`
- `NotificationRequested`
- `AuditEventRecorded`

## Messaging Direction

MVP messaging may be simulated in-memory. The event contracts should remain compatible with Azure Service Bus messages for future deployment.

## Idempotency

Consumers should be designed to tolerate retries and duplicate messages once asynchronous processing is introduced.
