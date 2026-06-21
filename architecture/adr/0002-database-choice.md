# ADR-0002: Database Choice

## Status

Accepted

## Context

The project targets a banking-style relational domain and must align with Azure deployment expectations.

## Decision

Use SQL Server for local development and Azure SQL Database for cloud deployment. EF Core migrations will be introduced when the first persisted feature is implemented.

## Consequences

The platform aligns with enterprise relational data practices and Azure managed database services. Developers need SQL Server locally through Docker or another local instance.

## Alternatives Considered

- PostgreSQL: strong relational option but less aligned with the stated Azure SQL direction.
- SQLite: convenient for demos but weaker for enterprise banking alignment.
- Cosmos DB: useful for some workloads but not the primary fit for loan origination transactional data.
