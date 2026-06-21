# Testing Strategy

## Goals

Testing demonstrates that the platform is not only shaped like an enterprise system, but behaves predictably around domain rules, validation, API contracts, observability, and integration seams.

## Backend Coverage

Current backend test projects cover:

- Customer registration application service behavior.
- Customer registration validation.
- Loan application submission service behavior.
- Loan application request validation.
- Loan application domain status transition rules.
- Eligibility rule engine and individual eligibility rules.
- Eligibility service behavior.
- Notification internal event endpoint behavior.
- Audit API endpoint behavior and audit HTTP logger behavior.
- Observability middleware and global exception handler behavior.

Command:

```powershell
dotnet test EnterpriseLoanOriginationPlatform.sln --configuration Release
```

## Frontend Coverage

The Angular project includes component test scaffolding and should continue to add tests for:

- Reactive form validation.
- API service wrappers.
- Error display behavior.
- Routing smoke tests.
- Status badge and empty-state rendering.

Command:

```powershell
cd src/web/loan-portal-angular
npm test -- --watch=false --browsers=ChromeHeadless
```

## CI Quality Gates

GitHub Actions validates:

- .NET restore, build, and tests.
- Angular dependency install, build, and tests.
- Docker Compose image build for services and frontend.

## Test Data Rules

Use synthetic test data only. Do not include real customer names, identifiers, phone numbers, bank data, or production financial records.

## Future Test Enhancements

- API integration tests using WebApplicationFactory for more endpoints.
- Contract snapshot tests for response envelopes and Problem Details.
- Playwright smoke tests for the Angular portal.
- Bicep validation in CI if Azure CLI is available.
