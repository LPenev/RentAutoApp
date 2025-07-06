# 02 – Project Structure

This file documents the directory structure of the RentAutoApp solution.

## 🔷 Root Layout
<pre> ```
RentAutoApp/
├── src/
│   ├── RentAutoApp.Web/ 
│   │	├── RentAutoApp.Web.ViewModels/
│   │	└── RentAutoApp.Web.Infrastructure/
│   ├── RentAutoApp.Data/
│   │	├── RentAutoApp.Data.Models/
│   │	└── RentAutoApp.Data.Common/
│   ├── RentAutoApp.Services.Core/
│   │	├── RentAutoApp.Services.AutoMapping/
│   │	└── RentAutoApp.Services.Common/
│   └── RentAutoApp.GCommon/
├── tests/
│   ├── RentAutoApp.Services.Core.Tests/
│   ├── RentAutoApp.Web.Tests/
│   └── RentAutoApp.IntegrationTests/
├── docs/
├── README.md
├── RentAutoApp.sln
└── mkdocs.yml
``` </pre>
## 🔹 Folder Roles

- `Web/`: MVC controllers, Razor views, ViewModels, Infrastructure
- `Data/`: EF models, DbContext, repository abstractions
- `Services.Core/`: Business logic, service interfaces, AutoMapper profiles
- `GCommon/`: Shared enums, interfaces, constants
- `tests/`: Unit and integration tests
- `docs/`: Markdown documentation, mkdocs config, project readme
