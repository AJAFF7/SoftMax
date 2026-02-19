# HealthCare+ Doctor Appointment System - Setup Guide

## Overview
Your Blazor app has been transformed into a professional **Doctor Appointment System** with the following features:

### ✨ New Features Added:
1. **Doctor Directory** - Browse doctors by specialization
2. **Appointment Booking** - Book appointments with available time slots
3. **My Appointments** - View and manage your appointments
4. **Professional UI** - Modern, responsive design with gradient themes
5. **Pre-seeded Data** - 5 doctors across different specializations

---

## Database Migration

Since new models have been added, you need to create and run a migration.

### Step 1: Create Migration
```bash
cd BlazorAuthApp.Api
dotnet ef migrations add AddDoctorsAndAppointments
```

### Step 2: Update Database
```bash
dotnet ef database update
```

This will:
- Add `Doctors` table with 5 pre-seeded doctors
- Add `Appointments` table with relationships to Users and Doctors
- Update `Users` table with additional fields (FirstName, LastName, Email, Phone)

---

## Running the Application

### Terminal 1: Start the API
```bash
cd BlazorAuthApp.Api
dotnet run
```
The API will start at: `https://localhost:7188`

### Terminal 2: Start the Blazor WebAssembly App
```bash
cd BlazorAuthApp
dotnet watch run
```
The Blazor app will start at: `https://localhost:5001` (or the port shown in console)

---

## Pre-seeded Doctors

The database is automatically seeded with 5 doctors:

| Name | Specialization | Fee | Experience |
|------|---------------|-----|------------|
| Dr. Sarah Johnson | Cardiology | $150 | 12 years |
| Dr. Michael Chen | Pediatrics | $120 | 8 years |
| Dr. Emily Rodriguez | Dermatology | $130 | 10 years |
| Dr. David Patel | Orthopedics | $160 | 15 years |
| Dr. Lisa Thompson | Neurology | $180 | 18 years |

---

## Application Structure

### Frontend (BlazorAuthApp)
- **Pages/**
  - `Home.razor` - Landing page with hero section
  - `Doctors.razor` - Browse and filter doctors
  - `BookAppointment.razor` - Book appointments
  - `MyAppointments.razor` - View user's appointments
  
- **Services/**
  - `DoctorService.cs` - Doctor API communication
  - `AppointmentService.cs` - Appointment API communication
  
- **Models/**
  - `Doctor.cs` - Doctor model
  - `Appointment.cs` - Appointment model

### Backend (BlazorAuthApp.Api)
- **Controllers/**
  - `DoctorsController.cs` - Doctor CRUD endpoints
  - `AppointmentsController.cs` - Appointment management
  
- **Models/**
  - `Doctor.cs` - Doctor entity
  - `Appointment.cs` - Appointment entity
  - `User.cs` - Updated user entity
  
- **DTOs/**
  - `AppointmentDTOs.cs` - Data transfer objects

---

## Available Endpoints

### Doctors API
- `GET /api/doctors` - Get all available doctors
- `GET /api/doctors/{id}` - Get doctor by ID
- `GET /api/doctors/specializations` - Get all specializations
- `GET /api/doctors/search?specialization={spec}` - Search doctors

### Appointments API
- `GET /api/appointments` - Get all appointments
- `GET /api/appointments/user/{userId}` - Get user's appointments
- `GET /api/appointments/{id}` - Get appointment by ID
- `POST /api/appointments` - Create new appointment
- `PUT /api/appointments/{id}/status` - Update appointment status
- `DELETE /api/appointments/{id}` - Cancel appointment

---

## Usage Flow

1. **Register/Login** - Create an account or login
2. **Browse Doctors** - Navigate to "Find Doctors" to see available doctors
3. **Filter by Specialization** - Use the dropdown to filter doctors
4. **Book Appointment** - Click "Book Appointment" on a doctor card
5. **Fill Details** - Enter patient information, select date and time slot
6. **Manage Appointments** - View all your appointments in "My Appointments"
7. **Cancel if Needed** - Cancel appointments from the My Appointments page

---

## Appointment Statuses

- **Pending** - Newly created, awaiting confirmation
- **Confirmed** - Doctor confirmed the appointment
- **Completed** - Appointment has been completed
- **Cancelled** - Appointment was cancelled

---

## Available Time Slots

Appointments are available in 30-minute slots:
- Morning: 9:00 AM - 12:00 PM
- Afternoon: 2:00 PM - 6:00 PM

---

## Features Breakdown

### Professional UI/UX
- Modern gradient color scheme (Purple to Indigo)
- Responsive design (mobile, tablet, desktop)
- Smooth animations and transitions
- Professional doctor cards with avatars
- Status badges with color coding

### Navigation
- Sticky navigation bar with branding
- Role-based menu items (authenticated vs. guest)
- Smooth page transitions

### Data Validation
- Form validation on booking
- Duplicate time slot prevention
- User authentication checks

---

## Customization

### Adding More Doctors
You can seed more doctors by adding them to the `OnModelCreating` method in `ApplicationDbContext.cs`:

```csharp
new Doctor
{
    Id = 6,
    FirstName = "John",
    LastName = "Smith",
    Specialization = "General Practice",
    Description = "Description here",
    Email = "john.smith@hospital.com",
    Phone = "+1-555-0106",
    ImageUrl = "https://ui-avatars.com/api/?name=John+Smith&background=3B82F6&color=fff&size=200",
    ConsultationFee = 100.00m,
    YearsOfExperience = 5,
    IsAvailable = true
}
```

### Changing Color Scheme
Update the gradient colors in:
- `NavMenu.razor.css`
- `Home.razor.css`
- Individual page CSS files

---

## Troubleshooting

### Database Connection Issues
- Ensure PostgreSQL is running
- Check connection string in `appsettings.json`
- Verify database exists

### CORS Errors
- Ensure API is running on `https://localhost:7188`
- Check CORS policy in `Program.cs` includes your Blazor app URL

### Migration Errors
```bash
# Drop all tables and recreate
dotnet ef database drop
dotnet ef migrations remove
dotnet ef migrations add Initial
dotnet ef database update
```

---

## Next Steps

Consider adding:
1. Email notifications for appointments
2. Doctor availability calendar
3. Appointment reminders
4. Patient medical history
5. Video consultation integration
6. Prescription management
7. Admin dashboard for doctors
8. Payment processing

---

## Support

For issues or questions, check the console logs in both the API and Blazor app for detailed error messages.

---

## Technology Stack

- **Frontend**: Blazor WebAssembly (.NET 10.0)
- **Backend**: ASP.NET Core Web API (.NET 10.0)
- **Database**: PostgreSQL
- **ORM**: Entity Framework Core
- **Authentication**: Local Storage (can be upgraded to JWT)
- **Styling**: Custom CSS with modern design patterns

---

Enjoy your professional doctor appointment system! 🏥
