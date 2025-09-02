# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Architecture Overview

This is an **ASP.NET Core 9.0** application built on the **ABP Framework (v9.3.2)** using Domain-Driven Design principles. The solution follows a modular architecture with clear separation of concerns:

### Project Structure
- **src/**: Main application modules
  - `Crm.Domain.Shared` - Shared domain contracts and constants
  - `Crm.Domain` - Core domain logic and entities
  - `Crm.Application.Contracts` - Application service interfaces
  - `Crm.Application` - Application service implementations
  - `Crm.EntityFrameworkCore` - Entity Framework Core data access
  - `Crm.HttpApi` - HTTP API layer

- **test/**: Test projects
  - `Crm.TestBase` - Base test infrastructure
  - `Crm.Domain.Tests` - Domain layer tests
  - `Crm.Application.Tests` - Application layer tests
  - `Crm.EntityFrameworkCore.Tests` - Data access tests

- **shared/**: Shared Astra framework modules
  - Multiple Astra.* projects providing foundational infrastructure

### Key Technologies
- .NET 9.0
- ABP Framework 9.3.2
- Entity Framework Core
- Modular architecture with dependency injection
- Localization support
- Validation framework

## Development Commands

### Build Commands
```bash
# Build entire solution
dotnet build Crm.sln

# Build specific project
dotnet build src/Crm.Domain/Crm.Domain.csproj

# Clean solution
dotnet clean Crm.sln
```

### Test Commands
```bash
# Run all tests
dotnet test Crm.sln

# Run specific test project
dotnet test test/Crm.Application.Tests/Crm.Application.Tests.csproj

# Run tests with specific filter
dotnet test --filter "FullyQualifiedName~TestClassName"

# Run tests with verbosity
dotnet test --logger "console;verbosity=detailed"
```

### Common Development Tasks
```bash
# Add new package reference
dotnet add package Package.Name

# Restore packages
dotnet restore Crm.sln

# List project references
dotnet list reference
```

### Code Quality
```bash
# Format code (if editorconfig is configured)
dotnet format

# Analyze solution
dotnet analyze
```

## Architectural Patterns
- **Domain-Driven Design** with clear bounded contexts
- **Layered Architecture** with separation of concerns
- **Dependency Injection** throughout the application
- **Repository Pattern** for data access abstraction
- **CQRS Pattern** for application services
- **Modular Design** with ABP framework modules

## Important Conventions
- Follow ABP Framework naming conventions
- Use async/await pattern for I/O operations
- Implement proper error handling with ABP exception system
- Follow localization patterns for user-facing messages
- Use Entity Framework Core code-first migrations for database changes