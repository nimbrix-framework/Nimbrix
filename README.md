# Nimbrix
Nimbrix is an open-source, modular framework that simplifies the development of business applications by combining a robust CQRS-based back-end with a modular Angular front-end. It encourages domain-driven design, separation of concerns and consistent naming conventions across the entire stack.
## Overview
Nimbrix aims to streamline enterprise application development by providing building blocks for a multi-tiered architecture:
* **Back-end**: Built with .NET (ASP.NET Core) and implements the **Command Query Responsibility Segregation (CQRS)** pattern. It provides base interfaces (`ICommand`, `IQuery`) and pipeline behaviors for cross-cutting concerns such as logging, validation and exception handling. The `ServiceResult` type standardizes API responses and error handling.
* **Front-end**: An Angular workspace with separate libraries for core utilities, CQRS integration, UI components and testing. It follows the Angular style guide for naming and structure and includes CLI commands to serve and build libraries

The framework is designed to be extensible—developers can plug in logging and persistence providers, create custom UI modules and enforce a consistent code style across the project.

## Features
* **CQRS and mediator pattern**: Abstracts commands and queries, dispatches them through a dispatcher with pipeline behaviors for logging and exception handling
* **ASP.NET Core API base controller**: Provides a `CqrsBaseController` with helper methods such as `OkResponse`, `BadRequestResponse` and `HandleRequestAsync` for simplified API endpoints.
* **Standardized service results**: The `ServiceResult` type captures success status, error messages and HTTP status codes, promoting uniform error handling.
* **Modular Angular workspace**: Separate libraries for core services, client-side CQRS integration, UI components and testing. Each library can be built and published independently using Angular CLI commands.
* **Project documentation**: Comprehensive docs on architecture, naming conventions, package mapping and roadmap under the `docs/` directory.

## Project Structure

The repository is organized into back-end and front-end folders with clear separation of concerns. Key directories include:
| Path/Project               | Description                                                                                                                        |
| -------------------------- | ---------------------------------------------------------------------------------------------------------------------------------- |
| `src/Nimbrix.CQRS/`        | Interfaces and abstractions for commands, queries and handlers.  Also contains pipeline behaviors and the `ServiceResult` utility. |
| `src/Nimbrix.AspDotNet/`   | ASP.NET Core integration, including the `CqrsBaseController` for API endpoints.                                                    |
| `src/Nimbrix.Logging/`     | Placeholder for the logging infrastructure.                                                                                        |
| `src/Nimbrix.Persistence/` | Placeholder for database and persistence layers.                                                                                   |
| `src/Samples/`             | Sample ASP.NET Core application demonstrating basic usage.                                                                         |
| `src/Nimbrix.Angular/`     | Angular workspace containing multiple libraries:                                                                                   |
| `nimbrix-angular-core/`    | Core Angular utilities and shared services; includes instructions for building and publishing.                                     |
| `nimbrix-angular-cqrs/`    | Client-side CQRS integration with dispatchers and handlers (planned).                                                              |
| `nimbrix-angular-ui/`      | UI component library (planned).                                                                                                    |
| `nimbrix-angular-testing/` | Testing utilities for Angular apps.                                                                                                |

For a full description of each package, see `docs/package-map.md`.

## Getting Started
**Prerequisites**
* Back-end: .NET SDK 7.0 or higher.
* Front-end: Node.js 18+ and the Angular CLI (`npm install -g @angular/cli`).

**Clone the Repository**
```Bash
git clone https://github.com/nimbrix-framework/Nimbrix.git
cd Nimbrix
git checkout develop
```
**Run the Back-end Sample**
1. Navigate to the sample project:
```Bash
cd src/Samples/SampleApi
```
2. Restore dependencies and run the API:
```Bash
dotnet restore
dotnet run
```
3. The API will be available at https://localhost:5001 (default). Use tools like curl or Postman to test endpoints.

**Serve the Angular Workspace**
To start the development server for the Angular demo, run:
```Bash
cd src/Nimbrix.Angular
ng serve
```
This command compiles the application and starts a development server. Navigate to `http://localhost:4200/` in your browser; the app will automatically reload if you change any of the source files.

**Build the Angular Libraries**
Each Angular library can be built individually. For example, to build the core library:
```Bash
ng build nimbrix-angular-core
```
The build artifacts will be stored in the `dist/` directory. You can also publish the library to an npm registry with `npm publish`.

## Documentation
Detailed documentation can be found in the `docs/` folder:

* `docs/architecture.md` – explains the overall architecture and design principles.
* `docs/naming-conventions.md` – outlines naming conventions for C# and Angular projects.
* `docs/package-map.md` – provides a map of all packages and their responsibilities.
* `docs/roadmap.md` – lists future enhancements and milestones.

## Contributing
Contributions are welcome! Please follow these guidelines:

1. Discuss your idea: Create an issue or comment on existing ones before starting work.
2. Follow naming conventions: [Use PascalCase for C# classes and camelCase for fields](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/identifier-names#:~:text=By%20convention%2C%20C,NET%20Runtime%20team%27s%20coding%20style) follow the Angular file naming conventions (feature.type.ts with dash-case).
3. Write tests: Add appropriate unit and integration tests for any new functionality.
4. Create a pull request: Target the develop branch, provide a clear description and reference relevant issues.

## License
This project is licensed under the Apache License 2.0. See the [LICENSE](https://github.com/nimbrix-framework/Nimbrix/blob/develop/LICENSE#L66-L71) file for details.

## Acknowledgements

The Nimbrix framework is inspired by established architectural patterns, the .NET ecosystem and the Angular community. Special thanks to all contributors and the open-source community.