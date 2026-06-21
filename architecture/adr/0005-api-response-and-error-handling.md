# ADR-0005: API Response and Error Handling

## Status

Accepted

## Context

The platform exposes multiple APIs that must feel consistent to Angular clients, recruiters, and future integration consumers. Error responses also need to be supportable without leaking stack traces or implementation details.

## Decision

Adopt a standard success envelope containing `data`, `correlationId`, and `timestamp` as the target API governance pattern. In the MVP, metadata and audit query endpoints use the envelope while customer, loan application, eligibility, and notification business endpoints return typed DTOs directly for frontend simplicity. Use Problem Details-compatible error responses for validation, domain, not-found, conflict, and unexpected failures. Include correlation IDs in response headers and logs.

## Consequences

Clients can implement consistent response handling where the envelope is used, and support teams can trace failures by correlation ID. The mixed MVP shape should be resolved before publishing a stable external API contract.

## Alternatives Considered

- Raw DTO responses: simpler, but weaker for consistent correlation and metadata.
- Service-specific error shapes: flexible, but harder for frontend and operations teams.
- Full API gateway transformation: useful later, but unnecessary for the MVP.
