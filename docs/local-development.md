# Local Development Guide

## IDE Setup
- **Visual Studio 2022** or **JetBrains Rider** is recommended for backend `.NET 8` development.
- **VS Code** is recommended for Angular frontend development.

## Running the Platform
You can run the platform in two ways:
1. **Isolated Debugging**: Run individual services directly from your IDE. You only need the database. Start the DB with `docker compose up sqlserver -d`.
2. **Full Container Orchestration**: Run the entire microservice ecosystem as a suite of Docker containers. Run `docker compose --profile services --profile frontend up -d`.

For more details on the exact commands and testing flows, refer to the [DevOps Guide](devops-guide.md).
