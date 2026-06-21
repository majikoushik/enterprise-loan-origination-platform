# API Governance

## Route Conventions

APIs use `/api/v1` route prefixes and domain-oriented resource names. Future examples:

- `POST /api/v1/customers`
- `GET /api/v1/customers/{customerId}`
- `POST /api/v1/loan-applications`
- `GET /api/v1/loan-applications/{applicationId}`
- `POST /api/v1/eligibility/check`
- `GET /api/v1/audit/entity/{entityType}/{entityId}`

## Response Pattern

Successful responses should use:

```json
{
  "data": {},
  "correlationId": "string",
  "timestamp": "2026-01-01T10:00:00Z"
}
```

Errors should follow Problem Details with correlation ID included.

## Validation

All input DTOs must validate required fields, formats, numeric ranges, business boundaries, and invalid references.

## Versioning

Epic 0 uses URL version readiness through `/api/v1`. Formal API versioning libraries may be added when multiple API versions are required.

## OpenAPI

Every API registers Swagger/OpenAPI in development. Future epics should document DTOs, response codes, and validation behavior.

## Correlation

Clients should send `X-Correlation-ID`. APIs generate one when missing and return it in response headers and envelopes.
