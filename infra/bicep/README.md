# Azure Bicep Infrastructure

This directory contains the Bicep templates used to deploy the cloud-native infrastructure for the Enterprise Loan Origination Platform.

## Architecture Highlights
The blueprint leverages Azure PaaS / Serverless components to maximize scalability while minimizing operational overhead:
- **Azure Container Apps**: Hosting API backends and background workers.
- **Azure SQL Database**: Relational storage for all microservices.
- **Azure Service Bus**: Asynchronous, event-driven messaging.
- **Azure Key Vault**: Secrets and connection string management.
- **Application Insights & Log Analytics**: Centralized observability.
- **Azure Container Registry**: Private Docker image hosting.
- **Azure Static Web Apps**: Edge-cached frontend hosting.

## Deployment Commands
To perform a dry-run of the deployment:
```bash
az deployment group what-if \
  --resource-group rg-entloan-dev \
  --template-file main.bicep \
  --parameters parameters/dev.parameters.json \
  --parameters sqlAdministratorLoginPassword="YourStrong!Password123"
```

To execute the deployment:
```bash
az deployment group create \
  --resource-group rg-entloan-dev \
  --template-file main.bicep \
  --parameters parameters/dev.parameters.json \
  --parameters sqlAdministratorLoginPassword="YourStrong!Password123"
```

> **Warning:** Never commit real passwords to `parameters.json` files. Use secure parameters during execution or fetch them from an existing Key Vault.
