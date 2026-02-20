using System.ComponentModel.DataAnnotations;

namespace BlazorAuthApp.Api.DTOs;

// Doctor DTOs
public class DoctorDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Specialization { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public decimal ConsultationFee { get; set; }
    public int YearsOfExperience { get; set; }
    public bool IsAvailable { get; set; }
}

// Appointment DTOs
public class AppointmentDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int DoctorId { get; set; }
    public DateTime AppointmentDate { get; set; }
    public string TimeSlot { get; set; } = string.Empty;
    public string PatientName { get; set; } = string.Empty;
    public string PatientEmail { get; set; } = string.Empty;
    public string PatientPhone { get; set; } = string.Empty;
    public string Symptoms { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public bool PatientArrived { get; set; }
    public DateTime? PatientArrivedAt { get; set; }
    public bool IsFinished { get; set; }
    public DateTime? FinishedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public int? CheckedInByAssistantId { get; set; }
    public string? CheckedInByAssistantName { get; set; }
    public string? CheckedInByAssistantBarcode { get; set; }
    
    // Include doctor information
    public DoctorDto? Doctor { get; set; }
}

public class CreateAppointmentDto
{
    [Required]
    public int DoctorId { get; set; }
    
    [Required]
    public DateTime AppointmentDate { get; set; }
    
    [Required]
    public string TimeSlot { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(100)]
    public string PatientName { get; set; } = string.Empty;
    
    [Required]
    [EmailAddress]
    [MaxLength(200)]
    public string PatientEmail { get; set; } = string.Empty;
    
    [Required]
    [Phone]
    [MaxLength(20)]
    public string PatientPhone { get; set; } = string.Empty;
    
    [MaxLength(500)]
    public string Symptoms { get; set; } = string.Empty;
}

public class UpdateAppointmentStatusDto
{
    [Required]
    public string Status { get; set; } = string.Empty;
}

public class CheckInDto
{
    [Required]
    public int AssistantId { get; set; }
}
