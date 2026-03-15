# Architecture of the Nimbrix Framework
## Overview

The Nimbrix framework is designed as a modular toolkit. It consists of a back end based on .NET 8 that implements the CQRS pattern and a front end built on Angular. All building blocks are intended to be loosely coupled and to follow the Single Responsibility Principle—each component has a clearly defined task. By strictly separating query/command logic from the presentation layer, the system can scale independently and serve different clients. [Style Guide - Angular](https://v19.angular.dev/style-guide#:~:text=Apply%20the%20single%20responsibility%20principle,and%20maintain%2C%20and%20more%20testable)

## Back End: CQRS & .NET

The back‑end part of the framework comprises several NuGet packages:

* **Nimbrix.CQRS** – Implements the Command‑Query‑Responsibility‑Segregation pattern. The library exposes interfaces (`ICommand`, `IQuery`, `ICommandHandler`, `IQueryHandler`) and a dispatcher. A `ServiceResult` standardizes the outcome of an operation; it stores a success flag, a list of errors and an HTTP status code [ServiceResult.cs](https://github.com/nimbrix-framework/Nimbrix/blob/develop/src/Nimbrix/Nimbrix.CQRS/Common/ServiceResult.cs#L5-L71).
* **Nimbrix.AspDotNet** – Integrates the CQRS library into ASP.NET Core. The base class `CqrsBaseController` encapsulates common response formats such as `OkResponse`, `BadRequestResponse` and `CreatedResponse`. A generic `HandleRequestAsync` method processes CQRS requests and evaluates security attributes (authentication/roles). [CqrsBaseController.cs](https://github.com/nimbrix-framework/Nimbrix/blob/develop/src/Nimbrix/Nimbrix.AspDotNet/Controller/CqrsBaseController.cs#L31-L47)
* **Nimbrix.Logging and Nimbrix.Persistence** – Prepare infrastructure for unified logging and a data access layer. These packages currently contain no logic and serve as placeholders for future implementations of logging strategies and data access adapters.
* **Nimbrix.Samples** – Contains simple sample applications such as a WeatherForecastController to demonstrate how the libraries interact.

The .NET projects are located in `src/Nimbrix/`. Each project has its own `.csproj` file and is included in the `Nimbrix.sln` solution. The Nimbrix CQRS implementation makes it easier to strictly separate commands and queries and to register pipeline behaviors (e.g., logging or validation). Service results are standardized via **ServiceResult<T>**.

## Front End: Angular packages

Under `src/Nimbrix.Angular` there is an Angular workspace generated with the Angular CLI. The workspace consists of several libraries:

* **nimbrix‑angular‑core** – Base package for reusable components, directives and services. The library is built and published via `ng build nimbrix-angular-core` and `npm publish`.
* **nimbrix‑angular‑cqrs** – Intended to simplify communication with the CQRS back end. This library is still just a scaffold; only standard files are present.
* **nimbrix‑angular‑testing** – Contains helpers for unit and end‑to‑end tests.
* **nimbrix‑angular‑ui** – Package for UI components. Like the other packages it currently consists of the Angular standard scaffolding and awaits implementation.

The Angular libraries follow the conventions of the Angular Style Guide. File names consist of the feature name and the type (`feature.type.ts`), with words separated by dashes and the type separated by a dot [Style Guide - Angular](https://v19.angular.dev/style-guide#:~:text=Apply%20the%20single%20responsibility%20principle,and%20maintain%2C%20and%20more%20testable).

## Modularity and Single Responsibility

The framework emphasizes clear modularity:
* Separation of responsibilities: Each library encapsulates a specific domain (CQRS, logging, UI, etc.). This structure facilitates reuse and encourages independent evolution.
* One artifact per file: The Angular Style Guide recommends defining only one component, service or directive per file. This improves readability and prevents name clashes.
* Consistent build and publication processes: The Angular packages include ng-package.json files and standard build scripts that ensure libraries are built and published consistently.

With this architecture, Nimbrix provides a solid foundation for scalable business applications, with a clear separation between front end and back end and the ability to add additional services or UIs.