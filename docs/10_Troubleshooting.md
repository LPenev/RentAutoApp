# 10 – Troubleshooting Guide

This document lists common issues encountered when building, running, or testing the RentAutoApp project, along with solutions or mitigation steps.

---

## 🛠️ Build & Restore Issues

### ❌ `The type or namespace could not be found...`
- **Cause**: Missing project reference between layers.
- ✅ Solution: 
  - Open Visual Studio > Right-click project > `Add` → `Project Reference`
  - Verify dependencies as per `03_Dependencies.md`

---

### ❌ `CS0246: Cannot find symbol 'DbContext'`
- **Cause**: Missing EF Core package.
- ✅ Solution: 
  bash
  dotnet add package Microsoft.EntityFrameworkCore

### ❌ IntelliSense or build fails after git pull
- **Cause**: Missing NuGet packages or corrupted cache.

- ✅ Solution:
  dotnet restore

### 🧰 Migrations & EF Core Errors

### ❌ Unable to connect to database
Cause: Incorrect or missing DefaultConnection string.

- ✅ Solution:
  Open appsettings.json

- Use this as a fallback (for LocalDB):
  "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=RentAutoAppDb;Trusted_Connection=True;"

### ❌ More than one DbContext was found
Cause: Multiple contexts in solution but EF CLI doesn't know which to use.

- ✅ Solution:
  dotnet ef migrations add MigrationName --context ApplicationDbContext

### ❌ Pending model changes detected
- Cause: Model changes not yet migrated.

- ✅ Solution:
  dotnet ef migrations add YourMigrationName
  dotnet ef database update

##🧪 Test Failures

### ❌ System.NullReferenceException in test
- Cause: Unmocked dependency or uninitialized test setup.

- ✅ Solution:
  Ensure dependencies are mocked (e.g., with Moq or FakeItEasy)
  Add [SetUp] to initialize required services

### ❌ Test database not found
- Cause: In-memory or Test DB not configured.

- ✅ Solution:
  Add appsettings.Test.json with a dedicated connection string
  Use UseInMemoryDatabase("TestDb") for unit tests
  
## 🌐 Web App Issues

### ❌ HTTP 500 after app launch
- Cause: Middleware exception or misconfigured DI.

- ✅ Solution:
  Check logs in stdout
  Enable DeveloperExceptionPage() in Program.cs if in Development

### ❌ Razor views not updating after edit
- ✅ Solution:
  Clear temp files: Clean → Rebuild Solution
  Ensure Razor runtime compilation is enabled in dev mode

### 🧼 General Tips
- Restart Visual Studio or dotnet clean + dotnet build after major changes
- Use dotnet watch run during development for auto-reloads
- Keep migrations in sync with your codebase — don’t skip them!
- Set RentAutoApp.Web as the startup project explicitly