# High-Level Design

## Business Context

The platform supports a simplified loan origination journey for portfolio demonstration: customer registration, loan application submission, eligibility evaluation, application status tracking, notification simulation, and audit logging.

## System Context

Primary users are customers, loan operations users, and future administrators. The Angular portal communicates with domain APIs. APIs persist their own data over time and publish business events for notification and audit workflows.

## Container View

- Angular Portal: browser-based enterprise dashboard hosted locally in development and on Azure Static Web Apps or Azure Storage Static Website in Azure.
- Customer API: owns customer profile and registration capabilities.
- Loan Application API: owns loan application submission and status lifecycle.
- Eligibility API: owns demo rule-based eligibility evaluation.
- Notification Worker: simulates email and SMS notification handling.
- Audit API: records and queries business audit events.
- SQL Server: local relational database, aligned with Azure SQL Database.
- Message Bus: in-memory or simulated messaging for MVP, aligned with Azure Service Bus for production.

## Service Responsibilities

Customer API owns customer master data. Loan Application API owns loan application lifecycle. Eligibility API owns eligibility decisions and explanations. Notification Worker owns simulated notification outcomes. Audit API owns traceability records.

## Integration Points

Initial integration is synchronous HTTP from the Angular portal to APIs. Future epics will introduce integration events for `CustomerRegistered`, `LoanApplicationSubmitted`, `EligibilityCheckCompleted`, `NotificationRequested`, and `AuditEventRecorded`.

## Quality Attributes

The architecture prioritizes maintainability, testability, security, observability, Azure readiness, and clear domain boundaries over premature distributed-system complexity.
