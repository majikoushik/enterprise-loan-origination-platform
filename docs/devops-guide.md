# DevOps and Docker Guide

## Overview

The repository includes a CI-ready development workflow for backend services, Angular frontend, container image validation, and Azure deployment blueprint validation.

## Local Build

```powershell
dotnet restore EnterpriseLoanOriginationPlatform.sln
dotnet build EnterpriseLoanOriginationPlatform.sln --configuration Release
dotnet test EnterpriseLoanOriginationPlatform.sln --configuration Release
```

```powershell
cd src/web/loan-portal-angular
npm ci
npm run build
npm test -- --watch=false --browsers=ChromeHeadless
```

## Docker Compose

Build:

```powershell
docker compose --profile services --profile frontend build
```

Run:

```powershell
docker compose --profile services --profile frontend up -d
```

Stop:

```powershell
docker compose --profile services --profile frontend down
```

## Ports

| Component | Port |
| --- | --- |
| Angular UI | `4200` |
| Customer API | `7101` |
| Loan Application API | `7102` |
| Eligibility API | `7103` |
| Notification Worker/API | `5004` |
| Audit API | `5005` |
| SQL Server | `1433` |

## GitHub Actions

`.github/workflows/ci.yml` validates:

- .NET restore, build, and tests.
- Angular install, build, and tests.
- Docker Compose builds for backend services and frontend.

Template workflows also document future container build and Azure deployment patterns.

## Azure Readiness

The Dockerfiles use official runtime images and support environment-based configuration. The Bicep blueprint is designed for Azure Container Apps, Azure SQL, Service Bus, Key Vault, App Insights, Log Analytics, ACR, and Static Web Apps.

## Local Secret Warning

The compose SQL Server password is intentionally local-only and exists for reproducible demos. Production and shared environments must use secret stores.
