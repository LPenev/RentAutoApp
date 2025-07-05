# 05 – Code Conventions

This document defines the coding standards used in the **RentAutoApp** solution to ensure consistency, readability, and maintainability across all tiers of the application.

---

## 💡 General Guidelines

- Use **C# 10+** language features when appropriate (e.g. `record` types, `switch expressions`, nullable reference types).
- Keep each class in its own file with the same name as the class.
- Organize folders to reflect domain layers (Web, Services, Data, etc.).
- Prefer **single responsibility** per class and method.
- Group related files using `partial` classes where helpful.

---

## 📛 Naming Conventions

| Element        | Convention                            | Example                           |
|----------------|----------------------------------------|------------------------------------|
| Classes        | `PascalCase`                          | `VehicleService`, `UserManager`   |
| Interfaces     | `PascalCase`, prefix with `I`         | `IVehicleRepository`, `INotifier` |
| Methods        | `PascalCase`                          | `GenerateReport()`, `SaveAsync()` |
| Variables      | `camelCase`                           | `currentUser`, `isAvailable`      |
| Constants      | `PascalCase`                          | `MaxRentalDays`, `DefaultLanguage`|
| Enums          | `PascalCase`                          | `FuelType.Gasoline`               |
| Async Methods  | Suffix with `Async`                   | `UpdateReservationAsync()`        |
| Test Methods   | `MethodUnderTest_Scenario_Expected()` | `AddReservation_WhenValid_ShouldCreateEntry()` |

---

## 📁 Folder Layout & Namespace Patterns

- Use root-to-leaf namespaces that match folder structure:

- Keep ViewModels in `Web.ViewModels`, not in `Controllers` or `Views`.

- Group services, validators, mappers, etc. by domain (e.g. `Reservations`, `Users`).

---

## 🎯 Architecture Guidelines

- Use **Dependency Injection** via constructors for all services.
- Avoid static classes unless utility-only.
- Interface-first design: inject `IVehicleService`, not `VehicleService`.
- Keep business logic out of controllers; delegate to services.

---

## 🧼 Formatting & Style

- Indent with **4 spaces**, never tabs
- Place `{` on new line for types/methods:
```csharp
public class VehicleService
{
    public Task AddAsync() { }
}
