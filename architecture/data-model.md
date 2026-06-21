# Data Model

Epic 0 does not create database tables. The conceptual model is prepared for future epics.

## Implemented Entities

### Customer (Epic 1)

Implemented in `Customer.Api` using EF Core. Fields include `Id`, `FullName`, `Email`, `MobileNumber`, `DateOfBirth`, `EmploymentType`, `MonthlyIncome`, `ExistingMonthlyObligations`, and `CreatedAt`.

### LoanApplication (Epic 2)

Implemented in `LoanApplication.Api` using EF Core. Fields include `Id`, `CustomerId`, `LoanType`, `RequestedAmount`, `RequestedTenureInMonths`, `Purpose`, `DeclaredMonthlyIncome`, `ExistingEmiObligations`, `Status`, `CreatedAt`, and `UpdatedAt`.

## Future Candidate Entities

### EligibilityResult

Fields will include application ID, decision, rule version, rule outcomes, explanation, and evaluated timestamp.

### NotificationRequest

Fields will include channel, recipient reference, event type, delivery status, correlation ID, and timestamps.

### AuditEvent

Fields will include event type, entity type, entity ID, timestamp, correlation ID, and summary message.

## Database Direction

SQL Server is used locally and Azure SQL Database is the cloud target. EF Core migrations should be introduced with the first persisted feature.
