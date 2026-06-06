# Alma ERP - Modular Enterprise Resource Planning System

## Project Overview

Alma ERP is a modern, modular desktop application built with DevExpress XAF, C#, and .NET 8. It provides comprehensive business management capabilities with a focus on HR, Attendance, and Parameters management, with extensibility for future modules.

### Key Features

- **Modular Architecture**: Easily add new modules without modifying existing code
- **Authentication & Security**: Role-based access control with secure password hashing
- **HR Management**: Complete employee lifecycle management
- **Attendance Tracking**: Monthly attendance management with auto-save
- **Parameters Management**: Company settings and Chantier (site) management
- **SQLite Database**: Lightweight, file-based database with automatic migrations
- **Future API Ready**: Infrastructure prepared for REST API integration
- **Logging**: Comprehensive audit trail for all operations

## Technology Stack

- **Framework**: .NET 8
- **UI Framework**: DevExpress XAF (eXpressApp Framework)
- **UI Platform**: WinForms
- **Database**: SQLite
- **Language**: C#
- **Dependency Injection**: Built-in .NET DI container
- **Logging**: Serilog

## Project Structure

```
AlmaERP/
├── AlmaDesktop.Win/                    # WinForms Application Host
├── AlmaDesktop.Modules/                # Module definitions
├── AlmaERP.Module.BusinessObjects/     # Domain models
├── AlmaERP.Module.Services/            # Business logic layer
├── AlmaERP.Module.Controllers/         # XAF Controllers
├── AlmaERP.Module.Security/            # Security & Authentication
├── AlmaERP.Module.API/                 # Future API layer
├── AlmaERP.Module.Database/            # Database configuration & migrations
├── AlmaERP.Module.Common/              # Shared utilities & constants
├── AlmaERP.Module.HR/                  # HR Module
├── AlmaERP.Module.Attendance/          # Attendance Module
├── AlmaERP.Module.Parameters/          # Parameters Module
└── Documentation/                      # Implementation guides
```

## Getting Started

### Prerequisites

- .NET 8 SDK or later
- Visual Studio 2022 or later
- DevExpress XAF licenses/trial
- SQLite (included via NuGet)

### Installation

1. Clone the repository
2. Restore NuGet packages
3. Build the solution
4. Run AlmaDesktop.Win

### Default Credentials

- **Matricule**: admin
- **Password**: admin123

*Note: You will be prompted to change the password on first login.*

## Architecture Principles

- **Modular Design**: Each module is independently deployable
- **Separation of Concerns**: Clear separation between UI, business logic, and data layers
- **SOLID Principles**: Following industry best practices
- **Dependency Injection**: All dependencies managed through DI container
- **Service-Oriented**: Business logic exposed through well-defined service interfaces
- **Repository Pattern**: Data access abstraction
- **Async Operations**: Non-blocking, responsive UI

## Module Structure

Each module follows this pattern:

```
AlmaERP.Module.ModuleName/
├── BusinessObjects/        # Domain entities
├── Services/               # Service interfaces & implementations
├── Controllers/            # XAF controllers
├── Views/                  # UI definitions (if needed)
└── ModuleInitializer.cs    # Module registration
```

## Key Modules

### HR Module
- Employee Management (CRUD)
- Employee Details & Photos
- Search & Filtering
- Status Management

### Attendance Module
- Monthly Attendance Records
- Employee Attendance Tracking
- Auto-save Functionality
- Attendance Reports

### Parameters Module
- Company Settings
- Chantier (Site) Management
- General Configuration
- UI Preferences

## Database

### SQLite Configuration

- Database file: `ERP.db` (in application data folder)
- Automatic creation on first run
- Automatic schema migrations
- Seed data for admin user

### Connection String

```
Data Source=ERP.db;Version=3;
```

## Security

### Authentication

- Matricule + Password based login
- Passwords hashed with PBKDF2
- Session-based authentication
- Force password change on first login

### Authorization

- Role-based access control (RBAC)
- Roles: Administrator, RH, Manager, User
- Module-level permissions
- Operation-level permissions

## Logging

Logs are stored in:
- **Database**: Audit trail for CRUD operations
- **File**: Application logs in `Logs/` directory

Events logged:
- Application startup
- User login/logout
- All CRUD operations
- Errors and exceptions

## Future Modules (Ready for Implementation)

- Stock Management
- Purchasing
- Sales
- Accounting
- CRM
- Vehicle Management
- Document Management

The architecture supports adding these modules without modifying existing code.

## API Integration

The application is designed to work offline initially. API integration infrastructure is prepared for:

- REST endpoints
- Authentication tokens
- Data synchronization
- Offline-first architecture

## Coding Standards

- Follow SOLID principles
- Use async/await for I/O operations
- Meaningful variable and method names
- XML documentation for public APIs
- Unit tests for business logic

## Development Guidelines

1. **Add New Module**: See `DEVELOPMENT.md`
2. **Add New Entity**: Follow entity template in BusinessObjects
3. **Add New Service**: Implement IService interface
4. **Database Changes**: Create migration in Database module

## Support & Documentation

See the `Documentation/` folder for:
- Step-by-step implementation guides
- Architecture decisions
- Database schema
- API specifications

## License

Internal Use Only

## Version

Version 1.0.0 (Initial Release)
