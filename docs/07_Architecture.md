# 07 – System Architecture

This document outlines the architectural principles and design structure of the **RentAutoApp** solution, which follows a layered and modular architecture inspired by Clean Architecture and the Separation of Concerns principle.

---

## 🧱 Architectural Layers

[Web Layer]
	↓ 
[Services Layer]
	↓
[Data Layer]
	↓
[Data.Models & GCommon]

### 1. Architecture Diagram

# 🏛️ High-Level Architecture Diagram – RentAutoApp

                               +----------------------+
                               |    Client Browser    |
                               | (Razor Views / JS)   |
                               +----------+-----------+
                                          |
                                   HTTP / AJAX
                                          v
                               +----------------------+
                               |  RentAutoApp.Web     |
                               |   (Controllers)      |
                               +----------+-----------+
                                          |
                                          v
                         +----------------+-----------------+
                         |       Services Layer             |
                         |  RentAutoApp.Services.Core       |
                         +----------------+-----------------+
                                          |
                                          v
                            +-------------+-------------+
                            |         Data Layer        |
                            |     RentAutoApp.Data      |
                            +------+----------+---------+
                                   |          |
                                   v          v
              +------------------------+   +-----------------------------+
              | RentAutoApp.Data.Models|   | RentAutoApp.Data.Common     |
              +------------------------+   +-----------------------------+

                           +--------------------------------+
                           |        RentAutoApp.GCommon     |
                           | (Enums, Interfaces, Constants) |
                           +--------------------------------+

🧭 Notes
Flow: Top-down from Web → Services → Data → Models/Common

GCommon sits at the bottom and is referenced only upward by all tiers

This diagram helps developers understand separation of concerns and tiered dependencies at a glance

### 2. Presentation Layer (`RentAutoApp.Web`)
- ASP.NET Core MVC Controllers
- Razor Views & AJAX front-end logic
- Consumes ViewModels and service interfaces
- Responsible for input validation, response formatting, and user flow

### 3. Application/Service Layer (`RentAutoApp.Services.Core`)
- Business logic and use case orchestration
- Interfaces and implementations of service contracts
- Handles mapping (via AutoMapper) and orchestration of data operations

### 4. Data Access Layer (`RentAutoApp.Data`)
- Manages persistence via Entity Framework Core
- Contains `DbContext`, repositories, and unit of work patterns
- Depends on Models and Common interfaces

### 5. Shared Models & Utilities
- `RentAutoApp.Data.Models`: Entity classes mapped to the database
- `RentAutoApp.GCommon`: Shared enums, interfaces, constants, and utilities
  - Lowest tier in the system — **has no dependencies**

---

## 🔄 Communication Flow

User → Controller → Service → Repository → DbContext

View → ViewModel

Controller → Service (via DI)

Service maps ViewModel → Domain Model (AutoMapper)

Repository performs CRUD via EF Core

Response flows back: Domain Model → DTO/ViewModel → View

## ⚙️ Key Patterns Used
Pattern	Purpose
Dependency Injection (DI)	Inject services and repositories via constructor
Repository Pattern	Abstracts EF Core logic and enhances testability
AutoMapper	Separates mapping logic and maintains clean service methods
FluentValidation	Centralized validation logic for ViewModels
Layered Architecture	Ensures clear separation of concerns

## 🚫 Prohibited Dependencies
UI (Web) layer must NOT depend directly on the Data Layer

No project should reference “upwards” (e.g., Data referencing Services)

GCommon stays isolated — only referenced but never dependent

## 🧠 Architectural Principles Followed
Separation of Concerns Each layer has a clear purpose and responsibility.

Inversion of Control / DIP Layers communicate through interfaces to increase flexibility.

Open/Closed Principle Business logic is open for extension via new service implementations.

Testability Layers can be unit-tested in isolation using mocked dependencies.

## 🧩 Deployment View
Application entry point: RentAutoApp.Web

Configuration via Program.cs and appsettings.json

