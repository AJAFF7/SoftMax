# Quick Fix: Database Connection Error

## Problem
The migration failed because PostgreSQL user doesn't exist.

## Quick Solution (Choose ONE)

### Option 1: Use Your Mac Username (Easiest for macOS)
If you're on macOS with Homebrew PostgreSQL, try this:

1. **Create the database using your Mac username:**
   ```bash
   createdb blazorauthdb
   ```

2. **Update `BlazorAuthApp.Api/appsettings.json`:**
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=localhost;Port=5432;Database=blazorauthdb;Username=jaff;Password="
     }
   }
   ```
   (Replace `jaff` with your actual Mac username if different)

3. **Run migration:**
   ```bash
   cd BlazorAuthApp.Api
   dotnet ef database update
   ```

### Option 2: Create PostgreSQL User (Recommended for Production)

1. **Connect to PostgreSQL:**
   ```bash
   psql postgres
   ```

2. **Create user and database (in psql):**
   ```sql
   CREATE USER blazoruser WITH PASSWORD 'BlazorPass123!';
   CREATE DATABASE blazorauthdb OWNER blazoruser;
   GRANT ALL PRIVILEGES ON DATABASE blazorauthdb TO blazoruser;
   \q
   ```

3. **Update `BlazorAuthApp.Api/appsettings.json`:**
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=localhost;Port=5432;Database=blazorauthdb;Username=blazoruser;Password=BlazorPass123!"
     }
   }
   ```

4. **Run migration:**
   ```bash
   cd BlazorAuthApp.Api
   dotnet ef database update
   ```

### Option 3: Use postgres Superuser

1. **If postgres user exists, create database:**
   ```bash
   psql -U postgres -c "CREATE DATABASE blazorauthdb;"
   ```

2. **Update `BlazorAuthApp.Api/appsettings.json`:**
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=localhost;Port=5432;Database=blazorauthdb;Username=postgres;Password=your_postgres_password"
     }
   }
   ```

3. **Run migration:**
   ```bash
   cd BlazorAuthApp.Api
   dotnet ef database update
   ```

## After Database Setup

Once the database is set up and migration succeeds, you can start the application:

**Terminal 1 - API:**
```bash
cd BlazorAuthApp.Api
dotnet run
```

**Terminal 2 - Blazor App:**
```bash
cd BlazorAuthApp
dotnet watch run
```

## Check PostgreSQL Status

**macOS (Homebrew):**
```bash
brew services list | grep postgresql
# Start if not running:
brew services start postgresql@17
```

**Linux:**
```bash
sudo systemctl status postgresql
# Start if not running:
sudo systemctl start postgresql
```

**All platforms - Test connection:**
```bash
psql -l  # List all databases
```
