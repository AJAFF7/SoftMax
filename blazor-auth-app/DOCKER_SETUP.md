# PostgreSQL Docker Setup

## Quick Start

### 1. Start PostgreSQL in Docker
```bash
docker-compose up -d
```

This will:
- Pull PostgreSQL 17 Alpine image
- Create a container named `blazor-postgres`
- Expose PostgreSQL on port 5432
- Create a persistent volume for data
- Set up database: `blazorauthdb`
- Set up user: `blazoruser` with password: `BlazorPass123!`

### 2. Verify PostgreSQL is Running
```bash
docker-compose ps
```

You should see the `blazor-postgres` container with status "Up" and healthy.

### 3. Run Database Migrations
```bash
cd BlazorAuthApp.Api
dotnet ef database update
```

This will create all the necessary tables (Users, Doctors, Appointments).

### 4. Start the Application

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

---

## Docker Commands

### Start PostgreSQL
```bash
docker-compose up -d
```

### Stop PostgreSQL
```bash
docker-compose down
```

### Stop and Remove Data (Fresh Start)
```bash
docker-compose down -v
```

### View Logs
```bash
docker-compose logs -f postgres
```

### Connect to PostgreSQL CLI
```bash
docker exec -it blazor-postgres psql -U blazoruser -d blazorauthdb
```

### Check Container Status
```bash
docker-compose ps
```

---

## Connection Details

- **Host**: localhost
- **Port**: 5432
- **Database**: blazorauthdb
- **Username**: blazoruser
- **Password**: BlazorPass123!

The connection string in `appsettings.json` is already configured:
```
Host=localhost;Port=5432;Database=blazorauthdb;Username=blazoruser;Password=BlazorPass123!
```

---

## Troubleshooting

### Port Already in Use
If port 5432 is already in use, you can change it in `docker-compose.yml`:
```yaml
ports:
  - "5433:5432"  # Use port 5433 on host instead
```

Then update the connection string to use port 5433.

### Container Won't Start
```bash
# Check logs
docker-compose logs postgres

# Remove old containers and volumes
docker-compose down -v
docker-compose up -d
```

### Reset Database
```bash
# Stop and remove all data
docker-compose down -v

# Start fresh
docker-compose up -d

# Re-run migrations
cd BlazorAuthApp.Api
dotnet ef database update
```

---

## Data Persistence

Data is stored in a Docker volume named `postgres_data`. This means:
- ✅ Data persists when you stop/start containers
- ✅ Data survives container restarts
- ❌ Data is removed with `docker-compose down -v`

To completely reset the database, use:
```bash
docker-compose down -v
docker-compose up -d
cd BlazorAuthApp.Api
dotnet ef database update
```
