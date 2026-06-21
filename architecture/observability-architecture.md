# Observability Architecture

## Correlation IDs

All APIs use `X-Correlation-ID`. If the header is missing, the API creates one and returns it to the caller.

## Structured Logging

Future logging should use structured fields:

- Service name
- Correlation ID
- Event name
- Entity type
- Entity ID
- Outcome
- Exception details where applicable

## Health Checks

APIs expose `/health`. Future checks should include database and message bus readiness as dependencies are added.

## Metrics

Future service metrics should include request counts, latency, validation failures, eligibility outcomes, notification outcomes, and audit write failures.

## Distributed Tracing

The target Azure implementation should integrate with Application Insights and OpenTelemetry-compatible tracing.

## Log Analytics

Azure Log Analytics will centralize logs and support operational queries across APIs and workers.
