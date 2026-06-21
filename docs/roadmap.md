# Roadmap

## Completed

- Epic 0: Repository Foundation
  - Backend solution skeleton
  - Angular portal skeleton
  - Architecture documentation
  - ADRs
  - Docker Compose foundation
  - GitHub Actions CI foundation

## In Progress

- None

## Next

- Epic 1: Customer Registration
  - Customer entity and API
  - Request validation
  - EF Core persistence
  - Customer registration Angular form
  - Tests and documentation updates

## Future Enhancements

- Epic 2: Loan Application Submission
- Epic 3: Eligibility Evaluation
- Epic 4: Status Tracking
- Epic 5: Notification Simulation
- **Epic 6**: Centralized Audit Logging (Event traceability, `Audit.Api`).
- **Epic 7**: Observability & Production Readiness (Serilog, Correlation IDs, Problem Details, Health Checks).
- **Epic 8**: DevOps and Docker (Local container orchestration, GitHub CI quality gates).
- **Epic 9**: Azure Deployment Blueprint (Bicep IaC, GitHub Actions templates).

## Security Roadmap

- Simulated authentication notes during MVP
- Azure Entra ID or Entra External ID integration
- Role-based authorization
- Secure headers and CORS hardening

## Observability Roadmap

- Structured logging provider
- OpenTelemetry/Application Insights integration
- Dependency health checks
- Operational dashboards and support queries

## Azure Deployment Roadmap

- Bicep resource modules
- Container image build and push
- Azure Container Apps deployment
- Static frontend deployment
- Environment-specific configuration and Key Vault references
