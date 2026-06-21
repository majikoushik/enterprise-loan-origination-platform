# Observability Architecture

## Overview
The platform uses a standardized approach to observability, emphasizing context-rich structured logging, tracing, and active monitoring to ensure production-readiness on Azure.

## Core Pillars

### 1. Structured Logging (Serilog)
All logs are captured as structured events.
- **Enrichment**: Every log entry is enriched with `ServiceName`, `Environment`, and `CorrelationId`.
- **Sinks**: 
  - Local: Console
  - Production: Azure Application Insights

### 2. Distributed Tracing (Correlation IDs)
We use `X-Correlation-ID` across HTTP requests and message buses.
- **Origin**: Generated at the frontend by `correlation-id.interceptor.ts` (or at the API edge if missing).
- **Propagation**: Transferred automatically via `CorrelationIdMiddleware` and injected into the Serilog `LogContext`. Included in `ProblemDetails` error responses and `AuditEventRecord` payloads.

### 3. Health Checks
Standardized health check patterns are enforced:
- **Liveness** (`/health/live`): Confirms process is running. Used by Azure Container Apps Liveness Probe.
- **Readiness** (`/health/ready`): Confirms database connections. Used by Azure Container Apps Readiness Probe.

### 4. Global Exception Handling
Errors are caught globally using .NET 8's `IExceptionHandler`.
- Maps generic errors to HTTP 500.
- Maps business/validation errors to HTTP 400.
- Formats responses using `ProblemDetails` RFC 7807 specification.
- Excludes stack traces from HTTP responses to prevent security leaks, but preserves them in Application Insights.

## Azure Monitor Readiness
The system is ready to be hosted in Azure Container Apps with native integration into:
- **Azure Application Insights**: Telemetry, live metrics, and dependency tracking.
- **Azure Log Analytics**: Long-term log retention and KQL querying.
