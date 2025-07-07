# 14 – Project References Map

This document defines allowed and expected project dependencies in the `RentAutoApp` solution.  
It ensures architectural consistency by listing who can reference whom based on layer responsibilities.

---

## 🧭 Dependency Direction (Layered View)

<pre>
src/
├── RentAutoApp.Web/
│   └── [Refs: Services.Core, Web.ViewModels, Web.Infrastructure, GCommon]

├── RentAutoApp.Web.ViewModels/
│   └── [Refs: FluentValidation (external only)]

├── RentAutoApp.Web.Infrastructure/
│   └── [Refs: ViewModels, optionally GCommon]

├── RentAutoApp.Services.Core/
│   └── [Refs: Data, Services.AutoMapping, Services.Common, GCommon]

├── RentAutoApp.Services.AutoMapping/
│   └── [Refs: Web.ViewModels, Data.Models, GCommon]

├── RentAutoApp.Services.Common/
│   └── [Refs: GCommon]

├── RentAutoApp.Data/
│   └── [Refs: Data.Models, Data.Common]

├── RentAutoApp.Data.Models/
│   └── [Refs: GCommon (optionally)]

├── RentAutoApp.Data.Common/
│   └── [Refs: GCommon]

├── RentAutoApp.GCommon/
│   └── [Refs: (none)]
</pre>

---

## 🔗 Allowed Project References

| Project                                  | May Reference                                                                 |
|------------------------------------------|--------------------------------------------------------------------------------|
| `RentAutoApp.Web`                        | `Services.Core`, `Web.ViewModels`, `Web.Infrastructure`, `GCommon`            |
| `RentAutoApp.Web.ViewModels`             | _(no outward references)_                                                     |
| `RentAutoApp.Web.Infrastructure`         | `Web.ViewModels`, optionally `GCommon`                                        |
| `RentAutoApp.Services.Core`              | `Data`, `Services.AutoMapping`, `Services.Common`, `GCommon`                  |
| `RentAutoApp.Services.AutoMapping`       | `Data.Models`, `Web.ViewModels`, `GCommon`                                    |
| `RentAutoApp.Services.Common`            | `GCommon`                                                                     |
| `RentAutoApp.Data`                       | `Data.Models`, `Data.Common`                                                  |
| `RentAutoApp.Data.Models`                | `GCommon` (only for shared enums/interfaces)                                  |
| `RentAutoApp.Data.Common`                | `GCommon`                                                                     |
| `RentAutoApp.GCommon`                    | _(must not reference any other project)_                                      |

---

## 🚫 Forbidden References

These dependencies violate architecture and should be avoided:

| Forbidden Reference                           | Reason                                                |
|-----------------------------------------------|--------------------------------------------------------|
| `Web` ➡ `Data`                                | Web must use services only                            |
| `Services.Core` ➡ `Web.ViewModels` (except via AutoMapping) | Logic must be UI-agnostic                  |
| `Data.Models` ➡ `Services.Core`               | Low-level layer depending on high-level               |
| `GCommon` ➡ `Web` or `Services.Core`          | GCommon must stay completely decoupled                |

---

## 🛠️ How to Enforce

- Use **Solution Explorer → Project Dependencies** to verify allowed references.
- Enforce with:
  - `ArchUnitNET` (architecture tests)
  - `.editorconfig` or `Directory.Build.props` restrictions
  - Visual Studio analyzers

---

## 🧩 Visual Summary

<pre>
src/
├── RentAutoApp.Web/
├── RentAutoApp.Web.ViewModels/
├── RentAutoApp.Web.Infrastructure/
├── RentAutoApp.Services.Core/
├── RentAutoApp.Services.AutoMapping/
├── RentAutoApp.Services.Common/
├── RentAutoApp.Data/
├── RentAutoApp.Data.Models/
├── RentAutoApp.Data.Common/
└── RentAutoApp.GCommon/
</pre>

Arrows → represent allowed references (unidirectional)

```
GCommon ← (Data.Models, Data.Common, Services.Common, AutoMapping, Core, Web)
     ↑
Data.Models ← Data ← Services.Core
Web.ViewModels ← AutoMapping ← Services.Core ← Web
Web.Infrastructure ← Web
```

---

By enforcing these reference rules, you protect **clean architecture boundaries** and enable long-term maintainability.  
_“Architecture is the decisions you wish you didn’t have to revisit.”_ 🧱
