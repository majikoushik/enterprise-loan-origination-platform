# API Contracts

## Standards

- Version prefix: `/api/v1`.
- Success response: business endpoints currently return typed DTOs directly; metadata and audit query endpoints use `{ data, correlationId, timestamp }`.
- Error response: Problem Details-compatible shape with correlation ID.
- Correlation header: `X-Correlation-ID`.
- Swagger/OpenAPI: enabled in development.

## Foundation Endpoints

| Service | Endpoint | Purpose |
| --- | --- | --- |
| Customer API | `GET /api/v1/customer-service/metadata` | Service identity and responsibility |
| Loan Application API | `GET /api/v1/loan-application-service/metadata` | Service identity and responsibility |
| Eligibility API | `GET /api/v1/eligibility-service/metadata` | Service identity and responsibility |
| Audit API | `GET /api/v1/audit-service/metadata` | Service identity and responsibility |
| APIs | `GET /health`, `/health/live`, `/health/ready` | Health checks |

## Customer API

| Method | Endpoint | Request | Response | Status Codes |
| --- | --- | --- | --- | --- |
| `POST` | `/api/v1/customers` | `CustomerRegistrationRequest` | `CustomerResponse` | `201`, `400` |
| `GET` | `/api/v1/customers/{id}` | None | `CustomerResponse` | `200`, `404` |
| `GET` | `/api/v1/customers` | None | `CustomerResponse[]` | `200` |

## Loan Application API

| Method | Endpoint | Request | Response | Status Codes |
| --- | --- | --- | --- | --- |
| `POST` | `/api/v1/loan-applications` | `LoanApplicationRequest` | `LoanApplicationResponse` | `201`, `400` |
| `GET` | `/api/v1/loan-applications/{id}` | None | `LoanApplicationResponse` | `200`, `404` |
| `GET` | `/api/v1/loan-applications/customer/{customerId}` | None | `LoanApplicationResponse[]` | `200` |
| `GET` | `/api/v1/loan-applications` | None | `LoanApplicationResponse[]` | `200` |
| `GET` | `/api/v1/loan-applications/{id}/status` | None | Status payload | `200`, `404` |
| `GET` | `/api/v1/loan-applications/{id}/status-history` | None | `ApplicationStatusHistoryResponse[]` | `200`, `404` |
| `PATCH` | `/api/v1/loan-applications/{id}/status` | `UpdateApplicationStatusRequest` | `LoanApplicationResponse` | `200`, `400`, `404` |

## Eligibility API

| Method | Endpoint | Request | Response | Status Codes |
| --- | --- | --- | --- | --- |
| `POST` | `/api/v1/eligibility/check` | `EvaluateEligibilityRequest` | `EligibilityResultResponse` | `201`, `400` |
| `GET` | `/api/v1/eligibility/applications/{applicationId}` | None | `EligibilityResultResponse` | `200`, `404` |
| `GET` | `/api/v1/eligibility/results/{id}` | None | `EligibilityResultResponse` | `200`, `404` |

## Notification Worker/API

| Method | Endpoint | Request | Response | Status Codes |
| --- | --- | --- | --- | --- |
| `POST` | `/api/v1/internal/events` | Integration event payload | Empty success response | `200`, `400` |
| `GET` | `/api/v1/notifications` | None | Notification request list | `200` |

## Audit API

| Method | Endpoint | Request | Response | Status Codes |
| --- | --- | --- | --- | --- |
| `POST` | `/api/v1/audit/events` | `AuditEventRecord` | Empty success response | `200`, `400` |
| `GET` | `/api/v1/audit/events` | None | Audit event list envelope | `200` |
| `GET` | `/api/v1/audit/entity/{entityType}/{entityId}` | None | Audit event list | `200` |
| `GET` | `/api/v1/audit/customer/{customerId}` | None | Audit event list envelope | `200` |
| `GET` | `/api/v1/audit/correlation/{correlationId}` | None | Audit event list envelope | `200` |

## Error Contract

```json
{
  "type": "https://example.com/problems/validation-error",
  "title": "Validation failed",
  "status": 400,
  "detail": "One or more validation errors occurred.",
  "correlationId": "string",
  "errors": {}
}
```
