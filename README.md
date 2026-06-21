# Enterprise Loan Origination Platform

[![CI](https://github.com/koushikchandramaji/enterprise-loan-origination-platform/actions/workflows/ci.yml/badge.svg)](https://github.com/koushikchandramaji/enterprise-loan-origination-platform/actions/workflows/ci.yml)
![.NET](https://img.shields.io/badge/.NET-8.0-512BD4)
![Angular](https://img.shields.io/badge/Angular-18-DD0031)
![Azure](https://img.shields.io/badge/Azure-Container%20Apps%20%7C%20SQL%20%7C%20Service%20Bus-0078D4)
![Architecture](https://img.shields.io/badge/Architecture-Clean%20%7C%20API--First%20%7C%20Event--Ready-17324D)

## Executive Summary

Enterprise Loan Origination Platform is a portfolio-grade banking application that demonstrates how a modern loan journey can be designed, implemented, tested, containerized, documented, and prepared for Azure deployment.

The solution models a realistic but intentionally simplified origination workflow: customer registration, loan application submission, rule-based eligibility evaluation, status tracking, notification simulation, and centralized audit logging. It is not a real credit decisioning engine; it is an architecture showcase for .NET, Angular, microservices-ready service boundaries, observability, security, DevOps, and Azure cloud-native design.

## Architecture Highlights

- Domain-aligned services for customer, loan application, eligibility, notification, and audit capabilities.
- Thin API controllers with application services, domain models, validation, EF Core infrastructure, and shared building blocks.
- API-first REST contracts using `/api/v1`, response envelopes, Problem Details, Swagger/OpenAPI, and correlation IDs.
- Event-driven design direction using integration event contracts and MVP HTTP-based notification/audit simulation.
- Production-readiness patterns: health checks, structured logging, global exception handling, sanitized errors, and operational runbook.
- Azure deployment blueprint using Bicep for Container Apps, Azure SQL, Service Bus, Key Vault, Application Insights, Log Analytics, Container Registry, and Static Web Apps.

## Business Capabilities

| Capability | Status | Notes |
| --- | --- | --- |
| Customer registration | Implemented | Synthetic profile data, validation, persistence, Angular form |
| Loan application submission | Implemented | Application lifecycle, DTOs, validation, audit integration |
| Eligibility evaluation | Implemented | Demo rules for income, tenure, DTI, obligations, amount limits |
| Status tracking | Implemented | Controlled status transitions and status history |
| Notification simulation | Implemented | Email/SMS request simulation with delivery outcome tracking |
| Audit trail | Implemented | Centralized business event records with correlation ID |
| Observability foundation | Implemented | Correlation middleware, health checks, Problem Details, runbook |
| Docker and CI | Implemented | Service Dockerfiles, Docker Compose, GitHub Actions CI |
| Azure blueprint | Implemented | Bicep modules and deployment guide |

## Technology Stack

| Layer | Technology |
| --- | --- |
| Backend | .NET 8, ASP.NET Core Web API, C# 12 |
| Data | Entity Framework Core, SQL Server locally, Azure SQL target |
| Validation | FluentValidation-style request validators |
| Frontend | Angular 18, standalone components, strict TypeScript, reactive forms |
| Integration | HTTP APIs now, Azure Service Bus direction for production events |
| Observability | Correlation IDs, health checks, Problem Details, Application Insights readiness |
| DevOps | Docker, Docker Compose, GitHub Actions |
| Azure | Container Apps, Static Web Apps, Azure SQL, Service Bus, Key Vault, ACR, App Insights, Log Analytics |

## Repository Structure

```text
src/
  services/
    Customer.Api/
    LoanApplication.Api/
    Eligibility.Api/
    Notification.Worker/
    Audit.Api/
  building-blocks/
    Auditing/
    Messaging/
    Observability/
    Security/
    SharedKernel/
  web/
    loan-portal-angular/
tests/
architecture/
docs/
infra/bicep/
.github/workflows/
```

## System Architecture

- [System context](architecture/diagrams/system-context.md)
- [Container diagram](architecture/diagrams/container-diagram.md)
- [Loan application submission sequence](architecture/diagrams/loan-application-sequence.md)
- [Eligibility evaluation sequence](architecture/diagrams/eligibility-evaluation-sequence.md)
- [Notification and audit event flow](architecture/diagrams/notification-audit-event-flow.md)
- [Azure deployment](architecture/diagrams/azure-deployment.md)
- [CI/CD pipeline](architecture/diagrams/cicd-pipeline.md)

## Application Screens

Screenshots are intentionally documented as a publishing step because local browser rendering depends on the developer environment.

- Dashboard: `docs/screenshots/dashboard.png`
- Customer registration: `docs/screenshots/customer-registration.png`
- Loan application submission: `docs/screenshots/loan-application.png`
- Eligibility result: `docs/screenshots/eligibility-result.png`
- Audit trail: `docs/screenshots/audit-trail.png`

See [screenshot capture notes](docs/screenshots/README.md).

## API Overview

Swagger/OpenAPI is enabled for the APIs in development. Key contracts are summarized in [API contracts](docs/api-contracts.md).

| Service | Local Docker Port | Swagger |
| --- | --- | --- |
| Customer API | `7101` | `http://localhost:7101/swagger` |
| Loan Application API | `7102` | `http://localhost:7102/swagger` |
| Eligibility API | `7103` | `http://localhost:7103/swagger` |
| Notification Worker/API | `5004` | `http://localhost:5004/swagger` |
| Audit API | `5005` | `http://localhost:5005/swagger` |

All APIs use `X-Correlation-ID` propagation. Metadata and audit query endpoints return a standard success envelope:

```json
{
  "data": {},
  "correlationId": "string",
  "timestamp": "2026-01-01T10:00:00Z"
}
```

## Local Development

Prerequisites:

- .NET 8 SDK
- Node.js 22 or another Angular 18-compatible LTS/current Node version
- Docker Desktop
- Git

Restore and build:

```powershell
dotnet restore EnterpriseLoanOriginationPlatform.sln
dotnet build EnterpriseLoanOriginationPlatform.sln
```

Run an API:

```powershell
dotnet run --project src/services/Customer.Api/Customer.Api.csproj
```

Run the Angular portal:

```powershell
cd src/web/loan-portal-angular
npm install
npm start
```

Open `http://localhost:4200`.

## Docker-Based Development

Build and run the local platform:

```powershell
docker compose --profile services --profile frontend build
docker compose --profile services --profile frontend up -d
```

Stop the platform:

```powershell
docker compose --profile services --profile frontend down
```

The compose file contains a local-only SQL Server developer password for demo execution. Do not reuse it outside local development.

## Testing

Backend:

```powershell
dotnet test EnterpriseLoanOriginationPlatform.sln --configuration Release
```

Frontend:

```powershell
cd src/web/loan-portal-angular
npm ci
npm run build
npm run test:ci
```

Docker validation:

```powershell
docker compose --profile services --profile frontend build
```

Bicep validation:

```powershell
az bicep build --file infra/bicep/main.bicep
```

## Observability and Production Readiness

- `X-Correlation-ID` is generated or propagated across frontend and APIs.
- Backend services expose health endpoints for local diagnostics and Azure Container Apps probes.
- Global exception handling returns Problem Details without leaking stack traces.
- Audit events provide business traceability by entity, action, source service, and correlation ID.
- [Operational runbook](docs/operational-runbook.md) describes triage flows and Log Analytics query direction.

## Security and Compliance Readiness

- No real customer data, credentials, or production financial data are included.
- Configuration is environment-based and ready for Key Vault references in Azure.
- Authentication is intentionally deferred for MVP, with Azure Entra ID / Entra External ID and RBAC documented as the target.
- Logs and audit metadata must avoid secrets, passwords, full personal identifiers, and sensitive financial data.
- Security posture is documented in [security architecture](architecture/security-architecture.md).

## Azure Deployment Blueprint

The Azure blueprint is infrastructure-as-code ready but intentionally not auto-deployed from this repository. It includes:

- Azure Container Apps for backend APIs and workers.
- Azure Static Web Apps for the Angular portal.
- Azure SQL Database for persistence.
- Azure Service Bus for future asynchronous messaging.
- Azure Key Vault and Managed Identity for secret access.
- Azure Container Registry for image storage.
- Application Insights and Log Analytics for telemetry.

Start with [Azure deployment guide](docs/azure-deployment-guide.md) and [Bicep README](infra/bicep/README.md).

## Architecture Decision Records

- [ADR-0001: Architecture Style](architecture/adr/0001-architecture-style.md)
- [ADR-0002: Database Choice](architecture/adr/0002-database-choice.md)
- [ADR-0003: Azure Hosting Strategy](architecture/adr/0003-azure-hosting-strategy.md)
- [ADR-0004: Observability Strategy](architecture/adr/0004-observability-strategy.md)
- [ADR-0005: API Response and Error Handling](architecture/adr/0005-api-response-and-error-handling.md)
- [ADR-0006: Event-Driven Notification and Audit](architecture/adr/0006-event-driven-notification-and-audit.md)
- [ADR-0007: Angular Frontend Architecture](architecture/adr/0007-angular-frontend-architecture.md)
- [ADR-0008: Docker and DevOps Strategy](architecture/adr/0008-docker-and-devops-strategy.md)
- [ADR-0009: Azure Infrastructure as Code Strategy](architecture/adr/0009-azure-infrastructure-as-code-strategy.md)

## Roadmap

The MVP portfolio baseline is complete through Epic 10. Next improvements are deliberately productization-focused:

- Add real authentication with Azure Entra ID or Entra External ID.
- Replace MVP HTTP event simulation with Azure Service Bus topics/subscriptions.
- Add EF Core migrations and controlled migration deployment.
- Add API gateway policy examples with Azure API Management.
- Add browser-captured screenshots and a short demo script.
- Add OpenTelemetry traces and richer dashboards.

See [roadmap](docs/roadmap.md).

## Portfolio Value

This repository is designed to show Solution Architect-level thinking, not only feature delivery. It demonstrates enterprise modernization patterns, banking domain modeling, API governance, cloud-native Azure planning, event-driven workflow design, observability, secure configuration, containerization, CI/CD, architecture documentation, and pragmatic MVP scope control.
