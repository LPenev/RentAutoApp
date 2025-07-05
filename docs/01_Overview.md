# 01 – Project Overview

**RentAutoApp** is a modern, web-based Vehicle Rental Management System built with ASP.NET Core MVC (.NET 8).  
The application serves the operational needs of rental businesses managing fleets of cars, SUVs, and motorcycles.

## 🎯 Purpose

The solution is designed to automate the entire rental workflow:

- Vehicle and fleet management
- Reservation handling with availability checks
- Digital contract generation in PDF
- Maintenance schedules and expiry reminders
- Real-time vehicle status through AJAX updates
- Price calculation with discount engine
- Secure user access by roles (Admin, Employee, Client)

## 🌍 Target Audience

- Vehicle rental companies
- Fleet managers
- Internal staff responsible for bookings and contracts
- Clients renting vehicles online

## 🛠️ Technology Stack

| Category         | Technologies Used                                    |
|------------------|------------------------------------------------------|
| Backend          | ASP.NET Core 8 MVC, C#, Entity Framework Core 8      |
| Frontend         | Bootstrap 5, Razor Pages, **AJAX** (jQuery/Fetch)    |
| Database         | **Microsoft SQL Server (MSSQL)**                     |
| Mapping & Models | AutoMapper                                           |
| Validation       | FluentValidation                                     |
| Docs & Config    | Markdown (MkDocs), JSON configuration files          |
| Hosting          | Windows / IIS / Azure (future deployment)            |

## 🧩 Key Features

- Reservation calendar with AJAX-based availability updates
- Document expiry alerts (insurance, inspections, vignettes)
- Automatic PDF contract creation and optional signature
- Multi-role system with dynamic dashboard per role
- Real-time pricing engine with promotional logic
- Multilingual UI (Bulgarian / English)
