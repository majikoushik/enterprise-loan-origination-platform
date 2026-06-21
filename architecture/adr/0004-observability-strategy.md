# ADR-0004: Observability Strategy

## Status

Accepted

## Context

Enterprise banking systems require operational traceability, incident support, and production readiness from the beginning.

## Decision

Adopt correlation ID propagation, structured logging conventions, health checks, and Application Insights readiness. Use `X-Correlation-ID` as the standard request correlation header.

## Consequences

Support teams can trace requests across services as the platform grows. Future epics must avoid logging sensitive data and add dependency-specific health checks as persistence and messaging are introduced.

## Alternatives Considered

- Add observability after MVP: faster initially but creates rework and weak production-readiness posture.
- Use ad hoc logging only: insufficient for distributed support and architecture demonstration.
