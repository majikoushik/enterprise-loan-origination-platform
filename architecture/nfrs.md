# Non-Functional Requirements

## Scalability

APIs should remain stateless and independently deployable. Azure Container Apps will provide horizontal scaling for APIs and workers.

## Availability

Health checks are required for APIs. Future deployment should use managed Azure services and separate app/database availability concerns.

## Performance

MVP APIs should keep request flows simple and avoid unnecessary network hops. Eligibility rules should be deterministic and fast.

## Security

The platform must avoid secrets in source control, validate inputs, avoid sensitive logging, and prepare for Azure Entra ID or Entra External ID authentication.

## Observability

Every request must support correlation ID propagation. Logs, metrics, traces, and health endpoints should support production troubleshooting.

## Maintainability

Service boundaries and domain language must remain clear. Shared building blocks should stay small and justified.

## Reliability

Future event handling should support retry and idempotency. Database migrations should be applied through controlled deployment flows.

## Compliance Readiness

The MVP is not a regulated banking system. It should still demonstrate auditability, data minimization, secure configuration, and traceability.

## Cost Awareness

Azure deployment should prefer right-sized managed services, consumption-based hosting where practical, and explicit cost notes in deployment documentation.
