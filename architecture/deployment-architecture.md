# Deployment Architecture

## Local Deployment
For local testing and validation, the platform relies on **Docker Compose** to spin up the entire ecosystem simultaneously.
- **Frontend**: Nginx-based Angular build
- **Backend APIs**: `mcr.microsoft.com/dotnet/aspnet:8.0` containers
- **Database**: `mcr.microsoft.com/mssql/server:2022-latest`
- **Network**: All containers run on an internal default docker bridge network, allowing standard DNS resolution (e.g. `Server=sqlserver,1433`).

## Future Target: Azure Architecture

The current containerized infrastructure provides a 1-to-1 migration path to the following Azure resources:

### 1. Azure Container Registry (ACR)
All Dockerfiles created in this solution will be built and pushed to a secure ACR instance during a GitHub Actions CI/CD deployment pipeline.

### 2. Azure Container Apps (ACA)
The backend APIs (`Customer.Api`, `LoanApplication.Api`, `Eligibility.Api`, `Audit.Api`) and the `Notification.Worker` will be hosted on Azure Container Apps.
- **Why ACA?**: It provides serverless container execution, automatic KEDA-based scaling, built-in envoy proxy ingress, and seamless integration with Azure Managed Identities.
- **Health Probes**: ACA will utilize the `/health/live` and `/health/ready` endpoints exposed by the APIs to route traffic efficiently and restart failing pods.

### 3. Azure Static Web Apps (or Storage Website)
While the Angular app *can* run inside ACA via Nginx (as done locally), the most cost-effective and globally performant Azure solution is **Azure Static Web Apps**. The build output (`dist/`) will be synced directly to the edge.

### 4. Azure SQL Database
The local Docker SQL Server will be swapped for a managed Azure SQL Database logical server. Connection strings will be injected securely at runtime via Azure Key Vault.

### 5. Azure Service Bus
In the MVP, inter-service messaging is simulated via HTTP webhooks. When moving to production Azure, Azure Service Bus Topics/Queues will be provisioned, and the `Messaging` building block will be refactored to emit real AMQP events.
