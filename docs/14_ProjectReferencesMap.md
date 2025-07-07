# 14 – Project References Map

This document defines allowed and expected project dependencies in the `RentAutoApp` solution.  
It ensures architectural consistency by listing who can reference whom based on layer responsibilities.

---

## 🧭 Dependency Direction (Clean Architecture)

<pre>
src/ 
├── Web/
│   ├── ViewModels/
│   ├── Infrastructure/
│   └── [Refs: Services.Core + GCommon] 
│    
├── Services.Core/ 
│   ├── AutoMapping/ 
│   ├── Common/ 
│   └── [Refs: Data + GCommon] 
├── Data/ 
│ ├── Data.Models/ 
│ 	├── Data.Common/ 
│ 	└── [Refs: Models + Common] 
└── GCommon/ 
    └── [Refs: (none)]
</pre>

---

## 🔗 Allowed Project References

| Project                            | May Reference                                                  |
|------------------------------------|----------------------------------------------------------------|
| `RentAutoApp.Web`                  | `Services.Core`, `Web.ViewModels`, `Web.Infrastructure`, `GCommon` |
| `RentAutoApp.Services.Core`        | `Data`, `GCommon`, `Services.AutoMapping`, `Services.Common`      |
| `RentAutoApp.Data`                 | `Data.Models`, `Data.Common`                                   |
| `RentAutoApp.Web.ViewModels`       | _(no outward references)_                                      |
| `RentAutoApp.Web.Infrastructure`   | _(optional reference to ViewModels or GCommon)_                |
| `RentAutoApp.Services.AutoMapping` | `GCommon`, `Data.Models`                                       |
| `RentAutoApp.Services.Common`      | `GCommon`                                                      |
| `RentAutoApp.Data.Models`          | `GCommon` (if needed for enums, base interfaces)               |
| `RentAutoApp.GCommon`              | _(should not reference any other project)_                     |

---

## 🚫 Forbidden References

These dependencies violate architecture and should be avoided:

| Forbidden Reference                           | Reason                                                |
|-----------------------------------------------|--------------------------------------------------------|
| `Web` ➡ `Data`                                | UI must call services, not access DB directly         |
| `Services.Core` ➡ `Web.ViewModels`            | Core logic shouldn't depend on UI                     |
| `Data.Models` ➡ `Services.Core`               | Data should remain isolated from upper layers         |
| `GCommon` ➡ `Web` or `Services.Core`          | Common layer must stay dependency-free                |

---

## 🛑 Tip for Enforcement

Use `Solution Explorer → Project Dependencies` to visualize links.

Alternatively, enable analyzers like:
- **[ArchUnitNET](https://github.com/TNG/ArchUnitNET)** (architecture testing)
- **.editorconfig + StyleCop rules** to restrict usage of forbidden namespaces

---

## 🧩 Visual Summary
<pre>
src/ 
├── Web/
│   ├── ViewModels/
│   ├── Infrastructure/
│   └── [Refs: Services.Core + GCommon] 
│    
├── Services.Core/ 
│   ├── AutoMapping/ 
│   ├── Common/ 
│   └── [Refs: Data + GCommon] 
├── Data/ 
│ ├── Data.Models/ 
│ 	├── Data.Common/ 
│ 	└── [Refs: Models + Common] 
└── GCommon/ 
    └── [Refs: (none)]
</pre>

---

By enforcing this dependency flow, we ensure **long-term flexibility**, easier testing, and cleaner onboarding for new team members.  
_“Architecture is what determines what you don’t have to think about.”_ 🚦🧱

