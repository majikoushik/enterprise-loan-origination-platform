# API Contracts

## Foundation Endpoints

| Service | Endpoint | Purpose |
| --- | --- | --- |
| Customer API | `GET /api/v1/customer-service/metadata` | Service identity and responsibility |
| Loan Application API | `GET /api/v1/loan-application-service/metadata` | Service identity and responsibility |
| Eligibility API | `GET /api/v1/eligibility-service/metadata` | Service identity and responsibility |
| Audit API | `GET /api/v1/audit-service/metadata` | Service identity and responsibility |
| APIs | `GET /health` | Health check |

## Customer API

| Endpoint | Method | Request Body | Response Body | Status Codes |
| --- | --- | --- | --- | --- |
| `/api/v1/customers` | `POST` | `CustomerRegistrationRequest` | `CustomerResponse` | `201 Created`, `400 Bad Request` |
| `/api/v1/customers/{id}` | `GET` | None | `CustomerResponse` | `200 OK`, `404 Not Found` |
| `/api/v1/customers` | `GET` | None | `IEnumerable<CustomerResponse>` | `200 OK` |


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
