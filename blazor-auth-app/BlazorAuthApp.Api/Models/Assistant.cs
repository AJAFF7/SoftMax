using System.ComponentModel.DataAnnotations;

namespace BlazorAuthApp.Api.Models
{
    public class Assistant
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string ETagBarcode { get; set; } = string.Empty;

        // Face recognition descriptor (stored as JSON)
        public string? FaceDescriptor { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; } = true;

        // Navigation property for appointments checked in by this assistant
        public ICollection<Appointment>? CheckedInAppointments { get; set; }
    }
}
