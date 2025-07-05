# 12 – Feature Map & Functional Coverage

This document provides an overview of all major features supported by **RentAutoApp**, mapped across user roles and system layers.

---

## 👥 User Roles

| Role       | Description                                 |
|------------|---------------------------------------------|
| Admin      | Full access to all features and stats       |
| Employee   | Operational control over vehicles & rentals |
| Client     | Can register, book, and manage own rentals  |

---

## 🧩 Modules & Features

| Module             | Feature                                     | Roles         | Web UI | API  | DB Model |
|--------------------|---------------------------------------------|---------------|--------|------|----------|
| Vehicles           | Browse & filter fleet                       | All           | ✅     | ✅   | `Vehicle` |
|                    | CRUD vehicle entries                        | Admin, Emp    | ✅     | 🚧   | ✅        |
|                    | Attach photos & specs                       | Admin, Emp    | ✅     | –    | ✅        |
| Documents          | Upload/track insurance, vignette, etc.      | Admin, Emp    | ✅     | ✅   | `Document` |
|                    | Expiration reminders                        | All           | ✅     | –    | ✅        |
| Service History    | Log and schedule repairs                    | Admin, Emp    | ✅     | –    | `ServiceRecord` |
| Reservations       | View availability calendar                  | All           | ✅     | ✅   | –         |
|                    | Make a reservation                          | Client        | ✅     | ✅   | `RentalContract` |
|                    | Cancel reservation                          | Client, Emp   | ✅     | ✅   | ✅        |
| Contracts          | Auto-generate rental contract (PDF)         | Client, Emp   | ✅     | –    | `RentalContract` |
|                    | View & export contract                      | All           | ✅     | ✅   | –         |
| Pricing            | Base rate + coupons/discounts               | All           | ✅     | 🚧   | `PricingRule` |
| Locations          | Manage pickup/drop-off points               | Admin, Emp    | ✅     | –    | `Location` |
| Users              | Registration / Login                        | All           | ✅     | ✅   | `User`    |
|                    | Role-based dashboard                        | All           | ✅     | –    | –         |
| Dashboard          | Admin metrics & statistics                  | Admin         | ✅     | 🚧   | Aggregates |
| Notifications      | Alerts: expiring docs, rentals, services    | All           | ✅     | ✅   | `Notification` |
| Multilingual UI    | BG / EN support                            | All           | ✅     | –    | –         |
| Invoice            | Auto-generate invoice (PDF/Email)           | Client, Admin | ✅     | –    | `Invoice` |
| Visual Calendar    | Booking overview by date & vehicle          | All           | ✅     | ✅   | –         |
| Touch Signature    | Optional digital contract signing           | Client        | ✅     | –    | –         |

## 🗂️ Status Legend

- ✅ Fully implemented or planned and in progress

- 🚧 Planned for future implementation

– Not applicable or handled by another layer

## 📌 Notes
- AJAX endpoints serve the calendar, availability, and quick status updates

- Some internal features (e.g. statistics, invoicing) may be batch-based or triggered by back-office jobs in later phases

- Multilingual support is primarily UI-based; model values (e.g. categories) are language-independent