# Security Architecture

## Current Security Posture

The MVP focuses on secure engineering foundations without implementing real user authentication. It includes input validation, sanitized error responses, environment-based configuration, correlation IDs for support, and a no-real-data rule for demos.

## Secrets Management

- No real secrets should be committed.
- Local Docker Compose includes a clearly local-only SQL Server developer password for repeatable demo execution.
- Azure deployments should store secrets in Azure Key Vault.
- Azure Container Apps should use managed identities to read Key Vault references or receive secret-backed environment variables.

## Authentication and Authorization Direction

Future identity direction:

- Azure Entra ID or Entra External ID.
- JWT validation at APIs or through Azure API Management.
- Role-based authorization for Customer, Loan Officer, and Admin roles.
- Policy names captured centrally in the `Security` building block.

## Data Protection

- Demo and seed data must be synthetic.
- Do not store unnecessary sensitive personal or financial information.
- Audit metadata should contain minimal operational context and must not include secrets, passwords, full identifiers, or sensitive financial payloads.

## Frontend Security

- The Angular application must not contain secrets.
- API base URLs are environment configuration, not credentials.
- User-facing error messages should be business-friendly and avoid internal stack traces.
- Correlation IDs should be available for support escalation.

## OWASP Posture

The architecture accounts for common web/API risks through validation, output safety, future authentication, authorization hooks, CORS control, secure headers direction, dependency scanning through CI, and no-secret repository hygiene.

## Known MVP Limitations

- Real authentication and authorization are deferred.
- Transport security is expected from local HTTPS profiles or Azure ingress in hosted environments.
- Production-grade rate limiting, WAF policies, and API gateway policies are future enhancements.
