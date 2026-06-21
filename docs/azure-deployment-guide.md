# Azure Deployment Guide

## Target Architecture Overview
The **Enterprise Loan Origination Platform** is designed for a cloud-native Azure environment.
- **Frontend**: Azure Static Web Apps (Edge CDN, lightweight, highly scalable).
- **Backend**: Azure Container Apps (Serverless containers, KEDA scaling).
- **Database**: Azure SQL Database (Managed, elastic pools).
- **Messaging**: Azure Service Bus (Decoupled events).
- **Observability**: Azure Application Insights & Log Analytics (End-to-End Tracing).
- **Security**: Azure Key Vault & Managed Identities.

## Prerequisites
- Azure Subscription.
- Azure CLI (`az`) installed locally.
- GitHub repository with Actions enabled.

## Environment Variables and GitHub Secrets
To automate deployment via `.github/workflows/azure-deploy-template.yml`, the following secrets must be configured in your GitHub repository:
- `AZURE_CLIENT_ID`, `AZURE_TENANT_ID`, `AZURE_SUBSCRIPTION_ID`: Required for OIDC federated authentication to Azure.
- `SQL_ADMIN_PASSWORD`: Passed as a secure parameter to the SQL Database deployment.
- `ACR_LOGIN_SERVER`: The URI of the Container Registry (e.g., `entloanacrdev.azurecr.io`).

## Key Vault and Managed Identity Strategy
1. **Managed Identity**: Container Apps are assigned System-Assigned Managed Identities.
2. **Role Assignments**: The Managed Identities are granted `Key Vault Secrets User` roles.
3. **Configuration**: Instead of placing raw connection strings in environment variables, the Container Apps retrieve `ConnectionStrings__CustomerDb` from Key Vault dynamically on startup.

## Deployment Flow
### 1. Infrastructure as Code
Navigate to `infra/bicep/` and run a `what-if` deployment:
```bash
az deployment group what-if --resource-group <rg-name> --template-file main.bicep --parameters parameters/dev.parameters.json sqlAdministratorLoginPassword="<pwd>"
```
### 2. Container Image Build and Push
Trigger the `Container Build` workflow to package the `.NET` applications into Docker images and push them to ACR.

### 3. Container Apps Deployment
Once the infrastructure exists and ACR is populated, you can update the Container App revisions using the Azure CLI:
```bash
az containerapp update --name customer-api --resource-group <rg-name> --image <acr-name>.azurecr.io/customer-api:latest
```

### 4. Angular Frontend Deployment
Run the Azure Static Web Apps CLI (`swa`) or use the official `Azure/static-web-apps-deploy` GitHub Action to push the `dist/loan-portal-angular` folder to the Static Web App instance.

## Cost Awareness
- **Demo Environments**: The provided `dev.parameters.json` configures services securely but defaults to low-tier SKUs (e.g., `Basic` SQL Database, `Free` Static Web Apps).
- **Cleanup**: To prevent ongoing charges after a demo, delete the entire Resource Group:
```bash
az group delete --name rg-entloan-dev --yes --no-wait
```
