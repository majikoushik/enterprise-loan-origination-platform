# ADR-0006: Event-Driven Notification and Audit

## Status

Accepted

## Context

Loan application submission, eligibility completion, status changes, notification requests, and audit records are business facts that should be traceable and decoupled over time. The MVP still needs to run locally without requiring Service Bus.

## Decision

Model events explicitly and use HTTP-based event simulation for MVP notification and audit flows. Preserve Azure Service Bus-compatible event names and payload principles for the production path.

## Consequences

The platform demonstrates event-driven thinking while staying easy to run locally. Production hardening will require replacing HTTP fan-out with Service Bus topics/subscriptions, idempotent consumers, retries, and dead-letter handling.

## Alternatives Considered

- Direct synchronous side effects only: easiest, but weak architecture signal and poor decoupling.
- Service Bus from day one: realistic, but heavier local setup for a portfolio MVP.
- In-memory only events: simple, but less useful across separate services.
