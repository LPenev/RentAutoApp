# 06 – Testing Strategy

This document describes the testing approach, tools, and structure for the RentAutoApp solution. The goal is to ensure high code quality, stability, and maintainability through automated tests at multiple levels.

---

## 🧪 Testing Frameworks & Tools

| Purpose         | Tool / Library                       |
|------------------|--------------------------------------|
| Test Framework   | **NUnit**                            |
| Mocking          | Moq or FakeItEasy                    |
| Test Runner      | Visual Studio Test Explorer, `dotnet test` CLI |
| Integration      | Microsoft.AspNetCore.Mvc.Testing, TestServer, WebApplicationFactory |
| Assertions       | NUnit Assert, FluentAssertions (optional) |

---

## 🏗️ Test Project Structure

tests/
├── RentAutoApp.Services.Core.Tests/ # Unit tests for business logic and services 
├── RentAutoApp.Web.Tests/ # Unit tests for controller actions and web logic
└── RentAutoApp.IntegrationTests/ # API integration tests across all layers


---

## 🧩 Types of Tests

### ✅ Unit Tests
- Test isolated components (services, validators, helpers)
- Use mocking to eliminate dependencies
- Fast to execute and run as part of CI

### 🔁 Integration Tests
- Test real interactions between layers (Web ⇄ Services ⇄ DB)
- Use in-memory DBs or sandbox SQL instance
- Validate real routing, dependency injection, middleware behavior

---

## 🔎 Naming & Structure

- Test class names: `ClassUnderTestTests` (e.g. `VehicleServiceTests`)
- Method naming: `MethodName_Condition_ExpectedBehavior`

## csharp
public async Task CreateReservation_WhenCarUnavailable_ShouldThrowException()

## Arrange-Act-Assert pattern
Follow Arrange–Act–Assert pattern:
// Arrange
var service = new VehicleService(...);
// Act
var result = await service.CreateReservationAsync(...);
// Assert
Assert.IsNotNull(result);

## ⚙️ Test Execution
Run all tests via CLI:
dotnet test

## 🧼 Best Practices
Keep test methods focused — 1 assert per test if possible

Separate test data using builders or fixtures

Avoid testing private methods — test behavior through public APIs

Use [SetUp] for common test init and [TearDown] for cleanup

Tests should be deterministic (no randomness or external state)
