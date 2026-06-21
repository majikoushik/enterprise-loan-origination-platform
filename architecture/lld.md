# Low-Level Design

## Backend Structure

Each API starts as an ASP.NET Core Web API project with:

- Minimal service metadata endpoint
- `/health` endpoint
- Swagger/OpenAPI registration
- Problem Details readiness
- Correlation ID middleware
- Shared response envelope support

In Epic 1 and 2, `Customer.Api` and `LoanApplication.Api` introduced the standard Clean/Onion architecture structure:
- **Domain**: Entities, Enums, Domain Exceptions.
- **Application**: DTOs, Services, FluentValidation rules.
- **Infrastructure**: DbContext and EF Core Configurations.
- **API**: Thin controllers that handle only HTTP concerns and delegate to Application Services.

In Epic 2, `LoanApplication.Api` further solidified this pattern, adding more complex domain rules.
In Epic 3, `Eligibility.Api` introduced a Clean Architecture Rules Engine and synchronous HTTP integration (`ILoanApplicationClient`) for cross-service communication.
In Epic 4, `LoanApplication.Api` implemented a strict State Machine within `LoanApplicationEntity` to enforce valid status transitions and append to a persistent `ApplicationStatusHistory` collection.

## Database Pattern

## Frontend Structure

The Angular portal uses standalone components and feature-based folders:

- `core` for interceptors, services, and models
- `shared` for reusable components and validators
- `features` for dashboard, customers, loan applications, eligibility, status, and audit trail
- `layout` for the application shell

## API Flow

1. Request enters API with optional `X-Correlation-ID`.
2. Correlation middleware creates or propagates the correlation ID.
3. Endpoint or future controller delegates to application service.
4. Validation failures return Problem Details.
5. Successful responses use a `{ data, correlationId, timestamp }` envelope.

## Validation Flow

Epic 0 does not implement business requests. Future epics should use request DTO validation through FluentValidation or an equivalent consistent validation strategy.

## Error Handling

APIs register Problem Details and exception handling middleware. Future epics should add central exception mapping for validation, not found, conflict, and business-rule failures.

## Event Handling

The `Messaging` building block defines integration event shape and publisher abstraction. MVP messaging may be in-memory or simulated while preserving Azure Service Bus compatibility.
