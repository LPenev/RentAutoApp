# 08 – Database Design & Schema

This document provides an overview of the database layer in RentAutoApp, including key models, relationships, and data access technologies.

---

## 🗃️ ORM & Storage Engine

| Component            | Technology                  |
|----------------------|------------------------------|
| Object–Relational Mapping | Entity Framework Core 8       |
| Database Engine       | Microsoft SQL Server (LocalDB or full) |
| Migrations            | EF Core Code-First Migrations  |
| Connection Config     | `DefaultConnection` in `appsettings.json` |

---

## 📦 Data Access Layer

Located in: `src/RentAutoApp.Data/`

- `ApplicationDbContext` – Inherits from `DbContext`
- Configured with `DbSet<TEntity>` for all entities
- Registered in `Program.cs` via:
- CSharp
  services.AddDbContext<ApplicationDbContext>(options =>
      options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

## 📊 Core Domain Entities
Entity						Purpose
Vehicle			Main fleet unit with availability status
ServiceRecord	Repair logs, mileage/date-based
Document		Vignette, insurance, technical control
User			Roles: Admin, Employee, Client
RentalContract	Reservation and booking record
Location		Pickup/return location
PricingRule		Discount logic, pricing tiers

## 🔗 Relationships
- One-to-many: Vehicle → ServiceRecord, RentalContract

- One-to-many: User → RentalContract

- One-to-many: Vehicle → Document

- Many-to-one: RentalContract → Location (Pickup, DropOff)

## 🧰 Migrations
- Always generate a new migration

## 🧪 Testing Setup
- Integration tests use either InMemoryDatabase or sandbox SQL instance

- Optionally use appsettings.Test.json for a clean connection string

## 🛡️ Best Practices
- Use Guid as primary keys for external safety (e.g. public URLs)

- Normalize repeating data (fuel types, roles, etc.)

- Always seed admin user and lookup data in migrations or DbInitializer

- Soft-delete: Consider adding IsDeleted if archival is required

