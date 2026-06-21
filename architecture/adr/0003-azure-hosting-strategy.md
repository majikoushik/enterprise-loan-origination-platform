# ADR-0003: Azure Hosting Strategy

## Status

Accepted

## Context

The platform should be Azure-ready without forcing cloud deployment during local MVP development.

## Decision

Use Azure Container Apps for backend APIs and workers, Azure Static Web Apps or Azure Storage Static Website for Angular hosting, Azure Container Registry for images, Azure SQL Database for data, Azure Service Bus for messaging, Key Vault for secrets, and Application Insights with Log Analytics for observability.

## Consequences

The hosting model supports independent service deployment and managed operational capabilities. Future infrastructure work must document cost, environment variables, secrets, and deployment order.

## Alternatives Considered

- Azure App Service: simpler for APIs but less aligned with worker and event-driven container workloads.
- Azure Kubernetes Service: powerful but too operationally heavy for the MVP and portfolio scope.
- Virtual machines: flexible but not cloud-native enough for the target architecture.
