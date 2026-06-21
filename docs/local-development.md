# Local Development Guide

## Recommended Workflow

Use Docker Compose when validating the full platform and use direct `dotnet run` / `npm start` when debugging a single service or Angular feature.

## IDE Setup

- Visual Studio 2022 or JetBrains Rider for .NET services.
- VS Code for Angular frontend work.
- Docker Desktop for SQL Server and container validation.

## Backend Development

```powershell
dotnet restore EnterpriseLoanOriginationPlatform.sln
dotnet build EnterpriseLoanOriginationPlatform.sln
dotnet test EnterpriseLoanOriginationPlatform.sln
```

Run a single service:

```powershell
dotnet run --project src/services/LoanApplication.Api/LoanApplication.Api.csproj
```

## Frontend Development

```powershell
cd src/web/loan-portal-angular
npm install
npm start
```

The portal uses typed services and environment configuration for API URLs. Avoid direct `HttpClient` calls from components.

## Health and Diagnostics

APIs expose:

- `/health/live`
- `/health/ready`
- `/health`
- `/swagger` in development

Use the Angular System Health view for a quick service status check.

## Troubleshooting

- If SQL connectivity fails, check `docker ps` and the SQL Server container health.
- If Angular API calls fail, confirm the target API port and CORS settings.
- If a request fails, copy the correlation ID from the response or browser network tab and search backend logs.
