# Roadmap for the Nimbrix Framework

This document outlines the planned development path for the Nimbrix framework. The roadmap is a living document and serves as guidance for contributions and prioritization. It builds on the currently available modules and extends them step by step.

## Short‑term goals (coming months)

1. Expand documentation:
   * Create and maintain complete contents for the files architecture.md, naming-conventions.md and package-map.md (these goals are currently being implemented). Good documentation is important to onboard new team members quickly.

2. Implement logging:
   * The Nimbrix.Logging project should provide a pluggable logging layer (e.g. adapters for Serilog or Microsoft.Extensions.Logging).
   * Implement standardized logging of commands, queries and exceptions.

3. Build persistence layer:
   * In the Nimbrix.Persistence project implement a generic repository layer that supports various databases (e.g. SQLite or PostgreSQL). Optional integration of ORMs such as Entity Framework.

4. Extend Angular‑CQRS library:
   * In nimbrix-angular-cqrs implement services that send HTTP requests to the .NET back end and return typed results.
   * Integrate state management (e.g. Akita or NgRx) for local management of query results and status.

## Mid‑term goals (6–12 months)

1. Develop UI component library:
   * The nimbrix-angular-ui package should contain a collection of reusable UI elements (buttons, tables, form fields).
   * The components should be responsive and provide a consistent design. Follow the Angular Style Guide for structure and naming.

2. Build test framework:
   * In the Nimbrix.Testing project provide a base layer for unit and integration tests. Examples include tests for ServiceResult and the CqrsBaseController.
   * On the Angular side develop integration tests for the libraries in nimbrix-angular-testing.

3. Expand sample applications:
   * Expand the existing Samples (Weather Forecast) into a small business domain such as a to‑do or product management system to demonstrate typical CRUD operations.
   * New examples should cover both back‑end functionality (CQRS, logging, persistence) and front‑end components.

## Long‑term goals (12 months and later)

1. Microservices architecture:
   * Evaluate whether the CQRS framework can be extended for a microservices structure (separate services for different bounded contexts).
   * Provide support for event sourcing and messaging (e.g. via RabbitMQ).

2. Cloud deployment and DevOps:
   * Containerize the applications with Docker and deploy via Kubernetes or Azure Container Apps. Automate CI/CD pipelines to publish NuGet and npm packages.

3. Enhanced documentation and community:
   * Develop a developer manual with tutorials and code examples.
   * Build a community and collect contributions through open‑source participation.

This roadmap is intended to make the development of the Nimbrix framework transparent and to pursue a shared vision. It can be adjusted at any time if there are changes or new requirements.