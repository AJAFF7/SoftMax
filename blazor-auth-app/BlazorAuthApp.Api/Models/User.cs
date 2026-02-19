using System.ComponentModel.DataAnnotations;

namespace BlazorAuthApp.Api.Models;

public class User
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Username { get; set; } = string.Empty;
    
    [Required]
    public string PasswordHash { get; set; } = string.Empty;
    
    [MaxLength(100)]
    public string? FirstName { get; set; }
    
    [MaxLength(100)]
    public string? LastName { get; set; }
    
    [MaxLength(200)]
    public string? Email { get; set; }
    
    [MaxLength(20)]
    public string? Phone { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation property
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
