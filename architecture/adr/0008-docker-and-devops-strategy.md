# ADR-0008: Docker and DevOps Strategy

## Status

Accepted

## Context

The platform should be easy to run locally and credible for cloud-native deployment. CI must validate backend, frontend, and container build quality without requiring Azure credentials for every pull request.

## Decision

Use Dockerfiles for each deployable service, Docker Compose for local orchestration, and GitHub Actions for backend build/test, Angular build/test, and Docker Compose build validation. Keep Azure deployment workflows as templates until environment-specific secrets are configured.

## Consequences

Developers get a repeatable local environment and reviewers see a clear deployment path. CI stays practical for public portfolio use. Production deployment still needs environment setup, secret configuration, and release governance.

## Alternatives Considered

- Only local CLI execution: simpler, but weaker Azure/container readiness.
- Full CD to Azure by default: impressive, but risky and costly for a portfolio repository.
- Kubernetes manifests: powerful, but Azure Container Apps better matches the target simplicity.
