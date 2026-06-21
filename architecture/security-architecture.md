# Security Architecture

## Current Foundation

Epic 0 establishes secure engineering expectations without introducing authentication. No secrets or real customer data are included.

### 4. Secrets Management (Azure Key Vault)
- No secrets (such as SQL credentials or API tokens) are hardcoded or committed to source control.
- In Azure, all secrets are securely stored in **Azure Key Vault**.
- Access to Key Vault is governed by **System-Assigned Managed Identities** applied directly to the Azure Container Apps pods. Container Apps read configurations seamlessly at boot.

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
