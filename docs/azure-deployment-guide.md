# Azure Deployment Guide

## Target Architecture

The platform is designed for a cloud-native Azure environment:

- Frontend: Azure Static Web Apps.
- Backend: Azure Container Apps.
- Database: Azure SQL Database.
- Messaging: Azure Service Bus.
- Secrets: Azure Key Vault with Managed Identity.
- Observability: Application Insights and Log Analytics.
- Images: Azure Container Registry.

## Prerequisites

- Azure subscription.
- Azure CLI.
- GitHub Actions enabled.
- Permission to create a resource group, managed identities, Container Apps, SQL, Service Bus, Key Vault, and monitoring resources.

## Validate Bicep Locally

```powershell
az bicep build --file infra/bicep/main.bicep
```

## What-If Deployment

```powershell
az deployment group what-if `
  --resource-group rg-entloan-dev `
  --template-file infra/bicep/main.bicep `
  --parameters infra/bicep/parameters/dev.parameters.json `
  --parameters sqlAdministratorLoginPassword="<secure-password>"
```

## Create Deployment

```powershell
az deployment group create `
  --resource-group rg-entloan-dev `
  --template-file infra/bicep/main.bicep `
  --parameters infra/bicep/parameters/dev.parameters.json `
  --parameters sqlAdministratorLoginPassword="<secure-password>"
```

## GitHub Secrets

For OIDC-based deployments, configure:

- `AZURE_CLIENT_ID`
- `AZURE_TENANT_ID`
- `AZURE_SUBSCRIPTION_ID`

For demo image/database deployment, configure:

- `SQL_ADMIN_PASSWORD`
- `ACR_LOGIN_SERVER`

Use environment protection rules for shared or production-like environments.

## Deployment Flow

1. Run CI quality gates.
2. Validate Bicep.
3. Deploy infrastructure with a secure SQL admin password.
4. Build and push service images to ACR.
5. Update Azure Container Apps revisions.
6. Deploy Angular static assets to Azure Static Web Apps.
7. Smoke test `/health/ready`, `/swagger`, and the Angular portal.

## Key Vault and Managed Identity Strategy

Container Apps should use managed identities to access Key Vault. Connection strings and API keys should be stored as Key Vault secrets or Container Apps secret references, never plain text in source control.

## Cost Awareness

Use demo-friendly SKUs and tags. Review costs before leaving resources running. Delete the demo resource group after evaluation:

```powershell
az group delete --name rg-entloan-dev --yes --no-wait
```

## Known Blueprint Limitations

- The templates are a deployment blueprint, not a one-click production release.
- Real authentication, Service Bus consumers, database migrations, backup policies, and API gateway policies need hardening before production.
