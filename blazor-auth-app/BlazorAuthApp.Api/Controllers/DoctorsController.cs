using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlazorAuthApp.Api.Data;
using BlazorAuthApp.Api.DTOs;

namespace BlazorAuthApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DoctorsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public DoctorsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/doctors
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DoctorDto>>> GetDoctors()
    {
        var doctors = await _context.Doctors
            .Where(d => d.IsAvailable)
            .Select(d => new DoctorDto
            {
                Id = d.Id,
                FirstName = d.FirstName,
                LastName = d.LastName,
                Specialization = d.Specialization,
                Description = d.Description,
                Email = d.Email,
                Phone = d.Phone,
                ImageUrl = d.ImageUrl,
                ConsultationFee = d.ConsultationFee,
                YearsOfExperience = d.YearsOfExperience,
                IsAvailable = d.IsAvailable
            })
            .ToListAsync();

        return Ok(doctors);
    }

    // GET: api/doctors/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<DoctorDto>> GetDoctor(int id)
    {
        var doctor = await _context.Doctors.FindAsync(id);

        if (doctor == null)
        {
            return NotFound(new { message = "Doctor not found" });
        }

        var doctorDto = new DoctorDto
        {
            Id = doctor.Id,
            FirstName = doctor.FirstName,
            LastName = doctor.LastName,
            Specialization = doctor.Specialization,
            Description = doctor.Description,
            Email = doctor.Email,
            Phone = doctor.Phone,
            ImageUrl = doctor.ImageUrl,
            ConsultationFee = doctor.ConsultationFee,
            YearsOfExperience = doctor.YearsOfExperience,
            IsAvailable = doctor.IsAvailable
        };

        return Ok(doctorDto);
    }

    // GET: api/doctors/specializations
    [HttpGet("specializations")]
    public async Task<ActionResult<IEnumerable<string>>> GetSpecializations()
    {
        var specializations = await _context.Doctors
            .Where(d => d.IsAvailable)
            .Select(d => d.Specialization)
            .Distinct()
            .OrderBy(s => s)
            .ToListAsync();

        return Ok(specializations);
    }

    // GET: api/doctors/search?specialization=Cardiology
    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<DoctorDto>>> SearchDoctors([FromQuery] string? specialization)
    {
        var query = _context.Doctors.Where(d => d.IsAvailable);

        if (!string.IsNullOrEmpty(specialization))
        {
            query = query.Where(d => d.Specialization == specialization);
        }

        var doctors = await query
            .Select(d => new DoctorDto
            {
                Id = d.Id,
                FirstName = d.FirstName,
                LastName = d.LastName,
                Specialization = d.Specialization,
                Description = d.Description,
                Email = d.Email,
                Phone = d.Phone,
                ImageUrl = d.ImageUrl,
                ConsultationFee = d.ConsultationFee,
                YearsOfExperience = d.YearsOfExperience,
                IsAvailable = d.IsAvailable
            })
            .ToListAsync();

        return Ok(doctors);
    }
}
