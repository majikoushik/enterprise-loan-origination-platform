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

## Loan Application API

| Endpoint | Method | Request Body | Response Body | Status Codes |
| --- | --- | --- | --- | --- |
| `/api/v1/loan-applications` | `POST` | `LoanApplicationRequest` | `LoanApplicationResponse` | `201 Created`, `400 Bad Request` |
| `/api/v1/loan-applications/{id}` | `GET` | None | `LoanApplicationResponse` | `200 OK`, `404 Not Found` |
| `/api/v1/loan-applications/customer/{customerId}` | `GET` | None | `IEnumerable<LoanApplicationResponse>` | `200 OK` |
| `/api/v1/loan-applications` | `GET` | None | `IEnumerable<LoanApplicationResponse>` | `200 OK` |
| `/api/v1/loan-applications/{id}/status` | `GET` | None | `{ status: string }` | `200 OK`, `404 Not Found` |
| `/api/v1/loan-applications/{id}/status-history` | `GET` | None | `IEnumerable<ApplicationStatusHistoryResponse>` | `200 OK`, `404 Not Found` |
| `/api/v1/loan-applications/{id}/status` | `PATCH` | `UpdateApplicationStatusRequest` | `LoanApplicationResponse` | `200 OK`, `400 Bad Request`, `404 Not Found` |

## Eligibility API

| Endpoint | Method | Request Body | Response Body | Status Codes |
| --- | --- | --- | --- | --- |
| `/api/v1/eligibility/check` | `POST` | `EvaluateEligibilityRequest` | `EligibilityResultResponse` | `201 Created`, `400 Bad Request` |
| `/api/v1/eligibility/applications/{applicationId}` | `GET` | None | `EligibilityResultResponse` | `200 OK`, `404 Not Found` |
| `/api/v1/eligibility/results/{id}` | `GET` | None | `EligibilityResultResponse` | `200 OK`, `404 Not Found` |

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
