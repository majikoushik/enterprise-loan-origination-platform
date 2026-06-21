# Testing Strategy

## Backend

Backend testing should include:

- Domain rule tests
- Application service tests
- Request validation tests
- API integration tests where practical
- Failure and invalid transition tests

Epic 0 includes placeholder test projects to establish CI structure.

## Frontend

Frontend testing should include:

- Component tests
- Form validation tests
- API service tests
- Routing smoke tests

Epic 0 includes an Angular shell creation test.

## Test Data

Only synthetic data may be used. Do not use real customer or financial records.

## Commands

```powershell
dotnet test EnterpriseLoanOriginationPlatform.sln
```

```powershell
cd src/web/loan-portal-angular
npm test -- --watch=false --browsers=ChromeHeadless
```
