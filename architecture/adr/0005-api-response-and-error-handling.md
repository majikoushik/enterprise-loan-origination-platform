# ADR-0005: API Response and Error Handling

## Status

Accepted

## Context

The platform exposes multiple APIs that must feel consistent to Angular clients, recruiters, and future integration consumers. Error responses also need to be supportable without leaking stack traces or implementation details.

## Decision

Use a standard success envelope containing `data`, `correlationId`, and `timestamp`. Use Problem Details-compatible error responses for validation, domain, not-found, conflict, and unexpected failures. Include correlation IDs in responses and logs.

## Consequences

Clients can implement consistent response handling and support teams can trace failures by correlation ID. The pattern adds a small amount of envelope ceremony, but it makes API behavior easier to govern across services.

## Alternatives Considered

- Raw DTO responses: simpler, but weaker for consistent correlation and metadata.
- Service-specific error shapes: flexible, but harder for frontend and operations teams.
- Full API gateway transformation: useful later, but unnecessary for the MVP.
