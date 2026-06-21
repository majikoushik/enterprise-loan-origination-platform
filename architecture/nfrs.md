# Non-Functional Requirements

## Scalability

- Backend services are stateless and containerized for horizontal scaling on Azure Container Apps.
- Service-owned SQL databases align with Azure SQL scaling options.
- Notification and audit workflows are designed to move from MVP HTTP simulation to Azure Service Bus to absorb bursts.

## Availability and Reliability

- APIs expose health endpoints for process and readiness monitoring.
- Global exception handling creates predictable failures rather than leaking runtime details.
- Future production integrations should add retries, circuit breakers, idempotency, dead-letter queues, and replay procedures.

## Performance

- MVP APIs use simple request/response flows and EF Core persistence.
- Read models are intentionally modest. Future scaling can add pagination, indexing, caching, and async event processing.

## Security

- No real customer data, secrets, or production financial records are included.
- Configuration is environment-driven and ready for Key Vault references.
- Future authentication direction is Azure Entra ID or Entra External ID with role-based authorization.
- Logs and audit metadata must avoid secrets and sensitive personal/financial data.

## Observability

- Correlation IDs are propagated across frontend and backend requests.
- Health checks support local diagnostics and Azure Container Apps probes.
- Application Insights and Log Analytics are the target telemetry stores.
- The operational runbook defines basic triage and KQL query direction.

## Maintainability

- Domain language is used consistently: Customer, Loan Application, Eligibility Check, Notification, Audit Event.
- Service boundaries are explicit and documented.
- Shared building blocks remain small and focused to avoid premature platform complexity.

## Cost Awareness

- Local execution uses Docker Compose and SQL Server Developer Edition.
- Azure blueprint uses cost-conscious service selections for demo environments.
- Demo resource groups should be deleted after review to avoid ongoing charges.

## Compliance Readiness

The project is not compliance-certified. It demonstrates controls that would support future compliance work: auditability, traceability, secure configuration, least-privilege direction, sanitized errors, and documentation of architectural decisions.
