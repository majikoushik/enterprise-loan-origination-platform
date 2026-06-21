# Data Model

Epic 0 does not create database tables. The conceptual model is prepared for future epics.

## Implemented Entities

### Customer (Epic 1)

Implemented in `Customer.Api` using EF Core. Fields include `Id`, `FullName`, `Email`, `MobileNumber`, `DateOfBirth`, `EmploymentType`, `MonthlyIncome`, `ExistingMonthlyObligations`, and `CreatedAt`.

### LoanApplication (Epic 2 & 4)

Implemented in `LoanApplication.Api` using EF Core. Fields include `Id`, `CustomerId`, `LoanType`, `RequestedAmount`, `RequestedTenureInMonths`, `Purpose`, `DeclaredMonthlyIncome`, `ExistingEmiObligations`, `Status`, `CreatedAt`, and `UpdatedAt`.

Epic 4 introduced `ApplicationStatusHistory` to track all lifecycle changes. It includes `Id`, `ApplicationId`, `PreviousStatus`, `NewStatus`, `Reason`, `ChangedBy`, and `ChangedAt`.

### EligibilityResult (Epic 3)

Implemented in `Eligibility.Api` using EF Core. Fields include `Id`, `ApplicationId`, `CustomerId`, `Decision`, `RuleVersion`, `EvaluatedAt`, financial snapshots, and a collection of `RuleResults`.

## Future Candidate Entities

### NotificationRequest

Fields will include channel, recipient reference, event type, delivery status, correlation ID, and timestamps.

### AuditEvent

Fields will include event type, entity type, entity ID, timestamp, correlation ID, and summary message.

## Database Direction

SQL Server is used locally and Azure SQL Database is the cloud target. EF Core migrations should be introduced with the first persisted feature.
