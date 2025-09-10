# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Architecture Overview

This is a **full-stack CRM solution** consisting of:
- **Backend**: ASP.NET Core 9.0 application built on ABP Framework (v9.3.2)
- **Frontend**: Vue 3 + TypeScript admin dashboard with Element Plus UI

### Backend Architecture (aspnetcore/)

The backend follows **Domain-Driven Design** principles with a modular architecture:

#### Core Application Projects
- **Crm.Domain.Shared** - Shared domain contracts, constants, and configurations
- **Crm.Domain** - Core domain logic, entities, and business rules
- **Crm.Application.Contracts** - Application service interfaces and DTOs
- **Crm.Application** - Application service implementations
- **Crm.EntityFrameworkCore** - Entity Framework Core data access layer
- **Crm.HttpApi** - HTTP API layer for public endpoints
- **Crm.WebApi** - Main web application hosting all services

#### Admin Module Projects
- **Crm.Admin.Application.Contracts** - Admin service interfaces and DTOs
- **Crm.Admin.Application** - Admin application service implementations
- **Crm.Admin.HttpApi** - Admin HTTP API endpoints

#### Shared Infrastructure (shared/)
- **Astra.Common** - Common utilities and extensions
- **Astra.Domain.Shared** - Shared domain infrastructure
- **Astra.Domain** - Base domain entities and patterns
- **Astra.EntityFrameworkCore** - Shared EF Core infrastructure
- **Astra.Application.Contracts** - Shared application contracts
- **Astra.Application** - Shared application services
- **Astra.AspNetCore** - ASP.NET Core integration
- **Astra.Host.Shared** - Shared hosting infrastructure

#### Test Projects (test/)
- **Crm.TestBase** - Base test infrastructure
- **Crm.Domain.Tests** - Domain layer tests
- **Crm.Application.Tests** - Application layer tests
- **Crm.EntityFrameworkCore.Tests** - Data access tests

### Frontend Architecture (vue/admin/)

#### Vue 3 Admin Dashboard
- **Vue 3** with Composition API and TypeScript
- **Element Plus** UI component library
- **Pinia** for state management with persistence
- **Vue Router** for routing with guards
- **Vue I18n** for internationalization
- **Vite** as build tool

#### Frontend Structure
- `src/api/` - API service layer and HTTP client configuration
- `src/components/` - Reusable Vue components
- `src/views/` - Page-level Vue components
- `src/store/` - Pinia stores
- `src/router/` - Vue Router configuration
- `src/utils/` - Utility functions and helpers
- `src/types/` - TypeScript type definitions

## Development Commands

### Backend (ASP.NET Core)

#### Build Commands
```bash
# Navigate to backend directory
cd aspnetcore

# Build entire solution
dotnet build Crm.sln

# Build specific project
dotnet build src/Crm.Domain/Crm.Domain.csproj

# Clean solution
dotnet clean Crm.sln

# Restore packages
dotnet restore Crm.sln
```

#### Test Commands
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

#### Database Commands
```bash
# Add migration (from Crm.EntityFrameworkCore project)
dotnet ef migrations add MigrationName --project src/Crm.EntityFrameworkCore/Crm.EntityFrameworkCore.csproj --startup-project src/Crm.WebApi/Crm.WebApi.csproj

# Update database
dotnet ef database update --project src/Crm.EntityFrameworkCore/Crm.EntityFrameworkCore.csproj --startup-project src/Crm.WebApi/Crm.WebApi.csproj

# Remove last migration
dotnet ef migrations remove --project src/Crm.EntityFrameworkCore/Crm.EntityFrameworkCore.csproj
```

### Frontend (Vue Admin)

#### Development Commands
```bash
# Navigate to frontend directory
cd vue/admin

# Install dependencies
pnpm install

# Start development server
pnpm run dev

# Build for production
pnpm run build

# Preview production build
pnpm run serve
```

#### Code Quality Commands
```bash
# Run ESLint
pnpm run lint

# Fix ESLint issues
pnpm run fix

# Format code with Prettier
pnpm run lint:prettier

# Fix CSS/SCSS styles
pnpm run lint:stylelint

# Run lint-staged (pre-commit)
pnpm run lint:lint-staged
```

#### Git Commands
```bash
# Interactive commit with commitizen
pnpm run commit

# Setup husky hooks
pnpm run prepare
```

## Key Technologies

### Backend Stack
- **.NET 9.0** with C# 12
- **ABP Framework 9.3.2** for modular architecture
- **Entity Framework Core 9.0** with PostgreSQL
- **AutoMapper** for object mapping
- **MailKit** for email functionality
- **ConfigureAwait.Fody** for async optimization

### Frontend Stack
- **Vue 3.5.12** with Composition API
- **TypeScript 5.6.3** for type safety
- **Element Plus 2.10.2** UI components
- **Pinia 3.0.2** state management
- **Vue Router 4.4.2** routing
- **Vue I18n 9.14.0** internationalization
- **Vite 6.1.0** build tool
- **Axios** for HTTP requests

## Architectural Patterns

### Backend Patterns
- **Domain-Driven Design (DDD)** with clear bounded contexts
- **Clean Architecture** with separation of concerns
- **CQRS Pattern** for application services
- **Repository Pattern** for data access abstraction
- **Dependency Injection** throughout the application
- **Modular Design** with ABP framework modules

### Frontend Patterns
- **Composition API** with `<script setup>` syntax
- **Pinia stores** for global state management
- **Vue Router** with route guards for authentication
- **Auto-imports** for components and composables
- **Component composition** with Element Plus

## Important Conventions

### Backend Conventions
- Follow ABP Framework naming conventions
- Use async/await pattern for I/O operations
- Implement proper error handling with ABP exception system
- Use Entity Framework Core code-first migrations
- Follow DDD principles for entity and aggregate design
- Use AutoMapper for DTO mapping

### Frontend Conventions
- Use Composition API with `<script setup>` syntax
- Follow Element Plus component naming conventions
- Leverage auto-imports for Vue and Element Plus APIs
- Use Pinia stores with persisted state
- Implement route guards for authentication and permissions
- Follow TypeScript best practices

## Development Workflow

### Adding New Features
1. **Backend**: Start with domain entities in `Crm.Domain`
2. **Backend**: Create DTOs in `Crm.Application.Contracts`
3. **Backend**: Implement application services in `Crm.Application`
4. **Backend**: Add API controllers in `Crm.HttpApi` or `Crm.Admin.HttpApi`
5. **Frontend**: Create API services in `src/api/`
6. **Frontend**: Implement views and components
7. **Frontend**: Update state management in Pinia stores
8. **Testing**: Add unit and integration tests

### Database Changes
1. Modify domain entities in `Crm.Domain`
2. Create EF Core migration
3. Update database
4. Update related DTOs and services
5. Test data access layer

## Configuration

### Environment Variables
- Backend configuration in `aspnetcore/src/Crm.WebApi/appsettings.json`
- Frontend environment files in `vue/admin/.env*`
- Database connection strings and API URLs configured per environment

### Code Quality
- **Backend**: Uses `common.props` for consistent project configuration
- **Frontend**: ESLint, Prettier, Stylelint, and Husky for code quality
- **Git**: Conventional commits with commitizen
- **Testing**: xUnit for backend, Vue Test Utils for frontend

## Solution Structure Notes

- The solution uses a **dual-layer architecture** with separate public and admin APIs
- **Astra framework** provides shared infrastructure and base patterns
- **Admin modules** follow the same layered architecture as core modules
- **Frontend** is a single-page application that consumes both APIs
- **PostgreSQL** is the primary database with EF Core migrations
- views 中的文件夹以烤串命名法命名