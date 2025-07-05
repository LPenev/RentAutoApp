# 11 – Contributing Guidelines

Thank you for considering contributing to **RentAutoApp**!  
This document provides guidance for setting up your environment, making changes, and submitting high-quality pull requests.

---

## 🤝 Who Can Contribute

- Internal developers in the RentAuto team
- External contributors (if open-sourced later)
- QA engineers providing feedback or test cases
- Technical writers improving documentation

---

## 🧭 Project Philosophy

We believe in:

- Clean architecture and modular layers
- Code readability and reusability
- Test coverage and stability
- Respectful and collaborative communication

---

## 🔧 Dev Environment Setup

See [`04_DevelopmentSetup.md`](04_DevelopmentSetup.md) to configure your environment properly.

---

## 🛠️ Contribution Workflow

1. **Fork** the repository (if public), or create a new branch from `main`
2. **Create a feature branch**  
3. **Make your changes**
- Follow guidelines in [`05_CodeConventions.md`](05_CodeConventions.md)
- Include relevant tests and docs
4. **Commit your changes**  
5. **Run tests** before pushing:
6. **Push** and open a **Pull Request (PR)** to `main`

---

## ✍️ Commit Message Format

Use clear, descriptive messages:

feat: add support for contract PDF generation
fix: handle null in reservation calendar
docs: improve readme and architecture diagram

## 📣 Code Review Process
At least one developer must approve each PR

Reviewers may suggest refactoring or clarification

PRs should stay focused and small (1 feature or fix max)

Approved PRs are merged by maintainers using Squash and merge


## 💬 Communication
Use meaningful comments in your code

Open issues for bugs or design discussions

Prefer async communication (e.g., GitHub Issues, comments)

## 🧼 Branching Strategy
Branch				Purpose
main		Stable, production-ready code
dev			Main development integration branch
feature/*	Feature development (short-lived)
bugfix/*	Small patches or fixes
docs/*		Documentation-only changes

## 📜 License & Code of Conduct
If the project becomes open-source:

A standard MIT or Apache license will apply

A Contributor Covenant code of conduct will be included

