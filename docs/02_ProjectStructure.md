# 02 – Project Structure

This file documents the directory structure of the RentAutoApp solution.

## 🔷 Root Layout

```
RentAutoApp/
├── src/
│   ├── RentAutoApp.Web/ 
│   ├── RentAutoApp.Web.ViewModels/
│   ├── RentAutoApp.Web.Infrastructure/
│   ├── RentAutoApp.Data/
│   ├── RentAutoApp.Data.Models/
│   ├── RentAutoApp.Data.Common/
│   ├── RentAutoApp.Services.Core/
│   ├── RentAutoApp.Services.AutoMapping/
│   ├── RentAutoApp.Services.Common/
│   └── RentAutoApp.GCommon/
├── tests/
│   ├── RentAutoApp.Services.Core.Tests/
│   ├── RentAutoApp.Web.Tests/
│   └── RentAutoApp.IntegrationTests/
├── docs/
├── README.md
├── RentAutoApp.sln
└── mkdocs.yml
```

## 🔹 Folder Roles

- `RentAutoApp.Web/`: MVC controllers, Razor views, HTTP pipeline, main entry point for web app.
- `RentAutoApp.Web.ViewModels/`: View models used for UI presentation and form binding.
- `RentAutoApp.Web.Infrastructure/`: Middleware, extensions, filters, and static helpers.
- `RentAutoApp.Data/`: EF Core context setup and data access coordination.
- `RentAutoApp.Data.Models/`: Entity Framework Core models (Code First).
- `RentAutoApp.Data.Common/`: Repository interfaces and base data access abstractions.
- `RentAutoApp.Services.Core/`: Core business logic and service interfaces.
- `RentAutoApp.Services.AutoMapping/`: AutoMapper profiles for DTO ↔ Entity mapping.
- `RentAutoApp.Services.Common/`: Shared business utilities, validators, constants.
- `RentAutoApp.GCommon/`: Global shared types (enums, constants, shared interfaces).
- `tests/`: Unit and integration test projects by concern.
- `docs/`: Markdown documentation and developer guides.
