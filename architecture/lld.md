# Low-Level Design

## Backend Structure

Each API starts as an ASP.NET Core Web API project with:

- Minimal service metadata endpoint
- `/health` endpoint
- Swagger/OpenAPI registration
- Problem Details readiness
- Correlation ID middleware
- Shared response envelope support

In Epic 1, `Customer.Api` introduced the standard Clean/Onion architecture structure:
- **Domain**: Entities, Enums, Domain Exceptions.
- **Application**: DTOs, FluentValidation Validators, Application Services.
- **Infrastructure**: EF Core DbContext, Configurations.
- **Controllers**: API Endpoints.

Future epics should follow this structure.

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
