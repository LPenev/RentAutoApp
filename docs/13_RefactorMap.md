# 13 – Refactor & Folder Cleanup Map

This document records structural changes performed during cleanup of the `RentAutoApp.Web` layer to align the project with its multi-layered architecture.

---

## ✅ Folders and Files Moved

| Source (Old Path)                            | Destination (New Path)                          | Note                                     |
|----------------------------------------------|--------------------------------------------------|------------------------------------------|
| `RentAutoApp.Web/Models/Vehicle.cs`          | `RentAutoApp.Data.Models/`                      | Domain entity                            |
| `RentAutoApp.Web/Models/VehicleViewModel.cs` | `RentAutoApp.Web.ViewModels/`                   | UI-specific model                        |
| `RentAutoApp.Web/Services/VehicleService.cs` | `RentAutoApp.Services.Core/`                    | Business logic service                   |
| `RentAutoApp.Web/Repositories/`              | `RentAutoApp.Data/` or `Data.Common/`           | EF-related repositories                  |
| `RentAutoApp.Web/Constants/`                 | `RentAutoApp.GCommon/Constants/`                | Shared constants                         |
| `RentAutoApp.Web/Enums/`                     | `RentAutoApp.GCommon/Enums/`                    | Shared enums                             |
| `RentAutoApp.Web/Helpers/TimeHelper.cs`      | `RentAutoApp.GCommon/Helpers/`                  | General-purpose utility                  |
| `RentAutoApp.Web/Mappings/AutoMapping.cs`    | `RentAutoApp.Services.AutoMapping/`             | AutoMapper profiles                      |
| `RentAutoApp.Web/Validators/*.cs`            | `RentAutoApp.Services.Common/Validators/`       | FluentValidation classes                 |

> 🎯 All `using` directives and namespaces were updated accordingly after moves.

---

## 🧹 Deleted Files & Folders

| Path                                        | Reason for Deletion                            |
|---------------------------------------------|------------------------------------------------|
| `RentAutoApp.Web/Data/`                     | DbContext and migrations handled in `Data/`    |
| `RentAutoApp.Web/Models/` (domain entities) | Already moved to `Data.Models/`                |
| `RentAutoApp.Web/Services/Mock*`            | Temporary or unused mock data                  |
| `RentAutoApp.Web/Repositories/` (duplicated)| Already moved and maintained centrally         |
|----------------------------------------------------------------------------------------------|

---

## 🧭 After Refactor – Clean Web Layer Structure

<pre>
RentAutoApp.Web/ 
├── Controllers/ 
├── Views/ 
├── wwwroot/ 
├── Areas/ 
├── RentAutoApp.Web.ViewModels/ 
├── RentAutoApp.Web.Infrastructure/ 
├── appsettings.json 
└── Program.cs
</pre>

---

## ✍️ Refactor Author

- Performed by: **Lachezar Penev**
- Date: `{05.07.2025}`
- Summary: Reorganized RentAutoApp.Web to comply with layered Clean Architecture. Removed misplaced logic, centralized core services, and updated project references.


