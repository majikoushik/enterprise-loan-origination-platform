# API Governance

## Route Conventions

APIs use `/api/v1` route prefixes and domain-oriented resource names:

- `POST /api/v1/customers`
- `GET /api/v1/customers/{id}`
- `POST /api/v1/loan-applications`
- `GET /api/v1/loan-applications/{id}`
- `GET /api/v1/loan-applications/customer/{customerId}`
- `PATCH /api/v1/loan-applications/{id}/status`
- `POST /api/v1/eligibility/check`
- `GET /api/v1/eligibility/applications/{applicationId}`
- `GET /api/v1/audit/entity/{entityType}/{entityId}`

Service metadata endpoints may use service-specific routes for diagnostics and documentation.

## Response Pattern

Successful responses use a consistent envelope:

```json
{
  "data": {},
  "correlationId": "string",
  "timestamp": "2026-01-01T10:00:00Z"
}
```

Errors use Problem Details-compatible responses with correlation ID support. Validation failures should include field-level details when available.

## Validation

All input DTOs validate required fields, formats, numeric ranges, business boundaries, and invalid references. Domain rules are enforced in application/domain layers, not controllers.

## Versioning

The MVP uses URL version readiness through `/api/v1`. A formal API versioning package can be introduced when parallel API versions are required.

## OpenAPI

Every API registers Swagger/OpenAPI in development. Controllers should document response codes, DTO contracts, validation behavior, and business error outcomes.

## Correlation

Clients should send `X-Correlation-ID`. APIs generate one when missing, include it in response headers, and propagate it into logs, response envelopes, audit records, and downstream calls.

## Compatibility Rules

- Prefer additive changes to response DTOs.
- Avoid route changes after a feature is published unless an ADR documents the breaking change.
- Do not expose internal exception messages, connection strings, stack traces, or implementation details.
