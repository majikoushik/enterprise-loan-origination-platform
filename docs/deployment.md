# Deployment

## Current State

The repository is deployment-blueprint ready. It includes Dockerfiles, Docker Compose, GitHub Actions CI, Azure deployment templates, and deployment documentation. It does not deploy real Azure resources automatically.

## Local Containers

```powershell
docker compose --profile services --profile frontend build
docker compose --profile services --profile frontend up -d
```

Local services:

- Angular portal: `http://localhost:4200`
- Customer API: `http://localhost:7101`
- Loan Application API: `http://localhost:7102`
- Eligibility API: `http://localhost:7103`
- Notification Worker/API: `http://localhost:5004`
- Audit API: `http://localhost:5005`

## Azure Production Direction

The target Azure architecture uses:

- Azure Static Web Apps for the Angular frontend.
- Azure Container Apps for backend APIs and workers.
- Azure Container Registry for images.
- Azure SQL Database for service-owned persistence.
- Azure Service Bus for asynchronous events.
- Azure Key Vault and Managed Identity for secrets.
- Application Insights and Log Analytics for observability.

## Deployment Validation

Recommended validation sequence:

```powershell
dotnet restore EnterpriseLoanOriginationPlatform.sln
dotnet build EnterpriseLoanOriginationPlatform.sln --configuration Release
dotnet test EnterpriseLoanOriginationPlatform.sln --configuration Release
```

```powershell
cd src/web/loan-portal-angular
npm ci
npm run build
npm run test:ci
```

```powershell
docker compose --profile services --profile frontend build
az bicep build --file infra/bicep/main.bicep
```

## Secrets

Secrets must be supplied through environment variables, GitHub Actions secrets, Key Vault, or managed identity. Do not commit production credentials.

## Production Readiness Checklist

- Configure authentication and authorization.
- Replace MVP HTTP event simulation with Service Bus.
- Apply EF Core migrations through a controlled deployment flow.
- Configure App Insights dashboards and alerts.
- Validate backups, restore, and retention policies.
- Review Azure cost and cleanup plan.
