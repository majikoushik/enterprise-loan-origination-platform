# API Contracts

Epic 0 exposes metadata endpoints only. Future epics will document full request and response DTOs here.

## Foundation Endpoints

| Service | Endpoint | Purpose |
| --- | --- | --- |
| Customer API | `GET /api/v1/customer-service/metadata` | Service identity and responsibility |
| Loan Application API | `GET /api/v1/loan-application-service/metadata` | Service identity and responsibility |
| Eligibility API | `GET /api/v1/eligibility-service/metadata` | Service identity and responsibility |
| Audit API | `GET /api/v1/audit-service/metadata` | Service identity and responsibility |
| APIs | `GET /health` | Health check |

## Response Envelope

```json
{
  "data": {},
  "correlationId": "string",
  "timestamp": "2026-01-01T10:00:00Z"
}
```

## Error Contract

Errors should use Problem Details with correlation ID support. Validation errors should include field-level details.
