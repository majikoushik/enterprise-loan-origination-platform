# Developer Setup

## Prerequisites

- .NET 8 SDK.
- Node.js 22 or another Angular 18-compatible LTS/current Node version.
- npm.
- Docker Desktop.
- Git.
- Azure CLI only when validating the Bicep blueprint.

## First-Time Setup

```powershell
dotnet restore EnterpriseLoanOriginationPlatform.sln
dotnet build EnterpriseLoanOriginationPlatform.sln
```

```powershell
cd src/web/loan-portal-angular
npm install
```

## Run With Docker Compose

```powershell
docker compose --profile services --profile frontend build
docker compose --profile services --profile frontend up -d
```

Open `http://localhost:4200`.

## Run Services Individually

Start only SQL Server:

```powershell
docker compose up sqlserver -d
```

Run an API:

```powershell
dotnet run --project src/services/Customer.Api/Customer.Api.csproj
```

Run the Angular portal:

```powershell
cd src/web/loan-portal-angular
npm start
```

## Local Ports

| Component | URL |
| --- | --- |
| Angular portal | `http://localhost:4200` |
| Customer API | `http://localhost:7101` |
| Loan Application API | `http://localhost:7102` |
| Eligibility API | `http://localhost:7103` |
| Notification Worker/API | `http://localhost:5004` |
| Audit API | `http://localhost:5005` |
| SQL Server | `localhost:1433` |

## Security Note

The Docker Compose SQL password is for local developer execution only. Do not reuse it in Azure or any shared environment.
