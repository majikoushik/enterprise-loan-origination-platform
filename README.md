# Enterprise Loan Origination Platform

Portfolio-grade banking application foundation for a cloud-native loan origination journey. The repository demonstrates solution architecture, .NET backend engineering, Angular frontend engineering, Azure-ready deployment design, observability, security posture, and DevOps maturity.

## Architecture Summary

The platform is organized around domain-aligned service boundaries:

- `Customer.Api` for customer profile and registration ownership.
- `LoanApplication.Api` for application submission, lifecycle, and status transitions.
- `Eligibility.Api` for demo rule-based eligibility evaluation.
- `Notification.Worker` for simulated asynchronous notification delivery.
- `Audit.Api` for business event traceability.

Shared building blocks live under `src/building-blocks` and are intentionally small at this stage: response envelopes, integration event contracts, correlation ID middleware, and security role defaults.

The target Azure architecture uses Azure Container Apps, Azure SQL Database, Azure Service Bus, Azure Key Vault, Azure Application Insights, Log Analytics, Azure Container Registry, and Azure Static Web Apps or Storage Static Website for the Angular portal.

### Backend
- **Framework**: .NET 8, ASP.NET Core Web API
- **Language**: C# 12
- **Data Access**: Entity Framework Core, SQL Server
- **Architecture**: Clean Architecture, Domain-Driven Design (DDD)
- **Observability**: Serilog, Standard Health Checks, Global Exception Handling (ProblemDetails), Correlation ID tracking.

### Frontend
- SQL Server locally, Azure SQL for cloud alignment
- Swagger/OpenAPI readiness on every API
- Health check endpoints on APIs
- Docker Compose foundation for local dependencies and future service orchestration
- GitHub Actions CI foundation

## Repository Layout

```text
src/
  services/
    Customer.Api/
    LoanApplication.Api/
    Eligibility.Api/
    Notification.Worker/
    Audit.Api/
  building-blocks/
    SharedKernel/
    Messaging/
    Observability/
    Security/
  web/
    loan-portal-angular/
tests/
architecture/
docs/
infra/
.github/workflows/
```

## Current Epic Status

Epic 5: Notification Simulation is complete.

Implemented now:

- Customer Registration (Epic 1)
- Loan Application Submission (Epic 2)
- Rule-based Eligibility Evaluation engine with synchronous cross-service API calls (Epic 3)
- Application Status Tracking with strict state machine and timeline history (Epic 4)
- Notification Simulation via local event-driven webhook (Epic 5)
- Angular portal for all five epics.
- Backend solution and service skeletons
- Angular portal shell and route placeholders
- Correlation ID readiness
- Health check readiness
- Swagger/OpenAPI readiness
- Architecture documentation and ADRs
- Docker Compose and CI foundation
- Docker Compose and CI foundation

## Run Locally

Prerequisites:

- .NET 8 SDK
- Node.js LTS or current supported Node version
- Docker Desktop for SQL Server and future service orchestration

Backend example:

```powershell
dotnet restore EnterpriseLoanOriginationPlatform.sln
dotnet run --project src/services/Customer.Api/Customer.Api.csproj
```

Frontend:

```powershell
cd src/web/loan-portal-angular
npm install
npm start
```

Docker dependency foundation:

```powershell
$env:SQLSERVER_SA_PASSWORD = "<local-development-password>"
docker compose up sqlserver
```

## Build And Test

```powershell
dotnet build EnterpriseLoanOriginationPlatform.sln --configuration Release
dotnet test EnterpriseLoanOriginationPlatform.sln --configuration Release
```

```powershell
cd src/web/loan-portal-angular
npm ci
npm run build
npm test -- --watch=false --browsers=ChromeHeadless
```

## Documentation

- [Architecture Overview](architecture/README.md)
- [High-Level Design](architecture/hld.md)
- [Low-Level Design](architecture/lld.md)
- [API Governance](architecture/api-governance.md)
- [Security Architecture](architecture/security-architecture.md)
- [Observability Architecture](architecture/observability-architecture.md)
- [Deployment Architecture](architecture/deployment-architecture.md)
- [Developer Setup](docs/setup.md)
- [Roadmap](docs/roadmap.md)

## Security And Data Notice

No real customer data, credentials, secrets, or production financial data should be committed. All sample data introduced by future epics must be synthetic and clearly documented.

## Next Epic

Epic 3: Eligibility Evaluation, adding a rules-based engine for checking if an application passes minimum requirements.
