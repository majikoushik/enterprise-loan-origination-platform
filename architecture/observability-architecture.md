# Observability Architecture

## Overview

The platform standardizes observability around correlation IDs, structured logs, health checks, predictable error responses, and Azure Monitor readiness. The goal is to make business workflows diagnosable across frontend, APIs, background work, and audit records.

## Correlation IDs

- Header: `X-Correlation-ID`.
- Origin: Angular interceptor or API middleware when the header is missing.
- Propagation: downstream HTTP calls, response envelopes, Problem Details, audit events, and logs.
- Purpose: support teams can trace a customer registration, application submission, eligibility check, notification request, and audit record as one transaction.

## Structured Logging

Services should log structured fields such as:

- `ServiceName`
- `Environment`
- `CorrelationId`
- `EventName`
- `EntityType`
- `EntityId`
- `SourceService`

Logs must not include secrets, passwords, connection strings, stack traces in client responses, or sensitive personal/financial data.

## Health Checks

Backend services expose health endpoints for local diagnostics and Azure Container Apps probes:

- `/health/live`: process liveness.
- `/health/ready`: service readiness and key dependency reachability.
- `/health`: aggregate health details for local diagnostics.

## Error Handling

Global exception handling maps failures to Problem Details-compatible responses. Business validation and domain rule failures should produce client-safe messages; unexpected errors should log diagnostic detail server-side and return a generic response with correlation ID.

## Azure Monitor Readiness

The target telemetry stack is Application Insights plus Log Analytics. The operational runbook includes example KQL patterns for tracing a transaction by correlation ID.

## Future Enhancements

- OpenTelemetry traces across HTTP and Service Bus.
- Dashboards for application submission volume, eligibility pass/fail rate, notification failures, and API latency.
- Alerts for readiness failures, high error rate, queue depth, and SQL connectivity failures.
