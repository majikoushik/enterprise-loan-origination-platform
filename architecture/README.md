# Architecture Documentation

This folder captures the architecture baseline for the Enterprise Loan Origination Platform. The documentation is intentionally created in Epic 0 so implementation work can evolve from clear architectural intent.

## Document Map

- [High-Level Design](hld.md): system context, containers, and service responsibilities.
- [Low-Level Design](lld.md): service internals, API flow, validation, error handling, and event handling approach.
- [Non-Functional Requirements](nfrs.md): scalability, availability, security, observability, maintainability, and cost considerations.
- [API Governance](api-governance.md): REST conventions, response envelopes, validation, versioning, and OpenAPI expectations.
- [Security Architecture](security-architecture.md): secure defaults, secret handling, future authentication, and OWASP posture.
- [Observability Architecture](observability-architecture.md): correlation IDs, logging, health checks, metrics, and tracing readiness.
- [Deployment Architecture](deployment-architecture.md): local and Azure deployment direction.
- [Data Model](data-model.md): initial conceptual entities and future persistence direction.
- [Event Model](event-model.md): integration event conventions and MVP event candidates.
- [ADRs](adr/): architecture decision records.

## Architecture Principle

The MVP stays simple enough to run locally, but the service boundaries, contracts, and operational patterns are designed for a realistic Azure-hosted banking platform.
