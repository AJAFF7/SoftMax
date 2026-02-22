# SoftMax System Architecture

## Overview

The SoftMax application is a containerized multi-tier web application consisting of four main components working together:

1. **PostgreSQL Database** - Data persistence layer
2. **ASP.NET Core Web API** - Backend business logic and data access
3. **Blazor WebAssembly App** - Frontend user interface
4. **Nginx Web Server** - Reverse proxy and static file serving

## Architecture Diagram

```
┌─────────────────────────────────────────────────────────────┐
│                        User Browser                          │
│  (http://localhost)                                          │
└────────────────────────┬────────────────────────────────────┘
                         │
                         ▼
┌─────────────────────────────────────────────────────────────┐
│              Container: blazor-app (Port 80)                 │
│  ┌────────────────────────────────────────────────────────┐ │
│  │                   Nginx Web Server                      │ │
│  │                                                          │ │
│  │  • Serves Blazor WebAssembly static files              │ │
│  │  • Routes /api/* requests to backend API               │ │
│  │  • Handles MIME types for .wasm, .js, .css files       │ │
│  └────────┬───────────────────────────────┬────────────────┘ │
└───────────┼───────────────────────────────┼──────────────────┘
            │                               │
    Static Files                     API Requests (/api/*)
    (HTML, CSS, JS, WASM)                  │
            │                               │
            ▼                               ▼
   User's Browser              ┌────────────────────────────────┐
   (Blazor runs here)          │  Container: blazor-api         │
                               │  (Port 8080)                   │
                               │  ┌──────────────────────────┐  │
                               │  │  ASP.NET Core Web API    │  │
                               │  │                          │  │
                               │  │  • Authentication        │  │
                               │  │  • Business Logic        │  │
                               │  │  • QR Code Generation    │  │
                               │  │  • RESTful Endpoints     │  │
                               │  └────────┬─────────────────┘  │
                               └───────────┼────────────────────┘
                                          │
                                  Entity Framework Core
                                          │
                                          ▼
                               ┌────────────────────────────────┐
                               │  Container: blazor-postgres    │
                               │  (Port 5432)                   │
                               │  ┌──────────────────────────┐  │
                               │  │   PostgreSQL Database    │  │
                               │  │                          │  │
                               │  │  Tables:                 │  │
                               │  │  • Users                 │  │
                               │  │  • Doctors               │  │
                               │  │  • Assistants            │  │
                               │  │  • Appointments          │  │
                               │  └──────────────────────────┘  │
                               └────────────────────────────────┘

Network: blazor-network (Docker bridge network)
```

## Component Details

### 1. PostgreSQL Container (`blazor-postgres`)

**Purpose:** Primary data storage for the entire application

**Configuration:**
- **Image:** `postgres:17-alpine` (Lightweight Alpine Linux variant)
- **Port:** 5432 (PostgreSQL default)
- **Database:** `blazorauthdb`
- **Credentials:**
  - Username: `blazoruser`
  - Password: `BlazorPass123!`

**Features:**
- **Persistent Storage:** Uses Docker volume `postgres_data` to persist data across container restarts
- **Health Check:** Validates database readiness with `pg_isready` command every 10 seconds
- **Auto-initialization:** Database is created automatically on first startup

**Data Stored:**
- User accounts with hashed passwords
- Doctor profiles
- Assistant profiles with QR codes
- Appointment records
- Check-in history

---

### 2. API Container (`blazor-api`)

**Purpose:** Backend service handling business logic, authentication, and database operations

**Configuration:**
- **Base Image:** `mcr.microsoft.com/dotnet/aspnet:10.0-preview`
- **Port:** 8080 (mapped to host 8080)
- **Environment:** Docker (uses `appsettings.Docker.json`)

**Build Process:**
1. **Build Stage:**
   - Uses .NET SDK to compile the API project
   - Restores NuGet packages
   - Builds in Release mode

2. **EF Bundle Stage:**
   - Creates Entity Framework migration bundle
   - Bundles all database migrations into a single executable
   - Platform-specific (`linux-arm64` or generic fallback)

3. **Final Stage:**
   - Uses lightweight ASP.NET runtime image
   - Copies compiled API DLL
   - Copies migration bundle
   - Includes startup script

**Startup Process (`docker-entrypoint.sh`):**
1. **Wait for PostgreSQL:** Retries connection up to 30 times (60 seconds)
2. **Apply Migrations:** Runs EF Core migration bundle to update database schema
3. **Start API:** Launches the ASP.NET Core application

**Database Connection:**
```
Host=postgres;Port=5432;Database=blazorauthdb;Username=blazoruser;Password=BlazorPass123!
```
*Note: Uses container name `postgres` for DNS resolution within Docker network*

**API Endpoints:**

**Authentication:**
- `POST /api/auth/register` - User registration
- `POST /api/auth/login` - User login with JWT tokens

**Assistants:**
- `POST /api/assistants/register` - Register new assistant
- `POST /api/assistants/login` - Username/password login
- `POST /api/assistants/login-barcode` - QR code barcode login
- `GET /api/assistants/qrcode/{email}` - Generate QR code image

**Doctors:**
- `GET /api/doctors` - List all doctors
- `POST /api/doctors` - Create new doctor
- `GET /api/doctors/{id}` - Get doctor details

**Appointments:**
- `POST /api/appointments/book` - Book appointment
- `GET /api/appointments/user/{userId}` - User's appointments
- `GET /api/appointments` - All appointments (with filters)
- `POST /api/appointments/{id}/checkin` - Check-in patient

**Health Check:**
- Endpoint: `http://localhost:8080/api/doctors`
- Interval: Every 30 seconds
- Used by Docker to verify API is running

**Key Features:**
- **Password Hashing:** SHA-256 for secure password storage
- **QR Code Generation:** Uses QRCoder library
- **CORS Enabled:** Allows cross-origin requests from Blazor frontend

---

### 3. Blazor WebAssembly Container (`blazor-app`)

**Purpose:** Serves the frontend user interface as static files

**Configuration:**
- **Base Image:** `nginx:alpine` (Lightweight web server)
- **Port:** 80 (HTTP, mapped to host port 80)
- **Root Directory:** `/usr/share/nginx/html`

**Build Process:**
1. **Build Stage:**
   - Uses .NET SDK to build Blazor WebAssembly project
   - Compiles C# code to .NET IL
   - Publishes to `wwwroot` folder containing:
     - `index.html` - Entry point
     - `.wasm` files - WebAssembly binaries
     - `.js` files - JavaScript interop and runtime
     - `.dll` files - .NET assemblies
     - Static assets (CSS, images, etc.)

2. **Final Stage:**
   - Copies built files to Nginx HTML directory
   - Configures Nginx with custom config

**Nginx Configuration (`nginx.conf`):**

**Static File Serving:**
```nginx
location / {
    try_files $uri $uri/ /index.html;
}
```
- Serves Blazor app files
- Falls back to `index.html` for client-side routing (SPA)

**API Reverse Proxy:**
```nginx
location /api/ {
    proxy_pass http://blazor-api:8080/api/;
    proxy_set_header Host $host;
    proxy_set_header X-Real-IP $remote_addr;
    # ... more headers
}
```
- Forwards `/api/*` requests to backend API container
- Preserves client IP and headers
- Enables seamless frontend-backend communication

**MIME Type Handling:**
```nginx
location ~ \.wasm$ {
    types {
        application/wasm wasm;
    }
}
```
- Critical for WebAssembly files
- Ensures browser recognizes .wasm files

**Caching Strategy:**
```nginx
location ~ \.(js|css|json|ico|png|jpg|jpeg|gif|svg|woff|woff2|ttf|eot)$ {
    expires 1y;
    add_header Cache-Control "public, immutable";
}
```
- Static assets cached for 1 year
- Improves performance for returning users

**Client-Side Features:**
- **Blazor Components:** Interactive UI components
- **HTML5-QRCode:** QR code scanning library
- **Local Storage:** Stores user session data
- **HTTP Client:** Communicates with backend API via `/api` endpoint

---

## Network Architecture

**Docker Network:** `blazor-network` (Bridge driver)

**Communication Flow:**

1. **User Access:**
   ```
   Browser → http://localhost:80
   ```

2. **Page Load:**
   ```
   Browser → Nginx (blazor-app:80) → Static Files → Browser
   ```
   - Nginx serves index.html, .wasm, .js, .css files
   - Blazor WebAssembly loads in browser
   - Application runs entirely in client browser

3. **API Calls:**
   ```
   Browser → Nginx (blazor-app:80) → Reverse Proxy → API (blazor-api:8080)
   ```
   - JavaScript fetch() calls `/api/...`
   - Nginx forwards to `http://blazor-api:8080/api/...`
   - API processes request
   - Response flows back through Nginx to browser

4. **Database Operations:**
   ```
   API (blazor-api:8080) → PostgreSQL (postgres:5432)
   ```
   - API uses Entity Framework Core
   - TCP connection to `postgres:5432`
   - Database queries and updates

**Container DNS Resolution:**
- Containers reference each other by service name
- `postgres` resolves to PostgreSQL container IP
- `blazor-api` resolves to API container IP
- Docker manages DNS internally

---

## Data Flow Examples

### Example 1: Assistant Login with QR Code

```
1. User clicks "Open Camera & Scan QR Code" in browser
   ↓
2. Browser requests camera permission
   ↓
3. HTML5-QRCode library scans the QR code barcode
   ↓
4. Blazor app calls: POST /api/assistants/login-barcode
                      Body: "AST-20260222-12345"
   ↓
5. Nginx receives request at port 80
   ↓
6. Nginx proxies to: http://blazor-api:8080/api/assistants/login-barcode
   ↓
7. API receives request in AssistantsController.LoginByBarcode()
   ↓
8. API queries PostgreSQL:
   SELECT * FROM Assistants WHERE ETagBarcode = @barcode AND IsActive = true
   ↓
9. If match found:
    - Return Assistant data
   ↓
10. Response flows back: API → Nginx → Browser
    ↓
11. Blazor stores session in localStorage
    ↓
12. Redirects to /assistant-dashboard
### Example 2: Booking an Appointment

```
1. User fills appointment form in Blazor UI
   ↓
2. Blazor calls: POST /api/appointments/book
                  Body: { doctorId, userId, appointmentDate, ... }
   ↓
3. Request → Nginx → API
   ↓
4. API validates data in AppointmentsController
   ↓
5. API executes:
   INSERT INTO Appointments (DoctorId, UserId, AppointmentDate, ...)
   VALUES (@p0, @p1, @p2, ...)
   ↓
6. PostgreSQL returns new appointment ID
   ↓
7. API returns appointment object
   ↓
8. Response → Nginx → Browser
   ↓
9. Blazor updates UI with confirmation
```

---

## Deployment Workflow

### Starting the System:

```bash
# Start all containers
docker-compose up -d

# What happens:
# 1. Create network: blazor-network
# 2. Start postgres container
#    - Initialize database
#    - Health check begins
# 3. Wait for postgres health check to pass
# 4. Start api container
#    - Wait for postgres (docker-entrypoint.sh)
#    - Apply migrations
#    - Start API server
# 5. Start blazor-app container
#    - Nginx starts serving static files
```

### First-Time Database Setup:

The database is automatically initialized through Entity Framework migrations:

1. **Migration Bundle** created during API build
2. **docker-entrypoint.sh** runs migration bundle
3. **Tables Created:**
   - `Users`
   - `Doctors`
   - `Assistants` (includes FaceDescriptor column)
   - `Appointments`
   - `__EFMigrationsHistory` (tracking)

### Stopping the System:

```bash
# Stop and remove containers
docker-compose down

# Stop and remove containers + volumes (deletes database)
docker-compose down -v
```

---

## Security Considerations

### 1. **Password Security**
- SHA-256 hashing (consider bcrypt/argon2 for production)
- Passwords never stored in plain text

### 2. **Network Isolation**
- All containers on private `blazor-network`
- Only port 80 exposed to host
- API/Database not directly accessible from internet

### 3. **Database Access**
- Credentials in environment variables
- Connection string in config (use secrets for production)

### 4. **HTTPS**
- Currently HTTP only
- Production should use reverse proxy with SSL/TLS
- Certificate management required

---

## Performance Optimization

### 1. **Static Asset Caching**
- 1-year cache for immutable files
- Reduces bandwidth and load times

### 2. **Health Checks**
- Automatic container restart on failure
- Database connection validation

### 3. **WebAssembly**
- Client-side execution reduces server load
- Blazor runs in browser, not server

### 4. **Connection Pooling**
- Entity Framework Core manages DB connection pool
- Efficient database resource usage

### 5. **Docker Multi-Stage Builds**
- Final images contain only runtime dependencies
- Smaller image size = faster deployment

---

## Monitoring and Logging

### Container Logs:

```bash
# View all logs
docker-compose logs

# Follow specific service
docker-compose logs -f api

# Check postgres logs
docker-compose logs postgres
```

### Health Status:

```bash
# Check container health
docker ps

# View health check results
docker inspect blazor-api | grep -A 10 Health
```

---

## Environment Variables

### PostgreSQL:
- `POSTGRES_USER`: Database username
- `POSTGRES_PASSWORD`: Database password
- `POSTGRES_DB`: Database name

### API:
- `ASPNETCORE_ENVIRONMENT`: Runtime environment (Docker)
- `ASPNETCORE_URLS`: Listening address (http://+:8080)

### Connection String:
- Built dynamically using PostgreSQL container name and credentials

---

## Troubleshooting

### Database Connection Issues:
```bash
# Check if postgres is healthy
docker ps

# Test database connection
docker exec -it blazor-postgres psql -U blazoruser -d blazorauthdb
```

### API Not Starting:
```bash
# Check migration status
docker logs blazor-api

# Verify postgres is ready
docker logs blazor-postgres
```

### Nginx Proxy Issues:
```bash
# Check nginx config
docker exec blazor-app nginx -t

# View nginx logs
docker logs blazor-app
```

---

## Technology Stack Summary

| Component | Technology | Version |
|-----------|-----------|---------|
| Frontend | Blazor WebAssembly | .NET 10.0 |
| Web Server | Nginx | Alpine Linux |
| Backend API | ASP.NET Core | .NET 10.0 |
| Database | PostgreSQL | 17 Alpine |
| ORM | Entity Framework Core | 10.0 |
| Container Runtime | Docker | Latest |
| Orchestration | Docker Compose | v2+ |
| QR Code | HTML5-QRCode | Latest |
| QR Generation | QRCoder | .NET Library |

---

## Scalability Considerations

### Current Architecture:
- Single instance of each service
- Suitable for development and small deployments

### Production Scaling Options:

1. **Load Balancing:**
   - Multiple API container replicas
   - Nginx load balancer in front

2. **Database:**
   - PostgreSQL read replicas
   - Connection pooling optimization
   - Consider managed PostgreSQL (AWS RDS, Azure Database)

3. **Caching:**
   - Redis for session storage
   - API response caching

4. **CDN:**
   - Serve Blazor static files from CDN
   - Reduce origin server load

5. **Container Orchestration:**
   - Kubernetes for auto-scaling
   - Health-based pod management
   - Rolling deployments

---

## Conclusion

This architecture provides a clean separation of concerns with:
- **Frontend:** Blazor WebAssembly running in the browser
- **Gateway:** Nginx handling routing and static files
- **Backend:** ASP.NET Core API providing business logic
- **Database:** PostgreSQL ensuring data persistence

All components communicate securely within a Docker network, with Nginx acting as the single entry point for external traffic. The containerized approach ensures consistency across development and production environments.
