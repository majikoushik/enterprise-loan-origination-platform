# Deployment Architecture

## Local Deployment

Local development uses Docker Compose to run the full platform or individual dependencies:

- SQL Server 2022 Developer container.
- .NET 8 API/worker containers.
- Nginx-hosted Angular build for containerized frontend validation.
- Docker bridge networking for service-to-service DNS.

Developers can also run APIs directly with `dotnet run` and the Angular portal with `npm start`.

## Azure Target Architecture

The Azure blueprint in `infra/bicep` provisions a cloud-native architecture:

- Azure Static Web Apps for the Angular portal.
- Azure Container Apps for APIs and worker-style services.
- Azure Container Registry for images.
- Azure SQL Database for relational persistence.
- Azure Service Bus for future production messaging.
- Azure Key Vault for secrets.
- Managed Identity for secret access and platform permissions.
- Application Insights and Log Analytics for observability.

## Deployment Flow

1. Validate Bicep with `az bicep build`.
2. Run `az deployment group what-if` against a demo resource group.
3. Build backend and frontend artifacts in CI.
4. Build and push container images to Azure Container Registry.
5. Deploy or update Azure Container Apps revisions.
6. Deploy Angular static assets.
7. Run smoke tests against health and metadata endpoints.

## Cost and Cleanup

The blueprint is intended for demo environments. Review SKUs before use, apply resource tags, and delete demo resource groups after portfolio review or testing.

## Production Gaps

Before a real production launch, add authentication, migration automation, secrets rotation, API gateway policies, WAF/edge controls, backup/restore tests, and incident response procedures.
