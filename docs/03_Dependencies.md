# 03 – Project Dependencies

This document outlines the dependencies between the core layers and test projects in the RentAutoApp solution.

---

## 🧱 Dependency Table

| Project                             | Depends On                                                                 |
|-------------------------------------|------------------------------------------------------------------------------|
| `RentAutoApp.Web`                   | `RentAutoApp.Services.Core`, `RentAutoApp.Web.ViewModels`, `RentAutoApp.Web.Infrastructure`, `RentAutoApp.GCommon` |
| `RentAutoApp.Web.ViewModels`        | `RentAutoApp.Web.ViewModels` |
| `RentAutoApp.Web.Infrastructure`    | `RentAutoApp.GCommon` |
| `RentAutoApp.Services.Core`         | `RentAutoApp.Data`, `RentAutoApp.GCommon`, `RentAutoApp.Services.Common`, `RentAutoApp.Services.AutoMapping` |
| `RentAutoApp.Services.AutoMapping`  | `RentAutoApp.Data.Models`, `RentAutoApp.Web.ViewModels`       |
| `RentAutoApp.Services.Common`       |  |
| `RentAutoApp.Data`                  | `RentAutoApp.Data.Models`, `RentAutoApp.Data.Common`, `Microsoft.EntityFrameworkCore` |
| `RentAutoApp.Data.Models`           | `RentAutoApp.GCommon` |
| `RentAutoApp.GCommon`               | *(Standalone: shared types, enums, constants)*                              |
| `RentAutoApp.Services.Core.Tests`   | `RentAutoApp.Services.Core`, `RentAutoApp.GCommon`  |
| `RentAutoApp.Web.Tests`             | `RentAutoApp.Web`, `RentAutoApp.Services.Core` |
| `RentAutoApp.IntegrationTests`      | `RentAutoApp.Web`, `RentAutoApp.Services.Core` |

---

## 🧪 Test Project Details

### `RentAutoApp.Services.Core.Tests/`
- Unit tests for business logic and services.
- Uses **NUnit**
- Mocks dependencies using `Moq`
- Uses `FluentAssertions` for better test readability

### `RentAutoApp.Web.Tests/`
- Tests controllers, routing, and validation logic.
- Uses **NUnit** and mocks `Services.Core` interfaces
- Focused on MVC layer and view model validation

### `RentAutoApp.IntegrationTests/`
- Full-stack tests across layers
- Uses **NUnit** and `WebApplicationFactory<Program>`
- Optionally uses `Testcontainers` or Docker for environment setup

---

## 🔄 Dependency Principles

- Each test project should only depend on the layer it validates
- Favor interfaces and mock abstractions for isolation
- Avoid cyclic or reverse references between projects

---

## ✅ Best Practices

- Use consistent testing patterns (e.g. `When_X_Should_Y`)
- Group shared mocks or helpers in a `TestHelpers/` folder
- Follow the same dependency injection patterns as production code
