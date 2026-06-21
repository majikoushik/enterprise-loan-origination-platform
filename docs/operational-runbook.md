# Operational Runbook

## Purpose

This runbook helps developers and operators diagnose the portfolio platform locally and provides the shape of future Azure support procedures.

## Health Checks

Backend services expose:

- `GET /health/live`: process liveness.
- `GET /health/ready`: readiness for traffic and dependencies.
- `GET /health`: aggregate diagnostic health.

The Angular portal includes a System Health view for checking service readiness.

## Correlation ID Triage

Every request should carry `X-Correlation-ID`. Use it to connect frontend errors, API logs, audit events, and notification records.

Example Log Analytics query direction:

```kusto
AppTraces
| where Properties["CorrelationId"] == "123e4567-e89b-12d3-a456-426614174000"
| order by TimeGenerated asc
```

## Common Scenarios

### API Is Unhealthy

Check:

- SQL Server container is running and healthy.
- Connection string environment variable matches the service.
- `/health/ready` response details.
- Service console logs by correlation ID or startup failure.

### Angular Cannot Call API

Check:

- API container or `dotnet run` process is running.
- Port matches the Angular environment configuration.
- Browser network tab for CORS, DNS, or HTTP status.
- Problem Details response for correlation ID.

### Notification Simulation Does Not Appear

Check:

- Notification Worker/API is running.
- Loan Application or Eligibility API has the correct `Notification__WebhookUrl`.
- Audit API is running if audit records are expected.
- Notification service logs for failed webhook processing.

### Audit Trail Is Missing Records

Check:

- `Audit__WebhookUrl` is configured in producer services.
- Audit API is healthy.
- Producer service logs show successful audit publish or retry failure.
- Audit query uses the correct entity type and entity ID.

## Safe Logging Rules

Do not log passwords, secrets, connection strings, full personal identifiers, or sensitive financial data. Client-facing errors should be sanitized; server logs may contain diagnostic detail without leaking secrets.

## Azure Operations Direction

Future production runbooks should include alert response, Service Bus dead-letter handling, SQL backup restore, Key Vault secret rotation, Container Apps revision rollback, and dashboard links.
