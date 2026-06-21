# ADR-0007: Angular Frontend Architecture

## Status

Accepted

## Context

The frontend must look and behave like an enterprise banking portal while remaining lightweight enough for a portfolio repository. It needs typed API integration, route-based features, validation, and future auth readiness.

## Decision

Use Angular standalone components with a feature-based folder structure. Keep typed models and API services under `core`, put workflow screens under `features`, and use interceptors for correlation IDs and error handling.

## Consequences

The portal is easy to scan, test, and extend by feature. Components avoid direct API plumbing and align with modern Angular practices. A heavier design system is deferred to avoid unnecessary dependency weight.

## Alternatives Considered

- Module-heavy legacy Angular structure: familiar, but less aligned with modern Angular.
- Introduce a large UI framework: faster widgets, but more visual and dependency overhead than needed.
- Single-page component prototype: quick, but not portfolio-grade or maintainable.
