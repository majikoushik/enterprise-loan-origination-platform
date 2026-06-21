# Architecture Documentation

This folder contains the architecture evidence for the Enterprise Loan Origination Platform. The documents are written for reviewers who want to understand the design decisions behind the code: service boundaries, contracts, event flow, data ownership, security, observability, and Azure deployment direction.

## Document Map

| Document | Purpose |
| --- | --- |
| [High-Level Design](hld.md) | Business context, system context, container view, service responsibilities, integration points |
| [Low-Level Design](lld.md) | Service internals, API flow, validation flow, persistence, event handling, frontend structure |
| [Non-Functional Requirements](nfrs.md) | Scalability, reliability, security, observability, maintainability, cost awareness |
| [API Governance](api-governance.md) | REST conventions, response envelopes, Problem Details, versioning, OpenAPI expectations |
| [Security Architecture](security-architecture.md) | Secure defaults, secret management, future auth direction, OWASP posture |
| [Observability Architecture](observability-architecture.md) | Correlation IDs, structured logging, health checks, tracing, operational diagnostics |
| [Deployment Architecture](deployment-architecture.md) | Local Docker topology and Azure target architecture |
| [Data Model](data-model.md) | Service-owned relational model and audit/event traceability |
| [Event Model](event-model.md) | Integration event conventions and MVP notification/audit simulation |
| [ADRs](adr/) | Architecture decision records |

## Diagrams

- [System context](diagrams/system-context.md)
- [Container diagram](diagrams/container-diagram.md)
- [Loan application submission sequence](diagrams/loan-application-sequence.md)
- [Eligibility evaluation sequence](diagrams/eligibility-evaluation-sequence.md)
- [Notification and audit event flow](diagrams/notification-audit-event-flow.md)
- [Azure deployment](diagrams/azure-deployment.md)
- [CI/CD pipeline](diagrams/cicd-pipeline.md)

## Architecture Principle

The MVP is intentionally small enough to run locally, but it is organized as a credible enterprise banking platform: explicit domain boundaries, API-first contracts, testable application logic, operational diagnostics, secure configuration, and a clear path from Docker Compose to Azure Container Apps.
