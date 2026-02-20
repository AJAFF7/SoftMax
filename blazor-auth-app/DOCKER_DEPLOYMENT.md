# Docker Deployment Guide

## Complete Containerized Setup

This setup runs the entire application stack in Docker containers:
- **PostgreSQL Database** (Port 5432)
- **API Backend** (Port 8080)
- **Blazor WebAssembly App** (Port 80)

---

## Quick Start

### 1. Start All Services with Docker

```bash
cd blazor-auth-app
chmod +x start-docker.sh
./start-docker.sh
```

This will:
- ✅ Build Docker images for API and Blazor app
- ✅ Start PostgreSQL, API, and Blazor containers
- ✅ Set up networking between containers
- ✅ Expose services on localhost

### 2. Run Database Migrations

After containers are running, apply migrations:

```bash
chmod +x run-migrations-docker.sh
./run-migrations-docker.sh
```

### 3. Access the Application

- **Blazor App**: http://localhost
- **API**: http://localhost:8080/api
- **PostgreSQL**: localhost:5432

---

## Docker Architecture

```
┌─────────────────┐
│  Blazor App     │  Port 80 (nginx)
│  (Frontend)     │
└────────┬────────┘
         │
         ↓ API calls
┌─────────────────┐
│  API Backend    │  Port 8080 (.NET)
│  (ASP.NET Core) │
└────────┬────────┘
         │
         ↓ Database queries
┌─────────────────┐
│  PostgreSQL     │  Port 5432
│  (Database)     │
└─────────────────┘
```

All containers communicate via a Docker bridge network called `blazor-network`.

---

## Manual Docker Commands

### Build Images
```bash
docker-compose build
```

### Start Services
```bash
docker-compose up -d
```

### Stop Services
```bash
docker-compose down
```

### View Logs
```bash
# All services
docker-compose logs -f

# Specific service
docker-compose logs -f api
docker-compose logs -f blazor-app
docker-compose logs -f postgres
```

### Restart a Service
```bash
docker-compose restart api
docker-compose restart blazor-app
```

### Fresh Start (Remove All Data)
```bash
docker-compose down -v
docker-compose up --build -d
```

---

## Database Management

### Access PostgreSQL CLI
```bash
docker exec -it blazor-postgres psql -U blazoruser -d blazorauthdb
```

### Run Migrations
```bash
docker exec blazor-api dotnet ef database update
```

### View Tables
```sql
-- Inside PostgreSQL CLI
\dt
SELECT * FROM "Users";
SELECT * FROM "Doctors";
SELECT * FROM "Appointments";
```

---

## Troubleshooting

### Container Not Starting

Check logs:
```bash
docker-compose logs <service-name>
```

### Port Already in Use

Stop existing services:
```bash
# Stop Docker services
docker-compose down

# Or change ports in docker-compose.yml
```

### Database Connection Issues

Verify PostgreSQL is healthy:
```bash
docker-compose ps
```

The postgres container should show "healthy" status.

### Rebuild After Code Changes

```bash
docker-compose down
docker-compose up --build -d
```

---

## Environment Variables

### API Container (`appsettings.Docker.json`)
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=postgres;Port=5432;Database=blazorauthdb;Username=blazoruser;Password=BlazorPass123!"
  }
}
```

**Note**: Use `Host=postgres` (container name) instead of `localhost` for inter-container communication.

### Blazor App (`wwwroot/appsettings.json`)
```json
{
  "ApiBaseUrl": "http://localhost:8080/api"
}
```

---

## Development vs Production

### Development (Current Setup)
- Ports exposed on localhost
- No HTTPS (HTTP only)
- Debug logging enabled

### Production Recommendations
1. Use HTTPS with SSL certificates
2. Use environment variables for secrets
3. Use Docker secrets or Azure Key Vault
4. Set up reverse proxy (nginx/Caddy)
5. Enable health checks and monitoring
6. Use container orchestration (Kubernetes/Docker Swarm)

---

## Container Details

| Service | Image | Port | Purpose |
|---------|-------|------|---------|
| postgres | postgres:17-alpine | 5432 | Database |
| api | Custom (.NET 9) | 8080 | REST API Backend |
| blazor-app | Custom (nginx + Blazor WASM) | 80 | Frontend Web App |

---

## Data Persistence

PostgreSQL data is persisted in a Docker volume:
- Volume name: `blazor-auth-app_postgres_data`
- Data survives container restarts
- Removed only with `docker-compose down -v`

View volumes:
```bash
docker volume ls
```

---

## Next Steps

1. ✅ Start containers: `./start-docker.sh`
2. ✅ Run migrations: `./run-migrations-docker.sh`
3. ✅ Open browser: http://localhost
4. ✅ Register a user and test the app

For local development without Docker, see [README.md](README.md)
