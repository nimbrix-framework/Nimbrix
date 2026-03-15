# Naming Conventions in the Nimbrix Project

This document defines naming conventions for the Nimbrix framework. It covers both .NET and Angular projects so that code is consistently named and easy to understand. Uniform names make it easier to navigate the repository and reduce onboarding and maintenance effort.

## .NET/C# conventions

The back‑end components use C# and follow the guidelines from [Microsoft Learn](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/identifier-names#:~:text=By%20convention%2C%20C,NET%20Runtime%20team%27s%20coding%20style):

* Classes, structures, methods and properties are written in PascalCase (each word starts with an uppercase letter and no underscores). Examples: `WeatherForecastController` or `HandleRequestAsync`.
* Variables and parameters use camelCase, i.e. the first word is lowercase and each additional word starts with an uppercase letter. Example: weatherForecastService.
* Interfaces start with the prefix `I` followed by **PascalCase**, e.g. `IQueryHandler<TQuery,TResult>`.
* Asynchronous methods end with the suffix `Async`, for example `HandleRequestAsync` in the [CqrsBaseController](https://github.com/nimbrix-framework/Nimbrix/blob/develop/src/Nimbrix/Nimbrix.AspDotNet/Controller/CqrsBaseController.cs#L70-L114).
* Avoid abbreviations and unclear acronyms. Names should be meaningful and self‑explanatory.
* Enumerations use singular names (e.g. `NotificationFailureMode` instead of `NotificationFailureModes`) and enumeration values in **PascalCase**.
* Generic types use short type parameters like `TQuery` or `TResult`. For ambiguous generic classes (e.g. `ServiceResult<T>`) provide a clear type parameter that reflects what the result represents.

## Angular conventions

For the front‑end libraries, Nimbrix follows the official Angular Style Guide. Key rules include:

* **File names** consist of the descriptive feature name and a type designation, separated by a dot. Words are separated by dashes (dash‑case). Examples: `user-profile.component.ts`, `service-result.service.ts`.
* **Folder names** reflect the feature; several components for one feature are placed in a common folder.
* **Components, directives, pipes** and **services** are defined in **PascalCase** within the code (`UserProfileComponent`), but file names follow the dash‑case convention.
* **Tests** have the `.spec.ts` suffix, e.g. `user-profile.component.spec.ts`.
* **One artifact per file**: Each component, pipe or directive has its own file. This principle (Single Responsibility/Rule of One) improves readability and maintenance.

## General recommendations

* **Consistency** is more important than the exact style. Stick to the patterns described here so that all files are similarly structured.
* **Library prefixes**: Angular libraries should use a common prefix (e.g. `nimbrix`) to avoid collisions with other libraries. The same applies to namespaces in C# (all projects start with `Nimbrix.`).
* **Intuitive names**: Avoid unclear abbreviations. Names should reflect the function (e.g. `ServiceResult` for a service response).
* **Language‑neutral naming**: Use English terms for code elements to facilitate international collaboration.

These conventions ensure that all components of the Nimbrix framework are well structured and fit seamlessly into the .NET and Angular ecosystem.