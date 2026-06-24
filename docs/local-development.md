# Local Development Guide

## Recommended Workflow

Use `start-local-platform.bat` for a simple local startup, Docker Compose when validating the full container topology, and direct `dotnet run` / `npm start` when debugging a single service or Angular feature.

On Windows, double-click `start-local-platform.bat` from the repository root to start the backend services and Angular frontend without Docker. The script checks for `dotnet` and `npm`, starts each API on the same local ports used by the Angular environment configuration, and opens `http://localhost:4200`.

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

## SQL Server and Synthetic Fallback

SQL Server LocalDB or the Docker Compose SQL Server container is the primary development data store. Each backend service probes its configured connection string during startup. When SQL Server is unavailable and `DataStore:UseSyntheticFallbackWhenSqlUnavailable` is enabled, the service switches to EF Core in-memory storage and seeds synthetic screen data.

The fallback covers:

- Customers: Anika Rao, Marcus Lee, and Priya Shah.
- Loan applications: submitted, under review, eligibility passed, and eligibility failed examples.
- Eligibility results: one passed decision and one failed decision with rule explanations.
- Notifications: simulated email and SMS delivery records.
- Audit trail: customer, application, eligibility, and status-change events.

This fallback is intended for local demos and development only. To require SQL Server and fail fast on database outages, set the configuration value to `false`:

```powershell
$env:DataStore__UseSyntheticFallbackWhenSqlUnavailable = "false"
dotnet run --project src/services/Customer.Api/Customer.Api.csproj
```

## Docker Compose

```powershell
Copy-Item .env.example .env
# Edit .env and set SQL_SERVER_PASSWORD to a strong local-only password.
docker compose up -d sqlserver
docker compose --profile services up --build
docker compose --profile frontend up --build
```

The `.env` file is intentionally gitignored. Do not commit local SQL passwords or cloud credentials.

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

- If SQL connectivity fails and fallback is enabled, `/health/ready` reports the synthetic in-memory store as ready. If fallback is disabled, check `docker ps`, LocalDB status, and the SQL Server container health.
- If Angular API calls fail, confirm the target API port and CORS settings.
- If a request fails, copy the correlation ID from the response or browser network tab and search backend logs.
