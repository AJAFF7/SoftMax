using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorAuthApp.Api.Models;

public class Appointment
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public int UserId { get; set; }
    
    [Required]
    public int DoctorId { get; set; }
    
    [Required]
    public DateTime AppointmentDate { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string TimeSlot { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(100)]
    public string PatientName { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(200)]
    public string PatientEmail { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(20)]
    public string PatientPhone { get; set; } = string.Empty;
    
    [MaxLength(500)]
    public string Symptoms { get; set; } = string.Empty;
    
    [MaxLength(50)]
    public string Status { get; set; } = "Pending"; // Pending, Confirmed, Completed, Cancelled
    
    public bool PatientArrived { get; set; } = false;
    
    public DateTime? PatientArrivedAt { get; set; }
    
    public bool IsFinished { get; set; } = false;
    
    public DateTime? FinishedAt { get; set; }
    
    public int? CheckedInByAssistantId { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? UpdatedAt { get; set; }
    
    // Navigation properties
    [ForeignKey("UserId")]
    public User User { get; set; } = null!;
    
    [ForeignKey("DoctorId")]
    public Doctor Doctor { get; set; } = null!;
    
    [ForeignKey("CheckedInByAssistantId")]
    public Assistant? CheckedInByAssistant { get; set; }
}
