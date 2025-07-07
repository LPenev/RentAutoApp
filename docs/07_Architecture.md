# 07 – System Architecture

This document outlines the architectural principles and design structure of the **RentAutoApp** solution, which follows a layered and modular architecture inspired by Clean Architecture and the Separation of Concerns principle.

---

## 🧱 Architectural Layers

[Web Layer]  
 ↓  
[ViewModels / Infrastructure]  
 ↓  
[Services Layer]  
 ↓  
[Data Layer]  
 ↓  
[Data.Models / Data.Common / GCommon]

---

### 1. High-Level Architecture Diagram – RentAutoApp

```plaintext
                             +----------------------+
                             |    Client Browser    |
                             | (Razor Views / JS)   |
                             +----------+-----------+
                                        |
                                 HTTP / AJAX
                                        v
                             +--------------------------+
                             |  RentAutoApp.Web         |
                             |  (Controllers, Razor)    |
                             +-----------+--------------+
                                         |
                                         v
                         +---------------+----------------+
                         |      Web Layer Submodules      |
                         | Web.ViewModels / Infrastructure|
                         +---------------+----------------+
                                         |
                                         v
                         +---------------+----------------+
                         |        Services Layer           |
                         | RentAutoApp.Services.Core       |
                         +--+--------------------------+---+
                            | Services.Common          |
                            | Services.AutoMapping     |
                            +--------------------------+
                                         |
                                         v
                            +------------+------------+
                            |         Data Layer      |
                            |    RentAutoApp.Data     |
                            +-------+-----------+-----+
                                    |           |
                                    v           v
           +-----------------------------+  +----------------------------+
           |   RentAutoApp.Data.Models   |  |  RentAutoApp.Data.Common   |
           +-----------------------------+  +----------------------------+

                           +--------------------------------+
                           |        RentAutoApp.GCommon     |
                           | (Enums, Interfaces, Constants) |
                           +--------------------------------+
```

---

### 2. Layer Descriptions

#### Presentation Layer (`RentAutoApp.Web`)
- ASP.NET Core MVC
- Razor views and client-side interactions
- Uses `RentAutoApp.Web.ViewModels` and `RentAutoApp.Web.Infrastructure`
- Handles requests and routes them to services

#### ViewModels Layer (`RentAutoApp.Web.ViewModels`)
- View models and form binding DTOs
- Uses `FluentValidation` for input validation

#### Infrastructure Layer (`RentAutoApp.Web.Infrastructure`)
- Middleware, filters, extensions for MVC
- Optional helpers for web configuration

#### Application / Service Layer (`RentAutoApp.Services.Core`)
- Business logic, orchestrations, validation triggers
- Uses:
  - `RentAutoApp.Services.AutoMapping` for AutoMapper profiles
  - `RentAutoApp.Services.Common` for shared logic and validators

#### Data Layer (`RentAutoApp.Data`)
- Database context (DbContext)
- Manages data access via repositories and Unit of Work
- Depends on:
  - `RentAutoApp.Data.Models`
  - `RentAutoApp.Data.Common`

#### Models & Shared Utilities
- `RentAutoApp.Data.Models`: Entity Framework models
- `RentAutoApp.Data.Common`: Interfaces and base repository classes
- `RentAutoApp.GCommon`: Shared enums, interfaces, constants

---

## 🔄 Communication Flow

User → Controller → Service → Repository → DbContext  
View → ViewModel  
Controller → Service (via DI)  
Service → Mapping (AutoMapper)  
Repository → DB  
Response flows back → Domain Model → ViewModel → View

---

## ⚙️ Key Patterns Used

| Pattern                  | Purpose |
|--------------------------|---------|
| Dependency Injection     | Inject services/repositories via constructors |
| Repository Pattern       | Abstraction over EF Core data access          |
| AutoMapper               | Clean and testable object-to-object mapping   |
| FluentValidation         | Input validation through separate validators |
| Layered Architecture     | Strong separation of responsibilities         |

---

## 🚫 Prohibited Dependencies

- Web layer **must NOT** depend directly on Data Layer
- No upward references (e.g., Data → Services)
- `GCommon` stays decoupled — only referenced, never dependent

---

## 🧠 Architectural Principles Followed

- **Separation of Concerns** — each layer with a specific purpose
- **Dependency Inversion** — rely on abstractions/interfaces
- **Open/Closed Principle** — extend business logic via services
- **Testability** — isolated layers, easy to test with mocks

---

## 🧩 Deployment View

- Entry point: `RentAutoApp.Web`
- Startup: `Program.cs`, `appsettings.json`
- Can be containerized for deployment (optional)
