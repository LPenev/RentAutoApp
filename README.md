# 🚗 RentAutoApp – Vehicle Rental Management System

**RentAutoApp** is a modern, full-stack web application built with **ASP.NET Core 8 MVC** for managing rental fleets of cars, SUVs, and motorcycles. It streamlines reservations, contracts, vehicle servicing, pricing, notifications, and document management — all in a clean, extensible architecture.

---

## 📦 Features

- **Fleet Management**: Categorized vehicles with availability and maintenance status
- **Reservation Engine**: Full contract flow with pickup/drop-off locations and calendar views
- **Documents & Notifications**: Track technical inspections, insurance, service history, and reminders
- **Pricing & Discounts**: Flexible pricing rules, coupons, and long-term rental offers
- **Multilingual Support**: UI in Bulgarian 🇧🇬 and English 🇬🇧
- **Admin Dashboard**: Stats, client history, most rented vehicles, pending actions

---

## 🛠️ Tech Stack

| Layer            | Technology                                              |
|------------------|----------------------------------------------------------|
| Backend          | ASP.NET Core 8 MVC, C#, Entity Framework Core 8         |
| Frontend         | Razor Views, Bootstrap 5, **AJAX** (jQuery/Fetch)       |
| Database         | **Microsoft SQL Server (MSSQL)**                        |
| Mapping / Validation | AutoMapper, FluentValidation                       |
| Testing          | **NUnit**, Moq, Microsoft.AspNetCore.Mvc.Testing        |
| Documentation    | MkDocs + Material Theme (`/docs`)                      |

---

## ⚙️ Getting Started

### 0. InitDemo

- Read docs/00_InitDemo.md

### 1. Prerequisites

- .NET 8 SDK
- Visual Studio 2022+
- SQL Server or LocalDB

### 2. Run the Project

bash:
	git clone https://github.com/LPenev/RentAutoApp.git
	cd RentAutoApp

# Optional: configure appsettings.json
dotnet ef database update --project RentAutoApp.Data --startup-project RentAutoApp.Web

dotnet run --project src/RentAutoApp.Web

## 📚 Documentation
Comprehensive architecture, setup guides, and module documentation can be found inside the /docs/ folder or rendered via:
	mkdocs serve

### 📁 Docs structure:

01_Overview.md: General purpose and scope

07_Architecture.md: Layered architecture and principles

09_API_Endpoints.md: Route and controller reference

See full structure in docs/mkdocs.yml

## 🔍 Project Structure
<pre>
	RentAutoApp/ 
	├── src/ # Main application source 
	├── tests/ # Unit and integration tests (NUnit) 
	├── docs/ # Markdown documentation & mkdocs config 
	└── README.md # You're here 
</pre>

## 🧪 Testing
Run all tests using:
	dotnet test
Tested layers include:

- ✅ Services (business logic)

- ✅ Web controllers

- ✅ End-to-end API integration

Test framework: NUnit with mocks (Moq)

## 🤝 Contributing
Contributions are welcome! See:
	docs/04_DevelopmentSetup.md
	docs/11_Contributing.md

Suggested branch flow:
	main       → stable
	dev        → integration
	feature/*  → features
	bugfix/*   → fixes

## 📄 License

© 2025 Lachezar Penev — All rights reserved.
