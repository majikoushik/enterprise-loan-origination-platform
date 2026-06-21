# DevOps and Docker Guide

## Overview
This guide provides instructions for building, testing, and running the **Enterprise Loan Origination Platform** locally and within continuous integration (CI) environments. The platform is designed to be fully containerized, setting a strong foundation for cloud-native deployment on Azure.

## 1. Local Development without Docker

For a standard debugging experience in Visual Studio or Rider:

1. **Database Setup**:
   Ensure you have a local SQL Server running. You can quickly spin one up using Docker without running the full platform:
   ```bash
   docker compose up sqlserver -d
   ```

2. **Run Backend Services**:
   Open a terminal in the root directory and restore/build the solution:
   ```bash
   dotnet restore EnterpriseLoanOriginationPlatform.sln
   dotnet build EnterpriseLoanOriginationPlatform.sln
   ```
   You can run each API project individually using their launch profiles. For example:
   ```bash
   cd src/services/Customer.Api
   dotnet run
   ```

3. **Run Frontend (Angular)**:
   Ensure Node.js v22+ is installed.
   ```bash
   cd src/web/loan-portal-angular
   npm install
   npm start
   ```
   The portal will be available at `http://localhost:4200`.

## 2. Local Development with Docker Compose

To validate the full microservice ecosystem in an isolated, production-like manner, use Docker Compose.

1. **Build all images**:
   ```bash
   docker compose --profile services --profile frontend build
   ```

2. **Start the ecosystem**:
   ```bash
   docker compose --profile services --profile frontend up -d
   ```

3. **Stop and clean up**:
   ```bash
   docker compose --profile services --profile frontend down
   ```

### Ports Reference
- **Angular UI**: `localhost:4200`
- **Customer API**: `localhost:7101`
- **Loan Application API**: `localhost:7102`
- **Eligibility API**: `localhost:7103`
- **Audit API**: `localhost:5005`
- **Notification Worker**: `localhost:5004`
- **SQL Server**: `localhost:1433`

## 3. Database Container and Migrations
The `sqlserver` service provisions a local developer edition of SQL Server with the `sa` user.
**Warning**: The password `YourStrong@Password!` is hardcoded in `docker-compose.yml` for local development **only**. Never use this in production.
Entity Framework Core is used for migrations. In a production pipeline, migrations should be applied out-of-band via a secure deployment script. Locally, EF Core `EnsureCreated()` or CLI tools are utilized.

## 4. Continuous Integration (GitHub Actions)
The repository uses `.github/workflows/ci.yml`. This pipeline enforces strict quality gates on every Pull Request to `main`:
1. **Backend Validation**: Restores, builds, and runs `.NET` unit tests.
2. **Frontend Validation**: Installs NPM dependencies, builds the Angular project, and executes Karma tests.
3. **Docker Validation**: Runs `docker compose build` to verify all `Dockerfile` configurations successfully compile into valid images without syntax errors.

## 5. Azure Target Architecture Readiness
The current Dockerfiles are optimized for deployment to **Azure Container Apps**:
- **Base Images**: Minimal size, utilizing official `.NET 8` and `Nginx` alpine images.
- **Environment Variables**: All services accept runtime overrides via standard ASP.NET Core `__` syntax (e.g., `ConnectionStrings__CustomerDb`).
- **Health Probes**: Docker images expose standard `/health/live` and `/health/ready` endpoints explicitly formatted for Azure Container App health probes.
- **Container Registry**: In a future deployment epic, GitHub actions will run `docker push` and publish these built images directly to **Azure Container Registry (ACR)**.
