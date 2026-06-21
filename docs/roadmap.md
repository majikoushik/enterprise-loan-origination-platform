# Roadmap

## Completed MVP and Portfolio Epics

- Epic 0: Repository Foundation.
- Epic 1: Customer Registration.
- Epic 2: Loan Application Submission.
- Epic 3: Eligibility Evaluation.
- Epic 4: Status Tracking.
- Epic 5: Notification Simulation.
- Epic 6: Audit Logging.
- Epic 7: Observability and Production Readiness.
- Epic 8: DevOps and Docker.
- Epic 9: Azure Deployment Blueprint.
- Epic 10: Portfolio Polish.

## Current Portfolio Baseline

The repository now demonstrates a full end-to-end architecture story:

- Domain-aligned APIs and Angular portal.
- Testable application/domain logic.
- Local Docker orchestration.
- CI validation.
- Observability and security foundations.
- Azure Bicep deployment blueprint.
- Architecture diagrams, ADRs, and operational documentation.

## Recommended Next Enhancements

- Capture real screenshots and add them under `docs/screenshots`.
- Add browser-level smoke tests with Playwright.
- Introduce Azure Entra ID or Entra External ID authentication.
- Replace HTTP event simulation with Azure Service Bus topics/subscriptions.
- Add EF Core migrations and controlled migration deployment.
- Add OpenTelemetry traces and Application Insights dashboards.
- Add API Management policies for rate limiting, JWT validation, and correlation propagation.
- Add demo script and sample request collection.

## Security Roadmap

- Add real authentication and authorization.
- Introduce role-based access for Customer, Loan Officer, and Admin personas.
- Harden CORS and secure headers for deployed environments.
- Add dependency and container vulnerability scanning.
- Add Key Vault secret rotation notes.

## Observability Roadmap

- Add OpenTelemetry distributed tracing.
- Add dashboards for application volume, eligibility decisions, notification failures, latency, and error rate.
- Add alerts for unhealthy APIs, SQL connectivity, high failure rate, and Service Bus dead-letter count.

## Azure Deployment Roadmap

- Validate Bicep in CI.
- Build and push images to Azure Container Registry.
- Deploy Container Apps revisions.
- Deploy Angular assets to Static Web Apps.
- Add Service Bus-based event consumers.
- Document rollback, backup, and cleanup procedures.
