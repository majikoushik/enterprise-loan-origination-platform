# Local Development

## Daily Workflow

1. Start SQL Server when persistence is required.
2. Run the API being developed with `dotnet run`.
3. Run the Angular portal with `npm start`.
4. Execute targeted tests before committing.

## Backend Commands

```powershell
dotnet restore EnterpriseLoanOriginationPlatform.sln
dotnet build EnterpriseLoanOriginationPlatform.sln
dotnet test EnterpriseLoanOriginationPlatform.sln
```

## Frontend Commands

```powershell
cd src/web/loan-portal-angular
npm install
npm run build
npm test -- --watch=false --browsers=ChromeHeadless
```

## Configuration

Use environment variables, user secrets, or local-only configuration files ignored by Git. Do not commit credentials.

For SQL Server in Docker Compose, set `SQLSERVER_SA_PASSWORD` before starting the container.

## Correlation IDs

The Angular HTTP interceptor sends `X-Correlation-ID`. APIs return the same header for troubleshooting.
