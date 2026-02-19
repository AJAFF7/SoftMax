namespace BlazorAuthApp.Models;

public class Doctor
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
    
    public string FullName => $"{FirstName} {LastName}";
}
