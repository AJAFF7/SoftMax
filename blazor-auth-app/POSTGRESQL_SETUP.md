# PostgreSQL Setup Guide for Blazor Auth App

## Current Status

Your Blazor Auth App now uses **PostgreSQL** database instead of browser localStorage!

###  Architecture

- **Frontend**: Blazor WebAssembly (Client-side)
- **Backend API**: ASP.NET Core Web API (.NET 10.0)
- **Database**: PostgreSQL

## Setup Instructions

### 1. Install PostgreSQL

**macOS** (using Homebrew):
```bash
brew install postgresql@17
brew services start postgresql@17
```

**Windows**:
Download and install from: https://www.postgresql.org/download/windows/

**Linux** (Ubuntu/Debian):
```bash
sudo apt update
sudo apt install postgresql postgresql-contrib
sudo systemctl start postgresql
```

### 2. Create Database User and Database

```bash
# Connect to PostgreSQL
psql postgres

# Create a new user (replace 'your_password' with your password)
CREATE USER blazoruser WITH PASSWORD 'your_password';

# Create the database
CREATE DATABASE blazorauthdb;

# Grant privileges
GRANT ALL PRIVILEGES ON DATABASE blazorauthdb TO blazoruser;

# Exit
\q
```

### 3. Update Connection String

Edit `BlazorAuthApp.Api/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=blazorauthdb;Username=blazoruser;Password=your_password"
  }
}
```

### 4. Create Database Migration

```bash
cd BlazorAuthApp.Api
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### 5. Update Blazor Client Configuration

Edit `BlazorAuthApp/wwwroot/appsettings.json`:

```json
{
  "ApiBaseUrl": "https://localhost:7001"
}
```

(Adjust the port based on your API's launchSettings.json)

### 6. Run the Application

**Terminal 1 - Start the API:**
```bash
cd BlazorAuthApp.Api
dotnet run
```

**Terminal 2 - Start the Blazor Client:**
```bash
cd BlazorAuthApp
dotnet run
```

## API Endpoints

- **POST** `/api/auth/register` - Register a new user
- **POST** `/api/auth/login` - Login with username/password
- **DELETE** `/api/auth/user/{username}` - Delete a user

## Database Schema

### Users Table

| Column | Type | Constraints |
|--------|------|-------------|
| Id | int | Primary Key, Auto-increment |
| Username | varchar(100) | NOT NULL, UNIQUE |
| PasswordHash | text | NOT NULL |
| CreatedAt | timestamp | NOT NULL, Default: UTC Now |

## Security Notes

⚠️ **Important**: This is a demo implementation. For production:

1. **Use proper authentication** (JWT tokens, OAuth, Identity Server)
2. **Use bcrypt or Argon2** for password hashing (currently using SHA256)
3. **Add HTTPS** enforcement
4. **Implement rate limiting**
5. **Add input validation**
6. **Use environment variables** for sensitive configuration
7. **Add proper error handling** and logging
8. **Implement authorization** policies

## Troubleshooting

**Connection Issues:**
- Verify PostgreSQL is running: `psql -U postgres -c "SELECT version();"`
- Check port 5432 is available: `lsof -i :5432`
- Verify connection string in appsettings.json

**Migration Issues:**
- Ensure EF Core tools are installed: `dotnet tool install --global dotnet-ef`
- Clean and rebuild: `dotnet clean && dotnet build`
- Remove migrations folder and recreate if needed

**CORS Issues:**
- Verify the Blazor client URL matches the CORS policy in Program.cs
- Check browser console for CORS-related errors
