# Audit Logging Data Model

## Overview
The `Audit.Api` service owns the `EnterpriseLoan_Audit` database, which stores immutable records of business actions across the platform.

### Table: AuditEvents
Stores centralized events from all microservices.

| Column | Type | Description |
|---|---|---|
| `Id` | `UNIQUEIDENTIFIER` | Primary Key |
| `EventId` | `UNIQUEIDENTIFIER` | Correlates to an original domain/integration event |
| `CorrelationId` | `NVARCHAR(100)` | Used to track flows across services |
| `EventType` | `NVARCHAR(100)` | E.g., `CustomerRegistered`, `LoanApplicationSubmitted` |
| `Category` | `NVARCHAR(50)` | E.g., `Customer`, `LoanApplication`, `Eligibility`, `Notification` |
| `EntityType` | `NVARCHAR(100)` | Primary entity involved (e.g. `Customer`, `LoanApplication`) |
| `EntityId` | `NVARCHAR(100)` | ID of the primary entity |
| `CustomerId` | `UNIQUEIDENTIFIER` | Nullable reference to the customer (if applicable) |
| `ActorType` | `NVARCHAR(50)` | E.g., `System`, `Customer`, `Underwriter` |
| `ActorId` | `NVARCHAR(100)` | ID of the actor performing the action |
| `Action` | `NVARCHAR(100)` | E.g., `Register`, `Submit`, `ChangeStatus`, `Evaluate` |
| `Summary` | `NVARCHAR(500)` | Human-readable summary |
| `MetadataJson` | `NVARCHAR(MAX)` | Additional context payload in JSON (must NOT contain secrets/PII) |
| `OccurredAtUtc` | `DATETIMEOFFSET` | When the event actually happened |
| `RecordedAtUtc` | `DATETIMEOFFSET` | When the event was saved in the audit log |
| `SourceService` | `NVARCHAR(100)` | E.g., `Customer.Api` |
| `Severity` | `NVARCHAR(50)` | E.g., `Info`, `Warning`, `Error` |

#### Indexes
- `IX_AuditEvents_EntityId`
- `IX_AuditEvents_CorrelationId`
- `IX_AuditEvents_OccurredAtUtc`
- `IX_AuditEvents_CustomerId`
