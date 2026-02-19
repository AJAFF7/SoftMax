using System.ComponentModel.DataAnnotations;

namespace BlazorAuthApp.Api.Models;

public class Doctor
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(100)]
    public string Specialization { get; set; } = string.Empty;
    
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;
    
    [MaxLength(200)]
    public string Email { get; set; } = string.Empty;
    
    [MaxLength(20)]
    public string Phone { get; set; } = string.Empty;
    
    [MaxLength(500)]
    public string ImageUrl { get; set; } = string.Empty;
    
    public decimal ConsultationFee { get; set; }
    
    public int YearsOfExperience { get; set; }
    
    public bool IsAvailable { get; set; } = true;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation property
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
