# Alma ERP Architecture

## Overview

Alma ERP follows a layered, modular architecture designed for scalability, maintainability, and extensibility. The architecture adheres to SOLID principles and separates concerns across multiple layers.

## Architecture Diagram

```
┌─────────────────────────────────────────────────────────────┐
│                     Presentation Layer                       │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐      │
│  │  HR Module   │  │Attendance    │  │ Parameters   │      │
│  │     UI       │  │ Module UI    │  │ Module UI    │      │
│  └──────────────┘  └──────────────┘  └──────────────┘      │
│           │                │                │                │
│           └────────────────┼────────────────┘                │
│                            ▼                                 │
│  ┌─────────────────────────────────────────────────────┐   │
│  │         Application Controllers (XAF)              │   │
│  └─────────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────────┘
                            ▼
┌─────────────────────────────────────────────────────────────┐
│                    Business Logic Layer                      │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐      │
│  │IEmployeeServ.│  │IAttendanceServ│  │ISettingsServ.│      │
│  └──────────────┘  └──────────────┘  └──────────────┘      │
│           │                │                │                │
│           └────────────────┼────────────────┘                │
│                            ▼                                 │
│  ┌─────────────────────────────────────────────────────┐   │
│  │         Service Implementations                    │   │
│  └─────────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────────┘
                            ▼
┌─────────────────────────────────────────────────────────────┐
│                    Data Access Layer                         │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐      │
│  │  Repository  │  │  Repository  │  │  Repository  │      │
│  │  <Employee>  │  │ <Attendance> │  │ <Settings>   │      │
│  └──────────────┘  └──────────────┘  └──────────────┘      │
│           │                │                │                │
│           └────────────────┼────────────────┘                │
│                            ▼                                 │
│  ┌─────────────────────────────────────────────────────┐   │
│  │              DbContext (XPO)                       │   │
│  └─────────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────────┘
                            ▼
┌─────────────────────────────────────────────────────────────┐
│                   Database Layer                            │
│  ┌─────────────────────────────────────────────────────┐   │
│  │              SQLite Database (ERP.db)              │   │
│  └─────────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────────┘
```

## Layer Responsibilities

### 1. Presentation Layer

**Responsibility**: User interface and interaction

**Components**:
- XAF Views (ListView, DetailView)
- Controllers for UI behavior
- Form layouts and customizations
- Data validation rules

**Guidelines**:
- Keep UI logic minimal
- Delegate business logic to services
- Use data binding for automatic updates
- Implement custom controllers for complex behavior

**Example**:
```csharp
public class EmployeeViewController : ViewController<Employee>
{
    public override void OnViewControlsCreated()
    {
        base.OnViewControlsCreated();
        // Configure list view, add actions, etc.
    }
}
```

### 2. Business Logic Layer

**Responsibility**: Core business rules and workflows

**Components**:
- Service interfaces (I*Service)
- Service implementations
- Business rules and validations
- Data transformations
- Cross-module interactions

**Guidelines**:
- Define clear service interfaces
- Keep services focused (single responsibility)
- Use dependency injection
- Implement async operations
- Use DTOs for data transfer

**Example**:
```csharp
public interface IEmployeeService
{
    Task<Employee> GetByIdAsync(int id);
    Task<Employee> CreateAsync(CreateEmployeeRequest request);
    Task<Employee> UpdateAsync(int id, UpdateEmployeeRequest request);
    Task DeleteAsync(int id);
}
```

### 3. Data Access Layer

**Responsibility**: Database operations and persistence

**Components**:
- Repository interfaces (IRepository<T>)
- Repository implementations
- DbContext configuration
- Query optimization
- Migrations and schema management

**Guidelines**:
- Use repository pattern for data access
- Abstract database implementation
- Support async queries
- Implement query optimization
- Handle transactions properly

**Example**:
```csharp
public interface IRepository<T> where T : BaseEntity
{
    Task<T> GetByIdAsync(int id);
    Task<IList<T>> GetAllAsync();
    Task<IList<T>> FindAsync(Func<T, bool> predicate);
    Task<T> AddAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task DeleteAsync(int id);
}
```

### 4. Database Layer

**Responsibility**: Data persistence

**Components**:
- SQLite database file (ERP.db)
- Database schema
- Indexes
- Constraints

**Characteristics**:
- File-based, no server required
- Automatic creation on first run
- Automatic migrations
- Full-text search support (future)

## Cross-Cutting Concerns

### Authentication & Authorization

**Location**: AlmaERP.Module.Security

**Components**:
- IAuthenticationService
- AuthenticationService
- SecurityStrategyComplex
- PasswordHasher

**Flow**:
1. User enters credentials
2. AuthenticationService validates
3. If valid, SecurityStrategy creates user context
4. Authorization checks permissions for actions

**Example**:
```csharp
var user = await _authService.AuthenticateAsync(matricule, password);
if (user != null && user.IsActive)
{
    _securityStrategy.SetUserContext(user);
}
```

### Logging

**Location**: Application-wide

**Types**:
- **Audit Logging**: Track all CRUD operations
- **Error Logging**: Capture exceptions
- **Performance Logging**: Monitor slow operations
- **Security Logging**: Track login attempts, access denials

**Implementation**:
```csharp
_logger.LogInformation("User {UserId} logged in", userId);
_logger.LogError("Error processing attendance: {Error}", ex.Message);
```

### Error Handling

**Strategy**:
- Catch exceptions at service layer
- Convert to meaningful error messages
- Log all errors
- Return error responses to UI

**Example**:
```csharp
try
{
    // Business logic
}
catch (DbUpdateException ex)
{
    _logger.LogError(ex, "Database error");
    throw new ServiceException("Failed to save data", ex);
}
```

### Dependency Injection

**Container**: Microsoft.Extensions.DependencyInjection

**Configuration** (in Program.cs):
```csharp
services.AddScoped<IEmployeeService, EmployeeService>();
services.AddScoped<IAttendanceService, AttendanceService>();
services.AddScoped<ISettingsService, SettingsService>();
services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
```

### Caching Strategy

**Levels**:
1. **Object-Level Cache**: XPO built-in caching
2. **Service-Level Cache**: In-memory cache for frequently accessed data
3. **Query Cache**: Cache for expensive queries

**Implementation**:
```csharp
private readonly IMemoryCache _cache;

public async Task<IList<Employee>> GetAllAsync()
{
    const string cacheKey = "employees_all";
    if (_cache.TryGetValue(cacheKey, out var employees))
    {
        return (IList<Employee>)employees;
    }

    var result = await _repository.GetAllAsync();
    _cache.Set(cacheKey, result, TimeSpan.FromHours(1));
    return result;
}
```

## Module Architecture

### Module Structure

Each module follows this structure:

```
AlmaERP.Module.ModuleName/
├── BusinessObjects/
│   ├── Entity1.cs
│   ├── Entity2.cs
│   └── ...
├── Services/
│   ├── IService1.cs
│   ├── Service1.cs
│   ├── IService2.cs
│   └── Service2.cs
├── Controllers/
│   ├── ListViewController.cs
│   ├── DetailViewController.cs
│   └── CustomController.cs
├── DTOs/
│   ├── CreateRequest.cs
│   ├── UpdateRequest.cs
│   └── Response.cs
├── Views/
│   ├── Entity1_ListView.xafml
│   ├── Entity1_DetailView.xafml
│   └── ...
└── ModuleInitializer.cs
```

### Module Registration

**In Program.cs**:
```csharp
var application = new WinApplication();
application.Modules.Add(new AlmaERP.Module.HR.ModuleInitializer());
application.Modules.Add(new AlmaERP.Module.Attendance.ModuleInitializer());
application.Modules.Add(new AlmaERP.Module.Parameters.ModuleInitializer());
```

### Module Independence

Modules are independent and communicate through:
1. **Service Interfaces**: Well-defined contracts
2. **Events**: Loosely coupled communication
3. **Shared Entities**: Common base classes

**Example**:
```csharp
// HR module defines employee
public interface IEmployeeService
{
    Task<Employee> GetByIdAsync(int id);
}

// Attendance module uses employee
public class AttendanceService
{
    private readonly IEmployeeService _employeeService;

    public AttendanceService(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    public async Task RecordAttendanceAsync(int employeeId, DateTime date)
    {
        var employee = await _employeeService.GetByIdAsync(employeeId);
        // Record attendance for employee
    }
}
```

## Data Flow

### Typical Read Operation

1. **User Action**: Click on employee list
2. **Controller**: EmployeeListViewController handles the action
3. **Service Layer**: Calls IEmployeeService.GetAllAsync()
4. **Repository Layer**: Calls IRepository<Employee>.GetAllAsync()
5. **Database**: Executes query against SQLite
6. **Result**: Returns to UI for display

### Typical Write Operation

1. **User Action**: Click save on new employee
2. **Controller**: EmployeeDetailViewController captures input
3. **Validation**: Check business rules
4. **Service Layer**: Calls IEmployeeService.CreateAsync(request)
5. **Repository Layer**: Calls IRepository<Employee>.AddAsync(entity)
6. **Database**: Inserts record into Employee table
7. **Transaction**: Commits changes
8. **Notification**: Updates UI
9. **Logging**: Logs the operation

## Extension Points

### Adding a New Module

1. Create new project: AlmaERP.Module.NewModule
2. Implement business objects
3. Implement services
4. Create controllers and views
5. Register module in Program.cs
6. Add migrations if needed

### Customizing Existing Module

1. Create custom controller
2. Override methods as needed
3. Register custom controller
4. No changes to core module required

### Adding New Entity Type

1. Create entity in BusinessObjects
2. Create service interface and implementation
3. Create repository if needed
4. Create views
5. Create migrations

## Performance Considerations

### Query Optimization

- Use indexes on frequently queried columns
- Implement pagination for large datasets
- Use lazy loading for related entities
- Cache frequently accessed data

### Memory Management

- Dispose repositories properly
- Use using statements for DbContext
- Implement object pooling if needed
- Monitor memory usage in long-running processes

### Scalability

Current design supports:
- Small company operations (< 1000 employees)
- Monthly operations
- Multi-user concurrent access (up to 50 users)

For larger scale:
- Migrate to SQL Server or PostgreSQL
- Implement service caching layer
- Add API layer for distributed access
- Implement event-driven architecture

## Security Architecture

### Authentication Flow

```
Login Screen
    ↓
Validate Credentials
    ↓
Hash Password & Compare
    ↓
Create Security Context
    ↓
Load User Permissions
    ↓
Main Application
```

### Authorization Model

- **Role-Based Access Control (RBAC)**
- **Roles**: Administrator, RH, Manager, User
- **Permissions**: Assigned to roles
- **Resources**: Controlled by permissions

### Data Security

- **Passwords**: PBKDF2 hashing
- **Connections**: Local file, encrypted if needed
- **Audit Trail**: All operations logged
- **Access Control**: Row-level security possible

## Future Enhancements

### Short-term (3-6 months)

- RESTful API integration
- Mobile application
- Advanced reporting
- Batch operations

### Medium-term (6-12 months)

- Multi-tenant support
- Cloud synchronization
- Document management
- Workflow automation

### Long-term (12+ months)

- Machine learning for insights
- Real-time analytics
- IoT integration
- Blockchain for audit trail
