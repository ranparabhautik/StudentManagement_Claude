# Student Management Claude

A clean-architecture .NET 10 console application for managing student records, built with Claude Code. It demonstrates layered architecture, dependency injection, validation, mapping, and structured logging.

---

## Table of Contents

- [Architecture](#architecture)
- [Project Structure](#project-structure)
- [Technologies](#technologies)
- [Getting Started](#getting-started)
- [Features](#features)
- [Domain Model](#domain-model)
- [Running Tests](#running-tests)

---

## Architecture

The solution follows **Clean Architecture** with four distinct layers:

```
┌─────────────────────────────────────┐
│     StudentManagementClaude         │  Presentation (Console UI)
│     (Entry Point / Console App)     │
├─────────────────────────────────────┤
│     StudentManagement.Application   │  Business Logic (Services, DTOs, Validators)
├─────────────────────────────────────┤
│     StudentManagement.Infrastructure│  Data Access (In-Memory Repositories)
├─────────────────────────────────────┤
│     StudentManagement.Domain        │  Core Entities & Interfaces
└─────────────────────────────────────┘
```

Dependencies flow inward — Domain has no external dependencies; outer layers depend on inner layers only through abstractions.

---

## Project Structure

```
StudentManagementClaude/
│
├── StudentManagement.Domain/              # Core business layer
│   ├── Entities/
│   │   └── Student.cs                    # Student entity
│   └── Interfaces/
│       ├── IEntity.cs                    # Base entity contract
│       ├── IGenericRepository.cs         # Generic CRUD repository interface
│       └── IStudentRepository.cs         # Student-specific repository interface
│
├── StudentManagement.Application/         # Business logic layer
│   ├── DTOs/
│   │   ├── CreateStudentDto.cs
│   │   ├── UpdateStudentDto.cs
│   │   └── StudentResponseDto.cs
│   ├── Interfaces/
│   │   └── IStudentService.cs
│   ├── Services/
│   │   └── StudentService.cs             # Core CRUD business logic
│   ├── Validators/
│   │   ├── CreateStudentDtoValidator.cs  # FluentValidation rules
│   │   └── UpdateStudentDtoValidator.cs
│   ├── Mapping/
│   │   └── StudentMappingProfile.cs      # AutoMapper profiles
│   └── DependencyInjection/
│       └── ApplicationServiceRegistration.cs
│
├── StudentManagement.Infrastructure/      # Data access layer
│   ├── Repositories/
│   │   ├── GenericRepository.cs          # In-memory generic repository
│   │   └── StudentRepository.cs
│   └── DependencyInjection/
│       └── InfrastructureServiceRegistration.cs
│
├── StudentManagementClaude/               # Console entry point
│   └── Program.cs                        # DI setup, Serilog config, interactive menu
│
├── StudentManagement.Tests/               # Unit tests
│   └── Services/
│       └── StudentServiceTests.cs        # xUnit + Moq tests for StudentService
│
├── .github/workflows/
│   ├── claude.yml                        # Claude PR Assistant workflow
│   └── claude-code-review.yml           # Claude Code Review workflow
│
└── README.md
```

---

## Technologies

| Technology | Purpose |
|---|---|
| **.NET 10.0** | Target framework |
| **AutoMapper** | Entity ↔ DTO mapping |
| **FluentValidation** | Input validation with rules |
| **Microsoft.Extensions.DependencyInjection** | IoC container |
| **Microsoft.Extensions.Logging** | Logging abstractions |
| **Serilog** | Structured file logging (`Logs/app-[date].txt`) |
| **xUnit** | Unit testing framework |
| **Moq** | Mocking for unit tests |
| **coverlet** | Code coverage collection |

---

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)

### Build & Run

```bash
# Clone the repository
git clone https://github.com/ranparabhautik/StudentManagement_Claude.git
cd StudentManagement_Claude

# Build the solution
dotnet build

# Run the console application
dotnet run --project StudentManagementClaude
```

### Application Menu

```
=== Student Management System ===
1. Add Student
2. View All Students
3. Find Student by ID
4. Update Student
5. Delete Student
6. Exit
```

---

## Features

- **Add Student** — Enter name, email, enrollment date, and grade with full validation
- **View All Students** — Display all records in a formatted table
- **Find Student by ID** — Look up a specific student
- **Update Student** — Modify an existing student's details
- **Delete Student** — Remove a student with confirmation
- **Validation** — FluentValidation enforces rules on all inputs (required fields, email format, date constraints, length limits)
- **Logging** — Serilog writes structured logs to daily-rolling files under `Logs/`
- **Error Handling** — Validation errors and not-found cases are surfaced with colored console output

---

## Domain Model

```csharp
public class Student : IEntity
{
    public int Id { get; set; }
    public string Name { get; set; }        // Required, max 100 characters
    public string Email { get; set; }       // Required, valid email format
    public DateTime EnrollmentDate { get; set; } // Required, must not be in the future
    public string Grade { get; set; }       // Required, max 5 characters (e.g. A, B+)
}
```

---

## Running Tests

```bash
dotnet test
```

Tests cover:
- Happy-path CRUD operations
- Validation failure scenarios
- Not-found / edge cases

Code coverage is collected via **coverlet**.

---

## GitHub Actions

Two automated workflows run on pull requests:

| Workflow | Trigger | Description |
|---|---|---|
| **Claude PR Assistant** | PR opened / updated | AI-assisted PR review and feedback |
| **Claude Code Review** | PR opened / updated | Automated code quality review |
