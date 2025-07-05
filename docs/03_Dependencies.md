# 03 – Project Dependencies

This document outlines the dependencies between the core layers and test projects in the RentAutoApp solution.

---

## 🧱 Dependency Table

| Project                          | Depends On                                                                 |
|----------------------------------|------------------------------------------------------------------------------|
| `RentAutoApp.Web`               | `RentAutoApp.Services.Core`, `RentAutoApp.Web.ViewModels`, `RentAutoApp.Web.Infrastructure` |
| `RentAutoApp.Services.Core`     | `RentAutoApp.Data`, `RentAutoApp.GCommon`, `RentAutoApp.Services.Common`, `RentAutoApp.Services.AutoMapping` |
| `RentAutoApp.Data`              | `RentAutoApp.Data.Models`, `RentAutoApp.Data.Common`                         |
| `RentAutoApp.Services.Core.Tests` | `RentAutoApp.Services.Core`, `RentAutoApp.GCommon`, `RentAutoApp.Data`, **NUnit**, mocking library (e.g., Moq) |
| `RentAutoApp.Web.Tests`         | `RentAutoApp.Web`, `RentAutoApp.Web.ViewModels`, `RentAutoApp.Services.Core`, **NUnit**, mocking library (e.g., Moq) |
| `RentAutoApp.IntegrationTests`  | `RentAutoApp.Web`, `RentAutoApp.Services.Core`, **Microsoft.AspNetCore.Mvc.Testing**, **NUnit**, `TestServer` or `WebApplicationFactory` |

---

## 🧪 Test Project Details

### `RentAutoApp.Services.Core.Tests/`
- Unit tests for business logic and services.
- Uses **NUnit**
- Mocks dependencies using `Moq` or `FakeItEasy`

### `RentAutoApp.Web.Tests/`
- Tests controllers, routing, and validation logic.
- Uses **NUnit** and mocks `Services.Core` interfaces

### `RentAutoApp.IntegrationTests/`
- Full-stack tests across layers
- Based on **NUnit**
- Uses `WebApplicationFactory<Program>` or `TestServer` for full runtime simulation

---

## 🔄 Dependency Principles

- Each test project should only depend on the layer it validates
- Favor interfaces and mock abstractions for isolation
- Avoid cyclic or reverse references between projects

---

## ✅ Best Practices

- Consistent NUnit usage (`[TestFixture]`, `[Test]`)
- Descriptive test method names (e.g. `When_X_Should_Y`)
- Shared test utilities in a `TestHelpers/` folder if needed
