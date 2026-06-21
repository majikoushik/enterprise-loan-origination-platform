# Developer Setup

## Prerequisites

- .NET 8 SDK
- Node.js 22 or another supported Node version for Angular 18
- npm
- Docker Desktop
- Git

## Initial Setup

```powershell
dotnet restore EnterpriseLoanOriginationPlatform.sln
cd src/web/loan-portal-angular
npm install
```

## Local Database Dependency

```powershell
$env:SQLSERVER_SA_PASSWORD = "<local-development-password>"
docker compose up sqlserver
```

Use a local-only password that satisfies SQL Server complexity requirements. Do not commit local credentials.

## Run A Backend API

```powershell
dotnet run --project src/services/Customer.Api/Customer.Api.csproj
```

Health endpoint:

```text
https://localhost:<port>/health
```

Swagger UI is enabled in development.

## Run The Angular Portal

```powershell
cd src/web/loan-portal-angular
npm start
```

Open `http://localhost:4200`.
