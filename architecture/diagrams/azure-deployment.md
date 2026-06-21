# Azure Deployment Diagram

```mermaid
flowchart TB
  FrontDoor[Azure Front Door optional] --> StaticWeb[Azure Static Web Apps or Storage Static Website]
  StaticWeb --> ContainerApps[Azure Container Apps APIs]
  ContainerApps --> Sql[Azure SQL Database]
  ContainerApps --> ServiceBus[Azure Service Bus]
  ContainerApps --> KeyVault[Azure Key Vault]
  ContainerApps --> AppInsights[Application Insights]
  AppInsights --> LogAnalytics[Log Analytics]
  Acr[Azure Container Registry] --> ContainerApps
```
