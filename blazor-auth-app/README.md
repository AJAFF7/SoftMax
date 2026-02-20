# 🏥 SoftMax Portal

A modern healthcare appointment management system built with Blazor WebAssembly, ASP.NET Core, and PostgreSQL.

## ✨ Features

### For Patients
- 👤 User Registration & Login
- 📅 Book Appointments with Doctors
- 👁️ View Appointment History
- ❌ Cancel Appointments
- 📊 Dashboard with Appointment Statistics

### For Assistants (Staff)
- 🔐 QR Code-based Authentication
- 📋 View All Appointments
- ✅ Check-in Patients
- ✓ Complete Appointments
- 🔍 Search & Filter Appointments
- 📊 Real-time Statistics Dashboard

### System Features
- 🐳 Fully Dockerized
- 🔒 JWT Authentication
- 🗄️ PostgreSQL Database
- 🌊 Modern Ocean Blue UI Theme
- 📱 Responsive Design
- 🚀 Blazor WebAssembly (Client-side)
- 🔌 RESTful API

## 🛠️ Tech Stack

- **Frontend**: Blazor WebAssembly (.NET 10.0 Preview)
- **Backend**: ASP.NET Core Web API (.NET 10.0 Preview)
- **Database**: PostgreSQL 17
- **ORM**: Entity Framework Core
- **Authentication**: JWT Tokens
- **Containerization**: Docker & Docker Compose
- **Web Server**: Nginx (for Blazor app)
- **QR Codes**: QRCoder Library

## 📋 Prerequisites

- Docker & Docker Compose
- .NET 10.0 SDK (for local development)
- Git

## 🚀 Quick Start

### Using Docker (Recommended)

1. **Clone the repository**
   ```bash
   git clone <your-repo-url>
   cd blazor-auth-app
   ```

2. **Start the application**
   ```bash
   docker-compose up -d
   ```

3. **Access the application**
   - Blazor App: http://localhost:80
   - API: http://localhost:8080/api
   - PostgreSQL: localhost:5432

4. **Create SoftMax Assistant Account**
   ```bash
   chmod +x register-softmax.sh
   ./register-softmax.sh
   ```

### Local Development

1. **Install .NET 10.0 SDK**
   ```bash
   # Download from: https://dotnet.microsoft.com/download/dotnet/10.0
   ```

2. **Setup PostgreSQL**
   ```bash
   # Using Docker
   docker run -d \
     --name postgres \
     -e POSTGRES_PASSWORD=postgres \
     -e POSTGRES_DB=blazorauthdb \
     -p 5432:5432 \
     postgres:17-alpine
   ```

3. **Run Migrations**
   ```bash
   cd BlazorAuthApp.Api
   dotnet ef database update
   ```

4. **Start API**
   ```bash
   cd BlazorAuthApp.Api
   dotnet run
   ```

5. **Start Blazor App**
   ```bash
   cd BlazorAuthApp
   dotnet run
   ```

## 📁 Project Structure

```
blazor-auth-app/
├── BlazorAuthApp/              # Blazor WebAssembly Frontend
│   ├── Pages/                  # Razor Pages/Components
│   ├── Services/               # API Service Layer
│   ├── Models/                 # Client-side Models
│   ├── wwwroot/                # Static Files
│   └── nginx.conf              # Nginx Configuration
│
├── BlazorAuthApp.Api/          # ASP.NET Core API Backend
│   ├── Controllers/            # API Controllers
│   ├── Models/                 # Database Models
│   ├── DTOs/                   # Data Transfer Objects
│   ├── Data/                   # DbContext & Migrations
│   ├── Migrations/             # EF Core Migrations
│   └── docker-entrypoint.sh    # Docker Startup Script
│
├── docker-compose.yml          # Docker Compose Configuration
├── register-softmax.sh         # Create SoftMax Assistant
└── README.md                   # This file
```

## 🔑 Default Credentials

### Patient Account
Create your own via registration at `/register`

### SoftMax Assistant
- **Username**: `softmax`
- **Password**: `SoftMax123!`
- **Login**: QR Code or credentials at `/assistant-login`

## 🐳 Docker Architecture

```
┌─────────────────┐
│   Nginx (80)    │  ← Blazor WebAssembly
│   + API Proxy   │     /api → blazor-api:8080
└────────┬────────┘
         │
    ┌────┴────────────────┐
    │                     │
┌───▼──────┐      ┌──────▼─────┐
│   API    │      │ PostgreSQL │
│  (8080)  │◄────►│   (5432)   │
└──────────┘      └────────────┘
```

## 🔧 Configuration

### Environment Variables

Edit `docker-compose.yml` to configure:

```yaml
POSTGRES_PASSWORD: postgres
POSTGRES_DB: blazorauthdb
ConnectionStrings__DefaultConnection: "Host=postgres;Database=blazorauthdb;Username=postgres;Password=postgres"
```

### API Base URL

For public access, update:
- `BlazorAuthApp/wwwroot/appsettings.json`
- Change `ApiBaseUrl` to your domain

### Cloudflare Tunnel (Optional)

Configure in Cloudflare dashboard:
- Service type: HTTP
- URL: http://localhost:80

## 📊 Database Schema

### Main Tables
- **Users** - Patient accounts
- **Doctors** - Healthcare providers
- **Appointments** - Booking records
- **Assistants** - Staff accounts with QR codes

### Key Features
- Patient check-in tracking
- Appointment status workflow
- QR code authentication
- Session management

## 🎨 UI Theme

- **Color Scheme**: Ocean Blue Gradient
- **Primary**: #006994 → #00b4d8 → #90e0ef
- **Status Colors**:
  - Pending: Orange (#f59e0b)
  - Confirmed: Green (#10b981)
  - Completed: Purple (#8b5cf6)
  - Cancelled: Red (#ef4444)

## 🔒 Security

- JWT-based authentication
- Password hashing with BCrypt
- CORS configuration
- Admin-only assistant creation
- Secure QR code generation

## 📝 API Endpoints

### Authentication
- `POST /api/auth/register` - User registration
- `POST /api/auth/login` - User login

### Appointments
- `GET /api/appointments` - Get all appointments
- `GET /api/appointments/user/{userId}` - User appointments
- `POST /api/appointments` - Create appointment
- `PUT /api/appointments/{id}/cancel` - Cancel appointment
- `PUT /api/appointments/{id}/status` - Update status

### Doctors
- `GET /api/doctors` - List all doctors

### Assistants
- `POST /api/assistants/register` - Create assistant (admin)
- `POST /api/assistants/login` - Assistant login
- `POST /api/assistants/login/barcode` - QR code login
- `GET /api/assistants/qrcode/{email}` - Generate QR code

## 🛠️ Development Scripts

| Script | Description |
|--------|-------------|
| `start-docker.sh` | Start all Docker services |
| `stop-docker.sh` | Stop all Docker services |
| `restart.sh` | Restart Docker services |
| `register-softmax.sh` | Create SoftMax assistant |
| `logs-docker.sh` | View Docker logs |

## 🚀 Deployment

### Production Checklist
- [ ] Update connection strings
- [ ] Configure HTTPS/SSL
- [ ] Set up Cloudflare Tunnel or reverse proxy
- [ ] Change default passwords
- [ ] Enable CORS for your domain
- [ ] Set up automated backups
- [ ] Configure monitoring

### Docker Compose Production
```bash
docker-compose -f docker-compose.yml up -d
```

## 🐛 Troubleshooting

### API not responding
```bash
docker-compose logs api
docker-compose restart api
```

### Database connection issues
```bash
docker-compose logs postgres
docker-compose exec postgres psql -U postgres -d blazorauthdb
```

### Blazor app not loading
```bash
docker-compose logs blazor-app
# Hard refresh browser: Ctrl+Shift+R
```

### Migration issues
```bash
docker-compose exec api dotnet ef migrations list
docker-compose exec api dotnet ef database update
```

## 📚 Documentation

- [Docker Setup Guide](DOCKER_SETUP.md)
- [Docker Quick Start](DOCKER_QUICKSTART.md)
- [Docker Deployment](DOCKER_DEPLOYMENT.md)

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the MIT License.

## 👥 Support

For issues and questions:
1. Check the troubleshooting section
2. Review Docker logs: `docker-compose logs`
3. Open an issue on GitHub

## 🎯 Roadmap

- [ ] Add unit tests
- [ ] Implement email notifications
- [ ] Add appointment reminders
- [ ] Multi-language support
- [ ] Mobile app (MAUI)
- [ ] Advanced reporting
- [ ] Integration with calendar systems

---

**Built with ❤️ using Blazor WebAssembly and .NET 10.0**
