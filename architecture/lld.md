# Low-Level Design

## Backend Structure

Each backend service follows the same practical Clean Architecture shape:

- `Domain`: entities, enums, domain exceptions, status transition rules, eligibility rule objects.
- `Application`: DTOs, validators, service interfaces, use-case orchestration.
- `Infrastructure`: EF Core `DbContext`, entity configuration, HTTP integration adapters.
- `Controllers`: thin HTTP adapters that delegate to application services.

Shared concerns live under `src/building-blocks`:

- `SharedKernel`: response envelopes and common entity primitives.
- `Messaging`: integration event contracts and publishing abstraction.
- `Auditing`: HTTP audit publisher and audit event record contract.
- `Observability`: correlation ID middleware, global exception handling, health registration helpers.
- `Security`: future role and policy naming defaults.

## API Flow

1. Client sends a request, optionally including `X-Correlation-ID`.
2. Correlation middleware creates or propagates the correlation ID.
3. Controller delegates to an application service.
4. Request validators enforce required fields, numeric boundaries, and business constraints.
5. Domain models enforce lifecycle rules such as valid loan application status transitions.
6. EF Core persists service-owned state.
7. Application service emits notification or audit records where relevant.
8. Controller returns the documented success DTO or envelope, or a Problem Details error.

## Database Pattern

The MVP uses SQL Server locally and aligns with Azure SQL for cloud hosting. Each service owns its logical database:

- `EnterpriseLoan_Customer`
- `EnterpriseLoan_LoanApplication`
- `EnterpriseLoan_Eligibility`
- `EnterpriseLoan_Notification`
- `EnterpriseLoan_Audit`

For local developer convenience, services can create schema at startup. Production deployment should use controlled EF Core migrations executed through a secure deployment process.

## Frontend Structure

The Angular portal uses standalone components and feature-based folders:

- `core`: typed API models, API services, interceptors.
- `features`: dashboard, customers, loan applications, eligibility, status, notifications, audit trail, system health.
- `layout`: application shell and navigation.
- `environments`: API base URL configuration.

Components use reactive forms where user input is involved. Components call typed services rather than `HttpClient` directly.

## Validation Flow

Validation is performed at API boundaries and reinforced in domain/application logic where business rules matter. Examples include customer contact validation, requested amount and tenure ranges, debt-to-income eligibility checks, and invalid status transition rejection.

## Error Handling

Global exception handling returns Problem Details-compatible responses and avoids exposing stack traces. Correlation IDs are returned to the client and logged server-side so support users can trace a failed request.

## Event Handling

Events represent business facts that already happened. The MVP uses HTTP-based simulation for notification and audit flows while preserving a contract shape compatible with Azure Service Bus. Production hardening should replace HTTP fan-out with topics/subscriptions, retries, poison message handling, and idempotent consumers.
