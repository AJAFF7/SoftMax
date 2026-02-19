using System.ComponentModel.DataAnnotations;

namespace BlazorAuthApp.Api.DTOs;

public class LoginRequest
{
    [Required]
    public string Username { get; set; } = string.Empty;
    
    [Required]
    public string Password { get; set; } = string.Empty;
}

public class RegisterRequest
{
    [Required]
    [MinLength(3)]
    [MaxLength(100)]
    public string Username { get; set; } = string.Empty;
    
    [Required]
    [MinLength(4)]
    public string Password { get; set; } = string.Empty;
}

public class AuthResponse
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public UserDto? User { get; set; }
}

public class UserDto
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
}
