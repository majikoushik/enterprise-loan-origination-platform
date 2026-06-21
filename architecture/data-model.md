# Data Model

## Ownership Model

Each service owns its persistence model. This prevents a shared-database anti-pattern while keeping the MVP easy to run locally on one SQL Server instance.

| Service | Logical Database | Key Tables |
| --- | --- | --- |
| Customer API | `EnterpriseLoan_Customer` | `Customers` |
| Loan Application API | `EnterpriseLoan_LoanApplication` | `LoanApplications`, `ApplicationStatusHistory` |
| Eligibility API | `EnterpriseLoan_Eligibility` | `EligibilityResults` |
| Notification Worker/API | `EnterpriseLoan_Notification` | `NotificationRequests`, `NotificationDeliveryAttempts` |
| Audit API | `EnterpriseLoan_Audit` | `AuditEvents` |

## Core Entities

### Customer

Stores synthetic customer profile details such as full name, email, mobile number, date of birth, employment type, monthly income, and existing monthly obligations.

### Loan Application

Stores customer reference, loan type, requested amount, tenure, purpose, declared income, existing obligations, status, and timestamps. Status transitions are controlled by domain rules.

### Eligibility Result

Stores application reference, decision, explanation, rule version, evaluated timestamp, and per-rule results for transparency.

### Notification Request

Stores simulated email/SMS notification intent, status, payload summary, and delivery attempts. It does not call a real provider in the MVP.

### Audit Event

Stores immutable business trace records including event type, category, entity, customer reference when applicable, actor, action, summary, metadata, source service, severity, occurred timestamp, recorded timestamp, and correlation ID.

## Data Protection Rules

- Use synthetic demo data only.
- Avoid storing unnecessary personal identifiers.
- Do not store secrets in tables or audit metadata.
- Treat income and obligations as demo financial data, not real customer records.

## Future Persistence Work

- Add EF Core migrations per service.
- Add row versioning where concurrent edits become realistic.
- Add pagination and filtered indexes for operational queries.
- Add backup/restore and retention policies for Azure SQL.
