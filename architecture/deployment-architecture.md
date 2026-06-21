# Deployment Architecture

## Local Development

Local development uses:

- .NET SDK for APIs and worker
- Angular CLI for frontend
- Docker Compose for SQL Server and future dependencies

## Azure Target Architecture

- Angular Portal: Azure Static Web Apps or Azure Storage Static Website
- APIs and workers: Azure Container Apps
- Images: Azure Container Registry
- Data: Azure SQL Database
- Messaging: Azure Service Bus
- Secrets: Azure Key Vault
- Monitoring: Application Insights and Log Analytics
- Optional gateway: Azure API Management
- Optional edge: Azure Front Door

## Container App Breakdown

- **Customer API App**: Hosts `Customer.Api` container.
- **LoanApplication API App**: Hosts `LoanApplication.Api` container.
- **Eligibility API App**: Hosts `Eligibility.Api` container.
- **Notification Worker App**: Hosts `Notification.Worker` container. Operates as an API to receive internal webhooks and runs a BackgroundService to process and simulate delivery of messages (Epic 5).

## Deployment Order

1. Provision shared observability and registry.
2. Provision database and messaging.
3. Provision Key Vault and managed identities.
4. Build and push container images.
5. Deploy APIs and worker.
6. Deploy Angular portal.
7. Validate health endpoints and smoke tests.

## Configuration

Environment-specific values must come from environment variables, managed identities, Key Vault references, or Azure platform configuration.

## Cost Awareness

Initial Azure environments should use small SKUs and consumption-based services where practical. Production-scale choices require explicit cost review.
