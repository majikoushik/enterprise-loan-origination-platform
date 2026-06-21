# Deployment

## Current State

Epic 0 adds deployment documentation and CI foundation. Production deployment templates will be introduced in the Azure deployment epic.

## Local Containers

```powershell
$env:SQLSERVER_SA_PASSWORD = "<local-development-password>"
docker compose up sqlserver
```

The `services` Docker Compose profile can be extended as API Dockerfiles are added.

## Azure Direction

Future deployment will use:

- Azure Container Registry for container images
- Azure Container Apps for APIs and workers
- Azure SQL Database for persistence
- Azure Service Bus for messaging
- Azure Key Vault for secrets
- Azure Application Insights and Log Analytics for observability
- Azure Static Web Apps or Azure Storage Static Website for Angular

## Secrets

Secrets must be provided through environment variables, Key Vault references, or managed identity. Do not store secrets in source control.

## Deployment Validation

Deployment pipelines should validate:

- Container image build
- API health endpoints
- Database migration status
- Angular artifact build
- Smoke tests
