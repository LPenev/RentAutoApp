# 01 – Overview

Welcome to the **RentAutoApp** project documentation. This document provides a high-level overview of the application's purpose, architecture, and key modules.

---

## 🚗 What is RentAutoApp?

RentAutoApp is a modular ASP.NET Core MVC application designed for managing car rentals. It supports multiple users, roles, reservation flow, vehicle listings, search, and administrative features.

---

## 🧱 Solution Layout

```plaintext
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

---

## 🧩 Layers Overview

- **Web**: MVC controllers, views, Razor pages
- **ViewModels**: UI binding and validation objects
- **Infrastructure**: Middleware and MVC helpers
- **Services.Core**: Application logic, validation
- **Services.AutoMapping**: AutoMapper profiles
- **Services.Common**: Shared business utilities
- **Data**: EF Core context and repository coordination
- **Data.Models**: Entity definitions
- **Data.Common**: Abstractions like repositories
- **GCommon**: Shared enums, interfaces, constants
- **tests/**: Unit and integration test coverage
