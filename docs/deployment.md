# Deployment

## Current State

Epic 0 adds deployment documentation and CI foundation. Production deployment templates will be introduced in the Azure deployment epic.

## Local Containers

For instructions on how to use Docker Compose and spin up the environment quickly on your laptop, please refer to the [DevOps Guide](devops-guide.md).

## Azure Production Deployment
Our cloud-native topology relies on **Azure Container Apps** and **Azure SQL Database**.
To understand how the Bicep IaC works and how to set up GitHub Actions CI/CD pipelines for Azure, please read the [Azure Deployment Guide](azure-deployment-guide.md).

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
