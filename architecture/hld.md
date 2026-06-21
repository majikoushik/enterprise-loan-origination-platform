# High-Level Design

## Business Context

The platform supports a simplified loan origination journey for portfolio demonstration: customer registration, profile lookup, loan application submission, rule-based eligibility evaluation, status tracking, notification simulation, and audit logging.

The implementation is intentionally honest about its scope. It demonstrates architecture patterns and banking workflow design; it does not claim to be a production credit bureau integration, a regulatory decision engine, or a complete core banking system.

## System Context

Primary users are customers, loan operations users, and future administrators. The Angular portal communicates with domain APIs over HTTP. APIs own their service data and emit or simulate business events for notification and audit workflows.

External production integrations are represented as future-ready boundaries:

- Azure Service Bus for asynchronous integration events.
- Azure Key Vault for secret retrieval.
- Azure Application Insights and Log Analytics for telemetry.
- Azure Entra ID or Entra External ID for future authentication.
- Azure API Management and Front Door as optional edge/gateway components.

## Container View

| Container | Responsibility |
| --- | --- |
| Angular Loan Portal | Enterprise dashboard, customer registration, loan submission, status, eligibility, notifications, audit trail |
| Customer API | Customer profile and registration ownership |
| Loan Application API | Application submission, lifecycle, status transitions, history |
| Eligibility API | Demo rule evaluation and eligibility result storage |
| Notification Worker/API | Email/SMS notification request simulation and outcome tracking |
| Audit API | Centralized immutable business event trail |
| SQL Server / Azure SQL | Relational persistence for service-owned databases |
| Messaging abstraction | MVP HTTP simulation now, Azure Service Bus-compatible direction |

## Service Responsibilities

Customer API owns customer master data. Loan Application API owns the loan lifecycle and status rules. Eligibility API owns rule evaluation and decision explanations. Notification Worker owns delivery simulation. Audit API owns business traceability and does not mutate business state.

## Integration Points

- Angular to APIs: synchronous REST calls.
- Loan Application API to Notification Worker and Audit API: MVP HTTP event publication.
- Eligibility API to Loan Application API: HTTP lookup of application facts for evaluation.
- Eligibility API to Notification Worker and Audit API: MVP HTTP event publication.
- Future production eventing: Azure Service Bus topics/subscriptions for business facts.

## Key Quality Attributes

The architecture prioritizes maintainability, testability, observability, secure configuration, Azure readiness, and clear domain boundaries. Distributed complexity is introduced only where it clarifies ownership or production direction.
