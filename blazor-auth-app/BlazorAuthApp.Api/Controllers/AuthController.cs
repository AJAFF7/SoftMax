using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlazorAuthApp.Api.Data;
using BlazorAuthApp.Api.Models;
using BlazorAuthApp.Api.DTOs;
using System.Security.Cryptography;
using System.Text;

namespace BlazorAuthApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public AuthController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request)
    {
        if (await _context.Users.AnyAsync(u => u.Username.ToLower() == request.Username.ToLower()))
        {
            return Ok(new AuthResponse 
            { 
                Success = false, 
                Message = "Username already exists" 
            });
        }

        var user = new User
        {
            Username = request.Username,
            PasswordHash = HashPassword(request.Password)
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok(new AuthResponse 
        { 
            Success = true, 
            Message = "Registration successful",
            User = new UserDto 
            { 
                Id = user.Id, 
                Username = user.Username 
            }
        });
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Username.ToLower() == request.Username.ToLower());

        if (user == null || !VerifyPassword(request.Password, user.PasswordHash))
        {
            return Ok(new AuthResponse 
            { 
                Success = false, 
                Message = "Invalid username or password" 
            });
        }

        return Ok(new AuthResponse 
        { 
            Success = true, 
            Message = "Login successful",
            User = new UserDto 
            { 
                Id = user.Id, 
                Username = user.Username 
            }
        });
    }

    [HttpDelete("user/{username}")]
    public async Task<ActionResult<AuthResponse>> DeleteUser(string username)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());

        if (user == null)
        {
            return Ok(new AuthResponse 
            { 
                Success = false, 
                Message = "User not found" 
            });
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return Ok(new AuthResponse 
        { 
            Success = true, 
            Message = "User deleted successfully" 
        });
    }

    private static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }

    private static bool VerifyPassword(string password, string hash)
    {
        var hashOfInput = HashPassword(password);
        return hashOfInput == hash;
    }
}
