# Security Architecture

## Current Foundation

Epic 0 establishes secure engineering expectations without introducing authentication. No secrets or real customer data are included.

## Secret Management

Local secrets should use environment variables or user secrets. Azure secrets should use Key Vault with managed identities.

## Authentication Direction

Future authentication should use Azure Entra ID or Entra External ID with JWT bearer validation in APIs.

## Authorization Direction

Future roles:

- Customer
- Loan Officer
- Admin

Role names are captured in the `Security` building block for future consistency.

## Data Protection

The platform should avoid storing unnecessary sensitive data. Demo data must be synthetic.

## Logging Controls

Logs must not include secrets, passwords, connection strings, full personal identifiers, or sensitive financial data.

## Frontend Security

The Angular app must not contain secrets. API errors displayed to users should be sanitized and correlation IDs should support support workflows.

## OWASP Posture

Future epics must consider input validation, output encoding, CORS configuration, secure headers, authentication, authorization, and dependency scanning.
