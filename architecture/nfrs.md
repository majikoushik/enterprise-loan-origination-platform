# Non-Functional Requirements (NFRs)

## 1. Scalability
- **Compute**: Services are containerized and stateless, ready for horizontal scaling via Azure Container Apps (KEDA-driven scaling).
- **Database**: Azure SQL provides elastic pools and scalability.
- **Messaging**: Event-driven decoupling using Service Bus prevents bottlenecks during traffic spikes.

## 2. Observability & Monitoring
- **Structured Logging**: All services emit logs via Serilog, enriched with Correlation ID, Service Name, and Environment.
- **Distributed Tracing**: `X-Correlation-ID` traces transactions end-to-end across frontend, backend, and background workers.
- **Health Checks**: Standard ASP.NET Core Liveness and Readiness probes are configured for orchestration managers.
- **Production Sinks**: Ready for Azure Application Insights and Azure Log Analytics.

## 3. Reliability & Resilience
- **Exception Handling**: Global exception handling prevents crashes and ensures predictable API responses using RFC 7807 Problem Details.
- **Retry Mechanisms**: Prepared for Polly resilience strategies (circuit breaker, retries) when integrating real external services.

## 4. Security
- **Data Safety**: No stack traces or PII are exposed in HTTP responses or Audit metadata.
- **Configuration**: Sensitive configuration (like DB connection strings) are loaded via environment variables, ready for Azure Key Vault.

## 5. Maintainability
- **Architecture**: Domain-Driven Design and Clean Architecture principles ensure logical separation.
- **Code Quality**: Shared logic (like auditing and observability) is extracted into reusable NuGet-style building blocks.
