# ADR-0010: Inter-Service Customer Lookup Strategy

## Status

Accepted

## Context

Loan application submission must validate that the referenced customer exists. Earlier MVP code used a stubbed customer lookup that returned `true` for every GUID, which was useful for isolated unit tests but could hide invalid-customer defects in runtime flows.

## Decision

`LoanApplication.Api` uses an HTTP-backed `ICustomerLookupService` implementation at runtime. It calls `Customer.Api` by service URL configuration and treats HTTP 404 as a missing customer. The stub implementation remains only for unit-test substitution and is marked obsolete to prevent accidental runtime use.

## Consequences

The loan application boundary now enforces the customer-existence business rule without sharing the customer database. The tradeoff is a synchronous service dependency during submission; this is acceptable for the MVP because the operation is user-facing and simple to reason about.

## Alternatives Considered

- Always-true stub: rejected for runtime because it hides business-rule defects.
- Shared database lookup: rejected because it violates service-owned data boundaries.
- Event-driven customer read model: preferred for higher scale and resilience, but deferred until the platform introduces durable messaging and read-model maintenance.
