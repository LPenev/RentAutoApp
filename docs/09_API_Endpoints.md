# 09 – API Endpoints & Controller Overview

This document describes the main controller routes and HTTP endpoints exposed by the `RentAutoApp.Web` layer.

---

## 📍 Base Structure

All controllers are located in `RentAutoApp.Web/Controllers/`.  
The API is designed for MVC interactions, but endpoints can also be exposed for AJAX or external API use where applicable.

---

## 🚗 VehicleController

| Method             | Route                     | HTTP | Description                                      |
|--------------------|---------------------------|------|--------------------------------------------------|
| `Index()`          | `/Vehicles`               | GET  | List all vehicles with filters/sorting           |
| `Details(id)`      | `/Vehicles/{id}`          | GET  | Show details for a selected vehicle              |
| `Create()`         | `/Vehicles/Create`        | GET  | Show vehicle creation form                       |
| `Create(model)`    | `/Vehicles/Create`        | POST | Save new vehicle to the fleet                    |
| `Edit(id)`         | `/Vehicles/Edit/{id}`     | GET  | Show edit form for a vehicle                     |
| `Edit(model)`      | `/Vehicles/Edit/{id}`     | POST | Apply updates to the vehicle                     |
| `Delete(id)`       | `/Vehicles/Delete/{id}`   | POST | Remove or archive a vehicle                      |

---

## 📄 RentalController

| Method                  | Route                              | HTTP | Description                            |
|-------------------------|-------------------------------------|------|----------------------------------------|
| `Index()`               | `/Rentals`                          | GET  | List of all reservations/contracts     |
| `Book()`                | `/Rentals/Book`                     | GET  | Start new booking flow                 |
| `Book(model)`           | `/Rentals/Book`                     | POST | Submit a booking/reservation           |
| `Details(id)`           | `/Rentals/Details/{id}`             | GET  | View contract summary + PDF export     |
| `Cancel(id)`            | `/Rentals/Cancel/{id}`              | POST | Cancel an active reservation           |

---

## 📋 DocumentController

| Method              | Route                             | HTTP | Description                            |
|---------------------|------------------------------------|------|----------------------------------------|
| `List(vehicleId)`   | `/Documents/Vehicle/{vehicleId}`   | GET  | List insurance, inspection, vignette   |
| `Upload()`          | `/Documents/Upload`                | POST | Upload new document (PDF, JPEG, etc.)  |
| `Download(id)`      | `/Documents/Download/{id}`         | GET  | Download attached file                 |
| `Delete(id)`        | `/Documents/Delete/{id}`           | POST | Remove expired/duplicate document      |

---

## 🧰 AdminController

| Method             | Route                  | HTTP | Description                                |
|--------------------|------------------------|------|--------------------------------------------|
| `Dashboard()`      | `/Admin/Dashboard`     | GET  | Overview of metrics, expiring documents     |
| `Users()`          | `/Admin/Users`         | GET  | List/manage user roles                      |
| `Statistics()`     | `/Admin/Stats`         | GET  | Reports: rentals/month, top vehicles, etc.  |

---

## 👥 AccountController

| Method             | Route                      | HTTP | Description                          |
|--------------------|----------------------------|------|--------------------------------------|
| `Login()`          | `/Account/Login`           | GET  | Show login form                      |
| `Login(model)`     | `/Account/Login`           | POST | Perform user authentication          |
| `Register()`       | `/Account/Register`        | GET  | Show registration form               |
| `Register(model)`  | `/Account/Register`        | POST | Register a new user                  |
| `Logout()`         | `/Account/Logout`          | POST | Sign out current user                |

---

## 📅 Ajax Endpoints (for calendar, availability, etc.)

| Route                            | HTTP | Description                            |
|----------------------------------|------|----------------------------------------|
| `/api/vehicles/availability`     | GET  | Return JSON status of vehicle availability |
| `/api/reservations/calendar`     | GET  | Return reservation dates in calendar JSON |
| `/api/notifications/pending`     | GET  | Get pending alert notifications for user |

---

## ⚙️ Notes

- Endpoints can be versioned via `/api/v1/` prefix if needed later
- All POST actions should include CSRF protection and model validation
- Responses for AJAX endpoints return `application/json`
- Error responses follow standard status codes (400, 404, 500)

