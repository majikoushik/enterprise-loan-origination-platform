# ADR-0011: Transactional Outbox for Event Reliability

## Status

Accepted

## Context

Loan application status changes publish notification events after the status update is saved. Direct HTTP event publishing can fail after the database transaction succeeds, which creates a reliability gap.

## Decision

For the MVP, failed event publication is logged as a structured warning and does not roll back the business transaction. The production architecture will use a transactional outbox table in the owning service database, with a background dispatcher that publishes events to Azure Service Bus and records retry/dead-letter outcomes.

## Consequences

The MVP remains simple and locally runnable while documenting the enterprise reliability path. The current implementation is observable but not fully reliable under downstream outage. The outbox pattern is required before representing event delivery as production-grade.

## Alternatives Considered

- Publish directly inside the transaction: rejected because external calls cannot participate safely in the EF transaction.
- Roll back status updates when notification publishing fails: rejected because notification failure should not invalidate the approved business state change.
- Azure Service Bus only: useful transport choice, but it still requires an outbox or equivalent local durability pattern for atomic state-plus-event persistence.
