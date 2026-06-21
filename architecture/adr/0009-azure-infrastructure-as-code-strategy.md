# ADR-0009: Azure Infrastructure as Code Strategy

## Status

Accepted

## Context

The target architecture is Azure-native and should be documented as deployable infrastructure. The repository needs a clear blueprint without forcing reviewers to provision resources.

## Decision

Use Bicep as the preferred Azure infrastructure-as-code language. Organize reusable modules under `infra/bicep/modules` and provide a development parameter file with secure parameters supplied at deployment time.

## Consequences

Bicep keeps the Azure deployment story close to the platform services and easy for Azure reviewers to understand. It supports what-if validation and modular resource ownership. The blueprint remains environment-aware and requires secure parameter handling before real deployment.

## Alternatives Considered

- Terraform: portable and popular, but Bicep is concise for Azure-native portfolio review.
- Manual portal deployment: easy for demos, but not architecture-grade or repeatable.
- ARM JSON templates: native, but less readable and less maintainable than Bicep.
