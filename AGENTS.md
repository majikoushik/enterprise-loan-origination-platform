# AGENTS.md

# Enterprise Loan Origination Platform — Agent Operating Guide

This file defines how AI coding agents must work in this repository.

The project is a portfolio-grade enterprise application intended to demonstrate Solution Architecture capability across:

- Banking domain architecture
- .NET backend engineering
- Angular frontend engineering
- Azure cloud-native deployment
- Microservices and API-driven architecture
- Event-driven workflow design
- Security, observability, testing, and DevOps maturity
- Architecture documentation expected from a senior Solution Architect

This is not a toy CRUD project. Every implementation should look like it was designed by a Solution Architect building an enterprise-grade banking platform.

---

# 1. Project Vision

Build a cloud-native Loan Origination Platform that demonstrates how a banking application can be designed, implemented, documented, tested, and deployed using modern enterprise engineering practices.

The application should support a simplified but realistic loan application journey:

1. Customer registration
2. Customer profile management
3. Loan application submission
4. Loan eligibility evaluation
5. Application status tracking
6. Notification event simulation
7. Audit logging
8. API documentation
9. Observability
10. Azure-ready deployment architecture

The application must be useful as a GitHub portfolio project for a Solution Architect profile.

---

# 2. Target Audience

This repository should be understandable and impressive to:

- Solution Architect recruiters
- Engineering managers
- Cloud architects
- Senior .NET interviewers
- Azure platform teams
- Technical leadership reviewers
- Enterprise modernization teams

The code, documentation, and diagrams should clearly demonstrate architecture thinking, not only coding ability.

---

# 3. Architecture Principles

Every change must follow these principles:

## 3.1 Architecture First

Before implementing a major feature, update or create the required architecture notes.

Expected artifacts may include:

- High-Level Design
- Low-Level Design
- API contracts
- Data model
- Event flow
- Sequence diagram
- NFR checklist
- ADRs
- Deployment notes

## 3.2 Enterprise-Grade Simplicity

Keep the MVP simple enough to run locally, but design it in a way that reflects enterprise thinking.

Do not over-engineer unnecessarily.

Good architecture means clear boundaries, maintainability, testability, and production readiness.

## 3.3 Domain-Oriented Design

Use banking domain language consistently.

Preferred terms:

- Customer
- Loan Application
- Eligibility Check
- Credit Decision
- Application Status
- Notification
- Audit Event
- Risk Rule
- Document Verification
- Underwriting Workflow

Avoid generic names such as `Data`, `Info`, `Manager`, `Helper`, or `CommonService` unless truly justified.

## 3.4 API-First Design

All backend APIs must be designed as enterprise APIs.

Each API should include:

- Clear route naming
- Request and response DTOs
- Validation rules
- Proper HTTP status codes
- Problem Details error responses
- Swagger/OpenAPI documentation
- Versioning readiness
- Consistent response patterns
- Correlation ID propagation

## 3.5 Azure-Ready by Design

Local development may use Docker Compose, but production direction must align with Azure.

Preferred Azure target architecture:

- Azure Static Web Apps or Azure Storage Static Website for Angular frontend
- Azure Container Apps for backend APIs and workers
- Azure SQL Database for relational data
- Azure Service Bus for asynchronous messaging
- Azure Key Vault for secrets
- Azure Container Registry for container images
- Azure Application Insights for telemetry
- Azure Log Analytics for centralized logs
- Azure API Management as optional future gateway
- Azure Front Door as optional future edge layer

## 3.6 Security by Default

Never commit secrets.

Never hardcode credentials.

Never expose sensitive data in logs.

Use environment-based configuration.

Use secure defaults.

Add security notes whenever implementing authentication, authorization, data access, logging, or external integrations.

## 3.7 Observability is Mandatory

Every service must be designed for production support.

Include:

- Structured logging
- Correlation ID
- Health checks
- Error logging
- Request logging
- Basic metrics readiness
- Distributed tracing readiness
- Application Insights integration pattern

## 3.8 Testable Architecture

Do not build code that cannot be tested.

Each meaningful backend feature should include:

- Unit tests for domain/application logic
- Integration tests for API/database behavior where practical
- Validation tests for request models
- Tests for failure scenarios

Each meaningful frontend feature should include:

- Component-level tests where practical
- Service tests for API integration wrappers
- Form validation tests where practical

---

# 4. Technology Stack

## 4.1 Backend

Use:

- ASP.NET Core Web API
- C#
- Entity Framework Core
- SQL Server for local and Azure SQL for cloud alignment
- FluentValidation or equivalent validation approach
- Swagger/OpenAPI
- Serilog or structured logging equivalent
- Health checks
- Docker support
- Clean Architecture or vertical-slice architecture where appropriate

Preferred backend structure:

- API layer
- Application layer
- Domain layer
- Infrastructure layer
- Shared building blocks only when truly reusable

Avoid putting business logic directly inside controllers.

## 4.2 Frontend

Use Angular as the frontend framework.

Frontend expectations:

- Angular with strict TypeScript
- Feature-based folder structure
- Standalone components or modern Angular architecture
- Reactive forms
- Typed API models
- Centralized API service layer
- HTTP interceptors for correlation ID and error handling
- Route-based navigation
- Clean UI with enterprise dashboard feel
- Responsive layout
- Clear form validation
- No business logic hidden inside templates

The frontend should look professional, modern, and suitable for a banking platform demo.

## 4.3 Database

Use SQL Server for local development unless a specific epic decides otherwise.

Database expectations:

- Entity Framework Core migrations
- Clear schema naming
- Audit fields where relevant
- Seed data for demo
- No real customer data
- No sensitive personal data
- Synthetic sample data only

## 4.4 Messaging

For MVP, messaging can be simulated or implemented locally.

Preferred design:

- Define an event abstraction first
- Use in-memory or local message simulation for MVP
- Keep production design compatible with Azure Service Bus

Example events:

- `LoanApplicationSubmitted`
- `EligibilityCheckCompleted`
- `LoanApplicationStatusChanged`
- `NotificationRequested`
- `AuditEventRecorded`

## 4.5 DevOps

Use GitHub Actions for CI.

Minimum pipeline expectations:

- Restore dependencies
- Build backend
- Run backend tests
- Build frontend
- Run frontend tests where practical
- Validate formatting or linting where practical

Future pipeline direction:

- Build Docker images
- Push to Azure Container Registry
- Deploy to Azure Container Apps
- Deploy Angular frontend to Azure Static Web Apps
- Run database migrations through controlled deployment flow

---

# 5. Recommended Repository Structure

Use this structure unless there is a strong reason to change it.

```text
enterprise-loan-origination-platform/
│
├── README.md
├── AGENTS.md
├── docker-compose.yml
├── .gitignore
├── .editorconfig
│
├── src/
│   ├── services/
│   │   ├── Customer.Api/
│   │   ├── LoanApplication.Api/
│   │   ├── Eligibility.Api/
│   │   ├── Notification.Worker/
│   │   └── Audit.Api/
│   │
│   ├── building-blocks/
│   │   ├── SharedKernel/
│   │   ├── Messaging/
│   │   ├── Observability/
│   │   └── Security/
│   │
│   └── web/
│       └── loan-portal-angular/
│
├── tests/
│   ├── Customer.Api.Tests/
│   ├── LoanApplication.Api.Tests/
│   ├── Eligibility.Api.Tests/
│   ├── Notification.Worker.Tests/
│   └── Audit.Api.Tests/
│
├── architecture/
│   ├── README.md
│   ├── hld.md
│   ├── lld.md
│   ├── nfrs.md
│   ├── api-governance.md
│   ├── security-architecture.md
│   ├── observability-architecture.md
│   ├── deployment-architecture.md
│   ├── data-model.md
│   ├── event-model.md
│   ├── diagrams/
│   │   ├── system-context.md
│   │   ├── container-diagram.md
│   │   ├── loan-application-sequence.md
│   │   └── azure-deployment.md
│   └── adr/
│       ├── 0001-architecture-style.md
│       ├── 0002-database-choice.md
│       ├── 0003-azure-hosting-strategy.md
│       └── 0004-observability-strategy.md
│
├── docs/
│   ├── setup.md
│   ├── local-development.md
│   ├── api-contracts.md
│   ├── testing-strategy.md
│   ├── deployment.md
│   ├── release-notes.md
│   └── roadmap.md
│
├── infra/
│   ├── bicep/
│   ├── terraform/
│   └── scripts/
│
└── .github/
    └── workflows/
        ├── ci.yml
        └── azure-deploy-template.yml
```

The agent may simplify the structure during the earliest MVP foundation, but it must preserve the long-term architectural direction.

---

# 6. MVP Scope

The MVP must include the following features.

## 6.1 Customer Registration

A customer should be able to register with basic profile details.

Sample fields:

- Full name
- Email
- Mobile number
- Date of birth
- Employment type
- Monthly income
- Existing monthly obligations

Do not use real customer data.

Use validation.

Use synthetic seed data.

## 6.2 Loan Application Submission

A customer should be able to submit a loan application.

Sample fields:

- Customer ID
- Loan type
- Requested amount
- Requested tenure in months
- Purpose
- Declared monthly income
- Existing EMI obligations

## 6.3 Eligibility Check

The system should evaluate basic eligibility.

Example rules:

- Minimum monthly income
- Maximum debt-to-income ratio
- Requested amount limit
- Tenure limit
- Existing obligations check

The rules should be simple but documented.

The implementation should be extensible.

Do not claim this is a real credit approval engine.

## 6.4 Application Status Tracking

Supported statuses:

- Draft
- Submitted
- UnderReview
- EligibilityPassed
- EligibilityFailed
- Approved
- Rejected
- Cancelled

Status transitions must be controlled.

Avoid random status updates without business rules.

## 6.5 Notification Event Simulation

When an application is submitted or eligibility is completed, create a notification request.

Notification channels for MVP:

- Email simulation
- SMS simulation

No real email or SMS provider is required in MVP.

## 6.6 Audit Logging

Important business actions must create audit records.

Examples:

- Customer registered
- Loan application submitted
- Eligibility check completed
- Status changed
- Notification requested

Audit logs should include:

- Event type
- Entity type
- Entity ID
- Timestamp
- Correlation ID
- Summary message

## 6.7 Angular Portal

The Angular frontend should support:

- Dashboard
- Customer registration form
- Loan application form
- Application status view
- Basic eligibility result display
- Clean navigation
- Responsive layout
- API error display
- Loading states

The UI should look professional and enterprise-ready.

---

# 7. Service Boundary Guidance

The final architecture may contain multiple services, but the MVP can be built incrementally.

Preferred service boundaries:

## Customer Service

Responsible for:

- Customer profile
- Customer contact details
- Employment and income summary

Not responsible for:

- Loan decisions
- Notifications
- Audit ownership beyond publishing events

## Loan Application Service

Responsible for:

- Loan application lifecycle
- Application status
- Loan application validation
- Status transition rules

Not responsible for:

- Customer master data ownership
- Notification delivery
- Final credit bureau integration

## Eligibility Service

Responsible for:

- Rule-based eligibility evaluation
- Eligibility result
- Decision explanation
- Risk rule versioning readiness

Not responsible for:

- Customer registration
- UI formatting
- Real credit bureau scoring

## Notification Worker

Responsible for:

- Consuming notification requests
- Simulating notification delivery
- Recording notification outcome

Not responsible for:

- Business decision-making

## Audit Service

Responsible for:

- Recording important business events
- Querying audit trail
- Supporting traceability

Not responsible for:

- Changing business state

---

# 8. Backend Coding Standards

## 8.1 Controllers

Controllers must be thin.

Controllers should:

- Accept requests
- Validate model state if applicable
- Call application services or handlers
- Return appropriate HTTP responses

Controllers should not:

- Contain business rules
- Directly perform database logic
- Construct complex domain behavior
- Hide exception handling locally

## 8.2 Application Layer

Application services or handlers should:

- Coordinate use cases
- Apply business rules
- Call repositories or DbContext
- Publish domain/integration events where required
- Return clear result objects

## 8.3 Domain Layer

Domain models should contain meaningful business behavior where appropriate.

Examples:

- Loan application status transition rules
- Eligibility decision result
- Debt-to-income calculation
- Loan amount validation

## 8.4 Infrastructure Layer

Infrastructure should contain:

- EF Core DbContext
- Database configurations
- External service adapters
- Message bus implementations
- Logging integrations
- Azure service adapters where applicable

## 8.5 Error Handling

Use a consistent error handling strategy.

Expected patterns:

- Global exception middleware
- Problem Details responses
- Validation error response format
- Correlation ID in error response
- No stack traces exposed to frontend
- Detailed error logs only in server logs

## 8.6 Validation

Validate all incoming requests.

Validation must cover:

- Required fields
- Format checks
- Numeric ranges
- Business rule boundaries
- Invalid status transitions
- Invalid customer/application references

## 8.7 Configuration

Use environment-based configuration.

Examples:

- `appsettings.json`
- `appsettings.Development.json`
- environment variables
- Azure Key Vault pattern for production

Never commit real secrets.

---

# 9. Angular Coding Standards

## 9.1 Structure

Use a feature-based Angular structure.

Preferred structure:

```text
loan-portal-angular/
│
├── src/
│   ├── app/
│   │   ├── core/
│   │   │   ├── interceptors/
│   │   │   ├── services/
│   │   │   └── models/
│   │   │
│   │   ├── shared/
│   │   │   ├── components/
│   │   │   ├── pipes/
│   │   │   └── validators/
│   │   │
│   │   ├── features/
│   │   │   ├── dashboard/
│   │   │   ├── customers/
│   │   │   ├── loan-applications/
│   │   │   └── eligibility/
│   │   │
│   │   └── layout/
│   │
│   └── environments/
```

## 9.2 UI Expectations

The UI should look like a professional fintech or banking dashboard.

Expected qualities:

- Clean layout
- Clear navigation
- Dashboard cards
- Status badges
- Form sections
- Responsive design
- Consistent spacing
- Error and success messages
- Loading indicators
- Empty-state messages

## 9.3 Forms

Use reactive forms.

Forms should include:

- Required validation
- Email validation
- Number range validation
- User-friendly error messages
- Disabled submit button when invalid
- Loading state during API call

## 9.4 API Integration

Use typed services for API calls.

Do not call HTTP APIs directly from components.

Use environment configuration for API base URL.

Use interceptors for:

- Correlation ID
- Error handling
- Future auth token handling

## 9.5 Frontend Security

Do not store secrets in frontend code.

Do not expose internal technical errors.

Sanitize or safely handle displayed data.

Prepare for future authentication and authorization.

---

# 10. API Design Standards

Use RESTful conventions.

Examples:

```text
POST   /api/v1/customers
GET    /api/v1/customers/{customerId}

POST   /api/v1/loan-applications
GET    /api/v1/loan-applications/{applicationId}
GET    /api/v1/loan-applications/customer/{customerId}
PATCH  /api/v1/loan-applications/{applicationId}/status

POST   /api/v1/eligibility/check
GET    /api/v1/eligibility/applications/{applicationId}

GET    /api/v1/audit/entity/{entityType}/{entityId}
```

Use consistent response models.

Example success response:

```json
{
  "data": {},
  "correlationId": "string",
  "timestamp": "2026-01-01T10:00:00Z"
}
```

Example error response should follow Problem Details style:

```json
{
  "type": "https://example.com/problems/validation-error",
  "title": "Validation failed",
  "status": 400,
  "detail": "One or more validation errors occurred.",
  "correlationId": "string",
  "errors": {}
}
```

---

# 11. Database Design Standards

Use clear table names.

Suggested tables:

- `Customers`
- `LoanApplications`
- `EligibilityResults`
- `NotificationRequests`
- `AuditEvents`

Each important table should include:

- Primary key
- Created timestamp
- Updated timestamp where relevant
- Status where relevant
- Row version or concurrency strategy where useful

Avoid storing unnecessary sensitive data.

Use synthetic sample values.

---

# 12. Event Design Standards

Events should represent business facts that already happened.

Good event names:

- `CustomerRegistered`
- `LoanApplicationSubmitted`
- `EligibilityCheckCompleted`
- `LoanApplicationStatusChanged`
- `NotificationRequested`
- `AuditEventRecorded`

Avoid command-like event names such as:

- `SubmitLoanApplicationEvent`
- `DoEligibilityCheck`
- `SendNotificationNow`

Event payloads should include:

- Event ID
- Event type
- Occurred timestamp
- Correlation ID
- Entity ID
- Minimal business data required by consumers

---

# 13. Observability Standards

Every backend service should include:

- Correlation ID middleware
- Structured logging
- Request logging
- Error logging
- Health check endpoint
- Application Insights readiness
- Log fields useful for troubleshooting

Minimum log fields:

- Timestamp
- Level
- Service name
- Correlation ID
- Event name
- Entity type
- Entity ID where relevant
- Message
- Exception details where applicable

Do not log:

- Passwords
- Secrets
- Full personal identifiers
- Sensitive financial data
- Connection strings

---

# 14. Security Standards

Security must be considered in every epic.

Minimum expectations:

- Input validation
- Output safety
- Secure configuration
- No secrets in repository
- No real personal data
- No sensitive data in logs
- CORS configured intentionally
- Future authentication and authorization hooks
- Secure headers where practical
- OWASP awareness in API and frontend implementation

Future authentication direction:

- Azure Entra ID or Entra External ID
- JWT-based API authorization
- Role-based access control
- Customer, Loan Officer, Admin roles

For MVP, authentication may be simulated or deferred, but architecture notes must explain the future direction.

---

# 15. Azure Deployment Direction

All deployment work should align with this target architecture.

## 15.1 Frontend

Preferred Azure options:

- Azure Static Web Apps
- Azure Storage Static Website with CDN
- Azure Front Door optional for future

## 15.2 Backend APIs and Workers

Preferred Azure options:

- Azure Container Apps for APIs and workers
- Azure Container Registry for images
- Azure API Management as optional API gateway

## 15.3 Database

Preferred:

- Azure SQL Database

## 15.4 Messaging

Preferred:

- Azure Service Bus

## 15.5 Secrets

Preferred:

- Azure Key Vault
- Managed Identity

## 15.6 Monitoring

Preferred:

- Azure Application Insights
- Azure Log Analytics

## 15.7 Infrastructure as Code

Use Bicep as the preferred Azure IaC option unless another epic explicitly selects Terraform.

Infrastructure files should be placed under:

```text
infra/bicep/
```

Include documentation explaining:

- Required Azure resources
- Deployment order
- Environment variables
- Secret configuration
- Cost-awareness notes

---

# 16. Documentation Requirements

Documentation is part of the product.

Every major epic must update documentation.

Minimum documentation files:

## README.md

Should include:

- Project overview
- Architecture summary
- Tech stack
- Features
- How to run locally
- How to run tests
- API documentation link
- Azure deployment direction
- Screenshots when UI exists

## architecture/hld.md

Should include:

- Business context
- System context
- Container view
- Major components
- Service responsibilities
- Integration points
- Key quality attributes

## architecture/lld.md

Should include:

- Service internals
- Main classes/modules
- API flow
- Database interaction
- Validation flow
- Error handling
- Event handling

## architecture/nfrs.md

Should include:

- Scalability
- Availability
- Performance
- Security
- Observability
- Maintainability
- Reliability
- Compliance readiness
- Cost awareness

## architecture/adr/

Every major architecture choice should have an ADR.

ADR format:

```markdown
# ADR-000X: Decision Title

## Status

Accepted

## Context

Why this decision is needed.

## Decision

What decision was made.

## Consequences

Positive and negative consequences.

## Alternatives Considered

Other options considered and why they were not selected.
```

## docs/roadmap.md

Should show:

- Completed MVP features
- In-progress features
- Future enhancements
- Azure deployment roadmap
- Security roadmap
- Observability roadmap

---

# 17. Testing Standards

Testing must be included from the beginning.

## Backend Testing

Use appropriate .NET testing tools.

Expected test types:

- Unit tests
- Application service tests
- Domain rule tests
- API integration tests where practical
- Validation tests

Important scenarios:

- Customer registration validation
- Loan application submission
- Invalid loan amount
- Invalid tenure
- Eligibility passed
- Eligibility failed
- Invalid status transition
- Audit event creation
- Notification request creation

## Frontend Testing

Use Angular testing tools.

Expected test types:

- Component tests where practical
- Service tests
- Form validation tests
- Routing smoke tests where practical

## Test Data

Use synthetic data only.

No real customer information.

No real financial data.

---

# 18. CI/CD Standards

GitHub Actions should be added early.

Minimum CI workflow:

```text
on:
  pull_request
  push to main
```

Pipeline should:

- Restore backend dependencies
- Build backend
- Run backend tests
- Install frontend dependencies
- Build Angular frontend
- Run frontend tests where practical

Future CD workflow should:

- Build Docker images
- Push images to Azure Container Registry
- Deploy backend to Azure Container Apps
- Deploy frontend to Azure Static Web Apps
- Apply database migrations safely

---

# 19. Agent Workflow Rules

When receiving an epic-based prompt, the agent must follow this workflow:

## Step 1: Understand the Epic

Read:

- Existing code
- README
- AGENTS.md
- Relevant architecture docs
- Existing tests
- Existing workflows

Do not assume the repository is empty.

## Step 2: Create an Implementation Plan

Before coding, identify:

- Files to create
- Files to modify
- Architecture impact
- Testing impact
- Documentation impact

## Step 3: Implement Incrementally

Make small, coherent changes.

Avoid massive unstructured changes.

Keep code buildable.

Do not leave half-implemented features.

## Step 4: Add or Update Tests

Every meaningful feature must include tests.

If tests cannot be added, explain why in documentation or final summary.

## Step 5: Update Documentation

Update relevant docs after implementation.

At minimum update:

- README.md when user-facing behavior changes
- architecture docs when architecture changes
- docs/roadmap.md after completing an epic

## Step 6: Validate

Before finishing, run or provide the commands to run:

- Backend build
- Backend tests
- Frontend build
- Frontend tests
- Docker Compose validation where applicable

## Step 7: Summarize

Final response should include:

- What was implemented
- Key files changed
- How to run
- How to test
- Architecture notes
- Next recommended epic

---

# 20. Agent Response Format

For every implementation epic, the agent should respond using this format:

```markdown
## Completed

Summary of what was implemented.

## Architecture Notes

Important architecture decisions or changes.

## Files Changed

List of important files added or modified.

## How to Run

Commands to run the application.

## How to Test

Commands to run tests.

## Documentation Updated

Docs that were created or updated.

## Suggested Next Epic

Recommended next step.
```

---

# 21. MVP Epic Roadmap

The user may give short epic prompts. The agent must infer technical tasks from this roadmap.

## Epic 0: Repository Foundation

Goal:

Create the initial enterprise-grade repository foundation.

Expected output:

- Solution structure
- Backend skeleton
- Angular skeleton
- Docker Compose skeleton
- README
- AGENTS.md
- Architecture docs
- ADRs
- Initial CI workflow

## Epic 1: Customer Registration

Goal:

Build customer registration backend and frontend flow.

Expected output:

- Customer API
- Customer entity
- Database migration
- Validation
- Angular customer form
- API integration
- Tests
- Documentation update

## Epic 2: Loan Application Submission

Goal:

Allow customers to submit loan applications.

Expected output:

- Loan application API
- Loan application entity
- Status model
- Validation
- Angular loan form
- Tests
- API documentation
- Audit event creation

## Epic 3: Eligibility Evaluation

Goal:

Add rule-based loan eligibility evaluation.

Expected output:

- Eligibility service/API
- Rule engine structure
- Eligibility result entity
- Decision explanation
- Angular eligibility result view
- Tests
- Architecture documentation

## Epic 4: Status Tracking

Goal:

Allow tracking of loan application status.

Expected output:

- Status transition logic
- Status query API
- Angular status dashboard
- Status badges
- Invalid transition handling
- Tests
- Documentation

## Epic 5: Notification Simulation

Goal:

Create event-driven notification request simulation.

Expected output:

- Notification worker or service
- Notification event model
- Simulated email/SMS result
- Audit trail integration
- Tests
- Documentation

## Epic 6: Audit Logging

Goal:

Add centralized audit logging for important business events.

Expected output:

- Audit API/service
- Audit event model
- Audit query endpoint
- Correlation ID support
- Angular audit trail view
- Tests
- Documentation

## Epic 7: Observability and Production Readiness

Goal:

Add production-readiness patterns.

Expected output:

- Structured logging
- Correlation ID
- Health checks
- Error handling middleware
- Application Insights readiness
- NFR documentation
- Incident support notes

## Epic 8: DevOps and Docker

Goal:

Make the project easy to build and run.

Expected output:

- Dockerfiles
- Docker Compose
- GitHub Actions CI
- Build validation
- Test validation
- Developer setup documentation

## Epic 9: Azure Deployment Blueprint

Goal:

Prepare Azure deployment architecture.

Expected output:

- Bicep templates
- Azure resource plan
- Deployment documentation
- Environment variable documentation
- Key Vault pattern
- Application Insights pattern
- Azure Container Apps deployment direction

## Epic 10: Portfolio Polish

Goal:

Make the repository impressive for GitHub visitors.

Expected output:

- Professional README
- Architecture diagrams
- Screenshots
- Roadmap
- Demo data
- Clear setup instructions
- Clean badges
- Final documentation review

---

# 22. Default Behavior for Short User Prompts

The user may provide prompts like:

```text
Implement Epic 1.
```

or:

```text
Add eligibility feature.
```

or:

```text
Make this Azure ready.
```

When prompts are short, the agent must not ask for unnecessary technical clarification.

The agent should use this AGENTS.md file as the source of technical direction.

The agent should proceed with best-practice implementation based on:

- Current repository state
- Project roadmap
- Enterprise architecture expectations
- Azure deployment direction
- Existing code and documentation

Ask clarification only when a decision would significantly change architecture, cost, security, or user-facing behavior.

---

# 23. Quality Bar

A feature is not complete unless:

- Code builds successfully
- Relevant tests are added or updated
- API behavior is documented
- Frontend behavior is usable where applicable
- Architecture docs are updated if needed
- README or setup docs are updated if commands changed
- No secrets are committed
- No real personal data is used
- Error handling is considered
- Logging is considered
- Validation is implemented
- The implementation supports future Azure deployment

---

# 24. Definition of Done

For every epic, the Definition of Done is:

```text
1. Feature implemented end-to-end or clearly scoped for the epic.
2. Backend code follows clean architecture or clear service boundaries.
3. Angular code follows feature-based structure.
4. API contracts are documented through Swagger/OpenAPI.
5. Validation and error handling are implemented.
6. Tests are added or updated.
7. Logging and correlation ID impact are considered.
8. Documentation is updated.
9. Local run instructions are clear.
10. Azure-readiness is preserved.
11. No secrets, credentials, or real customer data are committed.
12. Final response explains what changed and how to validate it.
```

---

# 25. Non-Negotiable Rules

The agent must not:

- Build a toy-style project
- Put all backend logic in controllers
- Put all frontend logic in components
- Skip validation
- Ignore tests
- Ignore documentation
- Hardcode secrets
- Use real personal data
- Create unclear folder structures
- Mix unrelated responsibilities
- Add unnecessary complexity without explanation
- Break existing working functionality
- Remove architecture documents without replacement
- Introduce cloud resources without documenting cost and purpose

---

# 26. Preferred Naming Conventions

## Backend

Use clear names:

- `Customer`
- `LoanApplication`
- `EligibilityResult`
- `NotificationRequest`
- `AuditEvent`
- `ApplicationStatus`
- `LoanPurpose`
- `EmploymentType`

Avoid vague names:

- `Data`
- `Info`
- `Helper`
- `Manager`
- `Processor`
- `Common`
- `Utility`

## Frontend

Use clear feature naming:

- `customer-registration`
- `loan-application-form`
- `application-status`
- `eligibility-result`
- `audit-trail`
- `dashboard`

---

# 27. Sample Domain Rules for MVP

Eligibility logic may start with simple rules:

```text
Minimum monthly income: 25,000
Maximum debt-to-income ratio: 50%
Maximum requested amount: 20 times monthly income
Minimum tenure: 6 months
Maximum tenure: 84 months
```

The result should include:

- Decision: Passed or Failed
- Rule results
- Explanation
- Evaluated timestamp
- Rule version

This must be documented as a demo rule engine, not a real banking credit decision system.

---

# 28. Portfolio Presentation Expectations

This repository should visually communicate seniority.

README should eventually include:

- Project banner or title
- Architecture diagram
- Feature list
- Tech stack badges
- Local setup
- Screenshots
- API documentation
- Azure deployment architecture
- CI/CD status badge
- Roadmap
- Architecture decision records
- Observability notes
- Security notes

The repository should make the visitor think:

```text
This person understands enterprise architecture, not only coding.
```

---

# 29. Final Guidance for AI Agents

When implementing, always optimize for:

- Clear architecture
- Professional repository presentation
- Maintainability
- Enterprise realism
- Azure readiness
- Strong documentation
- Clean code
- Testability
- Recruiter/interviewer impact

This project should become a flagship GitHub portfolio project for a Solution Architect specializing in .NET, Azure, microservices, enterprise modernization, and AI-enabled software engineering.
