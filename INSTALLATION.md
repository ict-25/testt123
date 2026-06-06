# Installation Guide - Alma ERP

## System Requirements

### Minimum Requirements

- **Operating System**: Windows 7 SP1 or later (Windows 10/11 recommended)
- **Processor**: Intel Core 2 or AMD equivalent, 1 GHz or faster
- **RAM**: 2 GB
- **Disk Space**: 500 MB free space
- **Display**: 1024 x 768 or higher

### Recommended Requirements

- **Operating System**: Windows 10 or Windows 11
- **Processor**: Intel Core i5 or better
- **RAM**: 8 GB or more
- **Disk Space**: 2 GB free space
- **Display**: 1920 x 1080 or higher

### Software Requirements

- **.NET 8 Runtime**: https://dotnet.microsoft.com/download/dotnet/8.0
- **Visual Studio 2022** (for development): https://visualstudio.microsoft.com/
- **DevExpress XAF**: Trial or Licensed version

## Installation Steps

### Step 1: Install .NET 8 Runtime

1. Download .NET 8 Runtime from: https://dotnet.microsoft.com/download/dotnet/8.0
2. Choose "Desktop Runtime" for Windows x64 or x86
3. Run the installer
4. Follow the installation wizard
5. Verify installation:
   ```bash
   dotnet --version
   ```

### Step 2: Clone Repository

1. Open Git Bash or Command Prompt
2. Navigate to desired folder:
   ```bash
   cd C:\Development
   ```
3. Clone the repository:
   ```bash
   git clone https://github.com/ict-25/testt123.git
   cd testt123
   ```

### Step 3: Restore NuGet Packages

#### Option A: Using Visual Studio

1. Open AlmaERP.sln in Visual Studio 2022
2. Build → Restore NuGet Packages
3. Wait for completion

#### Option B: Using Command Line

```bash
dotnet restore AlmaERP.sln
```

### Step 4: Install DevExpress Dependencies

1. Ensure you have DevExpress NuGet feed configured
2. In Visual Studio:
   - Tools → Options → NuGet Package Manager → Package Sources
   - Verify DevExpress feed is added
3. Restore packages again to ensure DevExpress components are installed

### Step 5: Build Solution

#### Using Visual Studio

1. Open AlmaERP.sln
2. Build → Build Solution (or Ctrl+Shift+B)
3. Wait for build to complete without errors
4. Output should show "Build succeeded"

#### Using Command Line

```bash
dotnet build AlmaERP.sln
```

### Step 6: Run Application

1. In Visual Studio Solution Explorer, right-click on **AlmaDesktop.Win**
2. Select "Set as Startup Project"
3. Press F5 or click Debug → Start Debugging
4. Application will launch with Splash Screen
5. Database will be created automatically in:
   ```
   %APPDATA%\Alma\ERP.db
   ```

### Step 7: First Login

1. Wait for application to load
2. Login form will appear
3. Enter default credentials:
   - **Matricule**: admin
   - **Password**: admin123
4. Click Login
5. You will be prompted to change password
6. Enter new password and confirm
7. Application main window will open

## Directory Structure After Installation

```
testt123/
├── .git/
├── AlmaDesktop.Win/
├── AlmaDesktop.Modules/
├── AlmaERP.Module.BusinessObjects/
├── AlmaERP.Module.Services/
├── AlmaERP.Module.Security/
├── AlmaERP.Module.Database/
├── AlmaERP.Module.HR/
├── AlmaERP.Module.Attendance/
├── AlmaERP.Module.Parameters/
├── AlmaERP.Module.API/
├── Documentation/
├── .gitignore
├── README.md
├── DEVELOPMENT.md
├── ARCHITECTURE.md
├── INSTALLATION.md
├── AlmaERP.sln
└── packages/
```

## Database Initialization

The database is automatically created on first run:

### Location

- **Windows**: `C:\Users\{YourUsername}\AppData\Roaming\Alma\ERP.db`
- **Environment Variable**: `%APPDATA%\Alma\ERP.db`

### Automatic Initialization Process

1. Application checks if database exists
2. If not, creates new database with schema
3. Seeds default data (admin user, settings)
4. Runs any pending migrations
5. Logs initialization to file and database

### Database Backup

Regularly backup your database:

```bash
Copy-Item "$env:APPDATA\Alma\ERP.db" -Destination "$env:APPDATA\Alma\ERP_backup_$(Get-Date -Format 'yyyyMMdd_HHmmss').db"
```

## Troubleshooting

### Issue: "Could not find .NET Runtime"

**Solution**:
1. Install .NET 8 Runtime from https://dotnet.microsoft.com/download/dotnet/8.0
2. Restart computer
3. Run application again

### Issue: Build fails with "DevExpress references not found"

**Solution**:
1. Add DevExpress NuGet feed:
   - Tools → Options → NuGet Package Manager → Package Sources
   - Click + (Add new source)
   - Name: DevExpress
   - Source: https://nuget.devexpress.com/api (requires registration)
2. Restore NuGet packages
3. Rebuild solution

### Issue: Database not created

**Solution**:
1. Ensure write permissions to AppData folder
2. Check logs in: `%APPDATA%\Alma\Logs\`
3. Try creating database folder manually:
   ```bash
   mkdir %APPDATA%\Alma
   ```
4. Run application with administrator privileges

### Issue: Login fails with "Invalid credentials"

**Solution**:
1. Verify database exists at `%APPDATA%\Alma\ERP.db`
2. Check if admin user was seeded
3. Reset database:
   - Delete ERP.db
   - Restart application
   - Database will be recreated with admin user

### Issue: Application crashes on startup

**Solution**:
1. Check logs: `%APPDATA%\Alma\Logs\application.log`
2. Ensure all dependencies installed:
   ```bash
   dotnet restore AlmaERP.sln
   ```
3. Rebuild solution
4. Run with administrator privileges

### Issue: "Port already in use" (if API is implemented)

**Solution**:
1. Change port in appsettings.json
2. Or kill process using port:
   ```bash
   netstat -ano | findstr :5000
   taskkill /PID {PID} /F
   ```

## Configuration

### Application Settings

Edit `appsettings.json` to customize:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning"
    }
  },
  "Database": {
    "AutoMigrate": true,
    "SeedData": true
  },
  "Security": {
    "PasswordExpiration": 90,
    "LockoutThreshold": 5
  }
}
```

### Environment Variables

Set for custom configurations:

```bash
set ALMA_DB_PATH=C:\CustomPath\ERP.db
set ALMA_LOG_LEVEL=Debug
set ALMA_THEME=Dark
```

## Performance Tuning

### Optimize Database

1. Rebuild database indexes:
   ```sql
   REINDEX;
   ```

2. Optimize database:
   ```sql
   VACUUM;
   ```

3. Update statistics:
   ```sql
   ANALYZE;
   ```

### Application Optimization

1. Clear application cache:
   ```bash
   rmdir %APPDATA%\Alma\Cache /s /q
   ```

2. Disable unnecessary logging in appsettings.json

3. Increase buffer pool size in database configuration

## Uninstallation

### Complete Removal

1. Delete application folder:
   ```bash
   rmdir C:\Development\testt123 /s /q
   ```

2. Delete application data:
   ```bash
   rmdir %APPDATA%\Alma /s /q
   ```

3. Remove shortcuts from:
   - Desktop
   - Start Menu
   - Quick Launch

## Next Steps

1. Configure application settings
2. Add company information
3. Create Chantiers (work sites)
4. Add employees
5. Set up user accounts
6. Configure Roles and Permissions
7. Import historical data (if needed)

## Support

For issues or questions:

1. Check logs: `%APPDATA%\Alma\Logs\application.log`
2. Review documentation in Documentation/ folder
3. Check DevExpress documentation: https://docs.devexpress.com/
4. Contact development team
