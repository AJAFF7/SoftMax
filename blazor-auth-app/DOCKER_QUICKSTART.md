# 🚀 Quick Start - Docker

Get the entire app running in 3 commands!

## Prerequisites
- Docker installed and running
- Docker Compose installed

## Start the App

```bash
cd blazor-auth-app
chmod +x *.sh
./start-docker.sh
```

That's it! The app will be available at:
- **Blazor App**: http://localhost
- **API**: http://localhost:8080

## First Time Setup

The migrations will run automatically when the API container starts.

## Useful Commands

### View logs
```bash
./logs-docker.sh          # All services
./logs-docker.sh api      # Just API
./logs-docker.sh postgres # Just database
```

### Stop the app
```bash
./stop-docker.sh          # Keep database data
./stop-docker.sh --clean  # Remove database data
```

### Restart services
```bash
docker-compose restart api
docker-compose restart blazor-app
```

## What's Running?

Check container status:
```bash
docker-compose ps
```

You should see 3 containers:
- `blazor-postgres` - PostgreSQL database
- `blazor-api` - .NET API backend
- `blazor-app` - Blazor WebAssembly frontend

## Troubleshooting

**Port already in use?**
```bash
# Stop and retry
./stop-docker.sh
./start-docker.sh
```

**Database issues?**
```bash
# Fresh start with clean database
./stop-docker.sh --clean
./start-docker.sh
```

**View detailed logs?**
```bash
./logs-docker.sh
```

For more details, see [DOCKER_DEPLOYMENT.md](DOCKER_DEPLOYMENT.md)
