using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlazorAuthApp.Api.Data;
using BlazorAuthApp.Api.DTOs;
using BlazorAuthApp.Api.Models;

namespace BlazorAuthApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppointmentsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public AppointmentsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/appointments
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetAppointments()
    {
        var appointments = await _context.Appointments
            .Include(a => a.Doctor)
            .Include(a => a.User)
            .OrderByDescending(a => a.CreatedAt)
            .Select(a => new AppointmentDto
            {
                Id = a.Id,
                UserId = a.UserId,
                DoctorId = a.DoctorId,
                AppointmentDate = a.AppointmentDate,
                TimeSlot = a.TimeSlot,
                PatientName = a.PatientName,
                PatientEmail = a.PatientEmail,
                PatientPhone = a.PatientPhone,
                Symptoms = a.Symptoms,
                Status = a.Status,
                PatientArrived = a.PatientArrived,
                PatientArrivedAt = a.PatientArrivedAt,
                IsFinished = a.IsFinished,
                FinishedAt = a.FinishedAt,
                CreatedAt = a.CreatedAt,
                Doctor = new DoctorDto
                {
                    Id = a.Doctor.Id,
                    FirstName = a.Doctor.FirstName,
                    LastName = a.Doctor.LastName,
                    Specialization = a.Doctor.Specialization,
                    Description = a.Doctor.Description,
                    Email = a.Doctor.Email,
                    Phone = a.Doctor.Phone,
                    ImageUrl = a.Doctor.ImageUrl,
                    ConsultationFee = a.Doctor.ConsultationFee,
                    YearsOfExperience = a.Doctor.YearsOfExperience,
                    IsAvailable = a.Doctor.IsAvailable
                }
            })
            .ToListAsync();

        return Ok(appointments);
    }

    // GET: api/appointments/user/{userId}
    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetUserAppointments(int userId)
    {
        var appointments = await _context.Appointments
            .Include(a => a.Doctor)
            .Include(a => a.CheckedInByAssistant)
            .Where(a => a.UserId == userId)
            .OrderByDescending(a => a.AppointmentDate)
            .Select(a => new AppointmentDto
            {
                Id = a.Id,
                UserId = a.UserId,
                DoctorId = a.DoctorId,
                AppointmentDate = a.AppointmentDate,
                TimeSlot = a.TimeSlot,
                PatientName = a.PatientName,
                PatientEmail = a.PatientEmail,
                PatientPhone = a.PatientPhone,
                Symptoms = a.Symptoms,
                Status = a.Status,
                PatientArrived = a.PatientArrived,
                PatientArrivedAt = a.PatientArrivedAt,
                IsFinished = a.IsFinished,
                FinishedAt = a.FinishedAt,
                CreatedAt = a.CreatedAt,
                CheckedInByAssistantId = a.CheckedInByAssistantId,
                CheckedInByAssistantName = a.CheckedInByAssistant != null ? a.CheckedInByAssistant.FullName : null,
                CheckedInByAssistantBarcode = a.CheckedInByAssistant != null ? a.CheckedInByAssistant.ETagBarcode : null,
                Doctor = new DoctorDto
                {
                    Id = a.Doctor.Id,
                    FirstName = a.Doctor.FirstName,
                    LastName = a.Doctor.LastName,
                    Specialization = a.Doctor.Specialization,
                    Description = a.Doctor.Description,
                    Email = a.Doctor.Email,
                    Phone = a.Doctor.Phone,
                    ImageUrl = a.Doctor.ImageUrl,
                    ConsultationFee = a.Doctor.ConsultationFee,
                    YearsOfExperience = a.Doctor.YearsOfExperience,
                    IsAvailable = a.Doctor.IsAvailable
                }
            })
            .ToListAsync();

        return Ok(appointments);
    }

    // GET: api/appointments/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<AppointmentDto>> GetAppointment(int id)
    {
        var appointment = await _context.Appointments
            .Include(a => a.Doctor)
            .Include(a => a.CheckedInByAssistant)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (appointment == null)
        {
            return NotFound(new { message = "Appointment not found" });
        }

        var appointmentDto = new AppointmentDto
        {
            Id = appointment.Id,
            UserId = appointment.UserId,
            DoctorId = appointment.DoctorId,
            AppointmentDate = appointment.AppointmentDate,
            TimeSlot = appointment.TimeSlot,
            PatientName = appointment.PatientName,
            PatientEmail = appointment.PatientEmail,
            PatientPhone = appointment.PatientPhone,
            Symptoms = appointment.Symptoms,
            Status = appointment.Status,
            PatientArrived = appointment.PatientArrived,
            PatientArrivedAt = appointment.PatientArrivedAt,
            IsFinished = appointment.IsFinished,
            FinishedAt = appointment.FinishedAt,
            CreatedAt = appointment.CreatedAt,
            CheckedInByAssistantId = appointment.CheckedInByAssistantId,
            CheckedInByAssistantName = appointment.CheckedInByAssistant?.FullName,
            CheckedInByAssistantBarcode = appointment.CheckedInByAssistant?.ETagBarcode,
            Doctor = new DoctorDto
            {
                Id = appointment.Doctor.Id,
                FirstName = appointment.Doctor.FirstName,
                LastName = appointment.Doctor.LastName,
                Specialization = appointment.Doctor.Specialization,
                Description = appointment.Doctor.Description,
                Email = appointment.Doctor.Email,
                Phone = appointment.Doctor.Phone,
                ImageUrl = appointment.Doctor.ImageUrl,
                ConsultationFee = appointment.Doctor.ConsultationFee,
                YearsOfExperience = appointment.Doctor.YearsOfExperience,
                IsAvailable = appointment.Doctor.IsAvailable
            }
        };

        return Ok(appointmentDto);
    }

    // POST: api/appointments
    [HttpPost]
    public async Task<ActionResult<AppointmentDto>> CreateAppointment(CreateAppointmentDto createDto)
    {
        // Get the user ID from the username in the request (you might need to adjust this based on your auth)
        var username = Request.Headers["X-Username"].ToString();
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

        if (user == null)
        {
            return Unauthorized(new { message = "User not found" });
        }

        // Verify doctor exists
        var doctor = await _context.Doctors.FindAsync(createDto.DoctorId);
        if (doctor == null)
        {
            return BadRequest(new { message = "Doctor not found" });
        }

        // Convert appointment date to UTC
        var appointmentDateUtc = createDto.AppointmentDate.Kind == DateTimeKind.Utc 
            ? createDto.AppointmentDate 
            : DateTime.SpecifyKind(createDto.AppointmentDate, DateTimeKind.Utc);

        // Check if the time slot is already taken
        var existingAppointment = await _context.Appointments
            .AnyAsync(a => a.DoctorId == createDto.DoctorId 
                && a.AppointmentDate.Date == appointmentDateUtc.Date 
                && a.TimeSlot == createDto.TimeSlot
                && a.Status != "Cancelled");

        if (existingAppointment)
        {
            return BadRequest(new { message = "This time slot is already booked" });
        }

        var appointment = new Appointment
        {
            UserId = user.Id,
            DoctorId = createDto.DoctorId,
            AppointmentDate = appointmentDateUtc,
            TimeSlot = createDto.TimeSlot,
            PatientName = createDto.PatientName,
            PatientEmail = createDto.PatientEmail,
            PatientPhone = createDto.PatientPhone,
            Symptoms = createDto.Symptoms,
            Status = "Pending",
            CreatedAt = DateTime.UtcNow
        };

        _context.Appointments.Add(appointment);
        await _context.SaveChangesAsync();

        // Load the doctor info for the response
        await _context.Entry(appointment).Reference(a => a.Doctor).LoadAsync();

        var appointmentDto = new AppointmentDto
        {
            Id = appointment.Id,
            UserId = appointment.UserId,
            DoctorId = appointment.DoctorId,
            AppointmentDate = appointment.AppointmentDate,
            TimeSlot = appointment.TimeSlot,
            PatientName = appointment.PatientName,
            PatientEmail = appointment.PatientEmail,
            PatientPhone = appointment.PatientPhone,
            Symptoms = appointment.Symptoms,
            Status = appointment.Status,
            PatientArrived = appointment.PatientArrived,
            PatientArrivedAt = appointment.PatientArrivedAt,
            IsFinished = appointment.IsFinished,
            FinishedAt = appointment.FinishedAt,
            CreatedAt = appointment.CreatedAt,
            Doctor = new DoctorDto
            {
                Id = appointment.Doctor.Id,
                FirstName = appointment.Doctor.FirstName,
                LastName = appointment.Doctor.LastName,
                Specialization = appointment.Doctor.Specialization,
                Description = appointment.Doctor.Description,
                Email = appointment.Doctor.Email,
                Phone = appointment.Doctor.Phone,
                ImageUrl = appointment.Doctor.ImageUrl,
                ConsultationFee = appointment.Doctor.ConsultationFee,
                YearsOfExperience = appointment.Doctor.YearsOfExperience,
                IsAvailable = appointment.Doctor.IsAvailable
            }
        };

        return CreatedAtAction(nameof(GetAppointment), new { id = appointment.Id }, appointmentDto);
    }

    // PUT: api/appointments/{id}/status
    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateAppointmentStatus(int id, UpdateAppointmentStatusDto updateDto)
    {
        var appointment = await _context.Appointments.FindAsync(id);

        if (appointment == null)
        {
            return NotFound(new { message = "Appointment not found" });
        }

        appointment.Status = updateDto.Status;
        appointment.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Ok(new { message = "Appointment status updated successfully" });
    }

    // DELETE: api/appointments/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAppointment(int id)
    {
        var appointment = await _context.Appointments.FindAsync(id);

        if (appointment == null)
        {
            return NotFound(new { message = "Appointment not found" });
        }

        // Instead of deleting, we can just mark it as cancelled
        appointment.Status = "Cancelled";
        appointment.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Ok(new { message = "Appointment cancelled successfully" });
    }

    // PUT: api/appointments/{id}/check-in
    [HttpPut("{id}/check-in")]
    public async Task<IActionResult> CheckInPatient(int id, [FromBody] CheckInDto checkInDto)
    {
        var appointment = await _context.Appointments.FindAsync(id);

        if (appointment == null)
        {
            return NotFound(new { message = "Appointment not found" });
        }

        // Verify assistant exists
        var assistant = await _context.Assistants.FindAsync(checkInDto.AssistantId);
        if (assistant == null || !assistant.IsActive)
        {
            return BadRequest(new { message = "Invalid or inactive assistant" });
        }

        appointment.PatientArrived = true;
        appointment.PatientArrivedAt = DateTime.UtcNow;
        appointment.CheckedInByAssistantId = checkInDto.AssistantId;
        appointment.Status = "Confirmed";
        appointment.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Ok(new { 
            message = "Patient checked in successfully", 
            patientArrived = true, 
            patientArrivedAt = appointment.PatientArrivedAt,
            checkedInBy = assistant.FullName,
            assistantBarcode = assistant.ETagBarcode
        });
    }

    // PUT: api/appointments/{id}/finish
    [HttpPut("{id}/finish")]
    public async Task<IActionResult> FinishAppointment(int id)
    {
        var appointment = await _context.Appointments.FindAsync(id);

        if (appointment == null)
        {
            return NotFound(new { message = "Appointment not found" });
        }

        appointment.IsFinished = true;
        appointment.FinishedAt = DateTime.UtcNow;
        appointment.Status = "Completed";
        appointment.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Ok(new { message = "Appointment marked as finished", isFinished = true, finishedAt = appointment.FinishedAt });
    }
}
