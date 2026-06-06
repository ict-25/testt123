# Alma ERP Development Guide

## Getting Started

### Prerequisites

1. Visual Studio 2022 or later
2. .NET 8 SDK
3. DevExpress XAF (Trial or Licensed)
4. Git

### Initial Setup

1. Clone the repository
2. Open AlmaERP.sln in Visual Studio
3. Restore NuGet packages
4. Set AlmaDesktop.Win as startup project
5. Build solution
6. Run application

## Project Organization

### Core Projects

#### AlmaDesktop.Win
- **Purpose**: WinForms application host
- **Responsibility**: Application initialization, splash screen, login form
- **Key Files**:
  - Program.cs: Application entry point
  - SplashForm.cs: Splash screen
  - LoginForm.cs: Login screen

#### AlmaDesktop.Modules
- **Purpose**: Base module definitions
- **Responsibility**: Module registration and initialization

#### AlmaERP.Module.BusinessObjects
- **Purpose**: Domain model entities
- **Responsibility**: Define all business entities
- **Key Entities**:
  - User
  - Employee
  - AttendanceMonth
  - AttendanceEmployee
  - AttendanceDay
  - Chantier
  - SystemSettings

#### AlmaERP.Module.Services
- **Purpose**: Business logic implementation
- **Responsibility**: Service interfaces and implementations
- **Key Services**:
  - IAuthenticationService
  - IEmployeeService
  - IAttendanceService
  - IChantierService
  - ISettingsService

#### AlmaERP.Module.Security
- **Purpose**: Authentication and authorization
- **Responsibility**: User management, role-based access control
- **Key Components**:
  - SecurityStrategyComplex
  - CustomAuthenticationProvider
  - PasswordHasher

#### AlmaERP.Module.Database
- **Purpose**: Database configuration and migrations
- **Responsibility**: SQLite setup, schema definition, seed data
- **Key Files**:
  - DbContext.cs
  - Migrations/
  - DatabaseInitializer.cs

#### AlmaERP.Module.API
- **Purpose**: Future API integration
- **Responsibility**: REST client, API configuration
- **Key Components**:
  - IApiClient
  - ApiConfiguration
  - ApiClientService

## Implementation Steps

### Phase 1: Foundation (Week 1)

1. **Project Structure**
   - Create all project files
   - Configure dependencies
   - Set up NuGet packages

2. **Database**
   - Create DbContext
   - Define all entities
   - Create migrations
   - Seed admin user

3. **Security**
   - Implement password hashing
   - Create authentication service
   - Configure XAF security

4. **Application Startup**
   - Implement splash screen
   - Create login form
   - Configure DI container

### Phase 2: Core Modules (Week 2)

1. **HR Module**
   - Employee entity and repository
   - Employee service
   - Employee list view
   - Employee detail view

2. **Attendance Module**
   - Attendance entities
   - Attendance service
   - Attendance UI with dynamic day checkboxes
   - Auto-save mechanism

3. **Parameters Module**
   - System settings entity
   - Chantier entity
   - Settings service
   - Settings UI

### Phase 3: Polish & Documentation (Week 3)

1. **UI/UX**
   - Implement main navigation
   - Create tab-based layout
   - Configure XAF list and detail views

2. **Logging**
   - Implement audit logging
   - Configure file logging
   - Log important events

3. **Testing & Documentation**
   - Unit tests for services
   - Integration tests
   - User documentation

## Adding a New Module

### Step 1: Create Project Structure

```
AlmaERP.Module.NewModule/
├── BusinessObjects/
│   └── NewEntity.cs
├── Services/
│   ├── INewService.cs
│   └── NewService.cs
├── Controllers/
│   └── NewController.cs
└── ModuleInitializer.cs
```

### Step 2: Create Business Objects

```csharp
namespace AlmaERP.Module.NewModule.BusinessObjects
{
    [Persistent("NewEntity")]
    public class NewEntity : BaseEntity
    {
        private string _name;

        [Size(100)]
        public string Name
        {
            get => _name;
            set => SetPropertyValue(nameof(Name), ref _name, value);
        }
    }
}
```

### Step 3: Create Service Interface

```csharp
namespace AlmaERP.Module.NewModule.Services
{
    public interface INewService
    {
        Task<NewEntity> GetByIdAsync(int id);
        Task<IList<NewEntity>> GetAllAsync();
        Task<NewEntity> CreateAsync(NewEntity entity);
        Task<NewEntity> UpdateAsync(NewEntity entity);
        Task DeleteAsync(int id);
    }
}
```

### Step 4: Implement Service

```csharp
namespace AlmaERP.Module.NewModule.Services
{
    public class NewService : INewService
    {
        private readonly IObjectSpace _objectSpace;

        public NewService(IObjectSpace objectSpace)
        {
            _objectSpace = objectSpace;
        }

        public async Task<NewEntity> GetByIdAsync(int id)
        {
            return await Task.Run(() =>
                _objectSpace.GetObjectByKey<NewEntity>(id)
            );
        }

        // Implement other methods...
    }
}
```

### Step 5: Register Module

```csharp
namespace AlmaERP.Module.NewModule
{
    public class NewModuleInitializer : ModuleBase
    {
        public override IEnumerable<ModuleUpdater> GetModuleUpdaters(
            IObjectSpace objectSpace, Version versionFromDB)
        {
            return new[] { new DatabaseUpdater(objectSpace, versionFromDB) };
        }
    }
}
```

### Step 6: Register in DI Container

In Program.cs:

```csharp
services.AddScoped<INewService, NewService>();
```

## Database Migrations

### Creating a Migration

1. Add new entity to DbContext
2. Create migration file in Migrations/ folder
3. Implement Up() and Down() methods
4. Run migration on application startup

### Migration Template

```csharp
namespace AlmaERP.Module.Database.Migrations
{
    public class AddNewEntityMigration : ModuleUpdater
    {
        public AddNewEntityMigration(IObjectSpace objectSpace, Version currentDBVersion)
            : base(objectSpace, currentDBVersion)
        {
        }

        public override void UpdateDatabase()
        {
            // Create table, add columns, etc.
            var updater = new SchemaModifier(ObjectSpace);
            updater.AddTable<NewEntity>();
        }
    }
}
```

## Service Implementation Patterns

### Pattern 1: Repository Pattern

```csharp
public interface IRepository<T> where T : BaseEntity
{
    Task<T> GetByIdAsync(int id);
    Task<IList<T>> GetAllAsync();
    Task<T> AddAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task DeleteAsync(int id);
}

public class GenericRepository<T> : IRepository<T> where T : BaseEntity
{
    private readonly IObjectSpace _objectSpace;

    public GenericRepository(IObjectSpace objectSpace)
    {
        _objectSpace = objectSpace;
    }

    // Implement methods...
}
```

### Pattern 2: Service Layer

```csharp
public interface IMyService
{
    Task<MyEntity> GetByIdAsync(int id);
    Task<IList<MyEntity>> SearchAsync(SearchCriteria criteria);
    Task<MyEntity> CreateAsync(CreateMyEntityRequest request);
}

public class MyService : IMyService
{
    private readonly IRepository<MyEntity> _repository;
    private readonly IMapper _mapper;

    public MyService(IRepository<MyEntity> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    // Implement methods...
}
```

## Configuration Files

### Application Settings

Create `appsettings.json`:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  },
  "Database": {
    "ConnectionString": "Data Source=ERP.db;Version=3;",
    "AutoMigrate": true
  },
  "Security": {
    "PasswordExpiration": 90,
    "LockoutThreshold": 5,
    "SessionTimeout": 3600
  },
  "Api": {
    "BaseUrl": "https://api.example.com",
    "Timeout": 30
  }
}
```

## Common Tasks

### Task 1: Add New Entity

1. Create entity class in BusinessObjects/
2. Add DbSet to DbContext
3. Create migration
4. Create service interface and implementation
5. Register service in DI
6. Create XAF view (if needed)

### Task 2: Add New Feature

1. Define feature in module
2. Create service interface
3. Implement service
4. Create controller
5. Create views
6. Register views in module

### Task 3: Modify Database Schema

1. Update entity class
2. Create migration
3. Update service if needed
4. Update views if needed

## Testing

### Unit Tests

```csharp
[TestFixture]
public class EmployeeServiceTests
{
    private Mock<IRepository<Employee>> _mockRepository;
    private EmployeeService _service;

    [SetUp]
    public void Setup()
    {
        _mockRepository = new Mock<IRepository<Employee>>();
        _service = new EmployeeService(_mockRepository.Object);
    }

    [Test]
    public async Task GetByIdAsync_WithValidId_ReturnsEmployee()
    {
        // Arrange
        var id = 1;
        var employee = new Employee { Id = id, Matricule = "E001" };
        _mockRepository.Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync(employee);

        // Act
        var result = await _service.GetByIdAsync(id);

        // Assert
        Assert.That(result.Id, Is.EqualTo(id));
        _mockRepository.Verify(r => r.GetByIdAsync(id), Times.Once);
    }
}
```

## Troubleshooting

### Issue: Database not created

**Solution**: Check connection string and ensure SQLite provider is registered.

### Issue: Login fails

**Solution**: Verify admin user is seeded, check password hash implementation.

### Issue: Attendance auto-save not working

**Solution**: Ensure transaction management is correct, check ObjectSpace configuration.

## Performance Tips

1. Use async/await for I/O operations
2. Implement caching for frequently accessed data
3. Use lazy loading for large collections
4. Optimize database queries
5. Implement pagination for list views

## Security Best Practices

1. Never store plaintext passwords
2. Hash passwords with PBKDF2 or bcrypt
3. Validate all user input
4. Use parameterized queries
5. Implement rate limiting for login attempts
6. Log all security events
7. Force HTTPS for API communication

## Resources

- [DevExpress XAF Documentation](https://docs.devexpress.com/eXpressAppFramework/)
- [Microsoft .NET Documentation](https://docs.microsoft.com/dotnet/)
- [SQLite Documentation](https://www.sqlite.org/docs.html)
- [SOLID Principles](https://en.wikipedia.org/wiki/SOLID)
