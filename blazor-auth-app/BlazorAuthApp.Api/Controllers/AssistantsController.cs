using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlazorAuthApp.Api.Data;
using BlazorAuthApp.Api.Models;
using BlazorAuthApp.Api.DTOs;
using System.Security.Cryptography;
using System.Text;
using QRCoder;

namespace BlazorAuthApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssistantsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AssistantsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/assistants/register
        [HttpPost("register")]
        public async Task<ActionResult<AssistantResponseDto>> Register(AssistantRegisterDto dto)
        {
            // Check if username already exists
            if (await _context.Assistants.AnyAsync(a => a.Username == dto.Username))
            {
                return BadRequest("Username already exists");
            }

            // Check if email already exists
            if (await _context.Assistants.AnyAsync(a => a.Email == dto.Email))
            {
                return BadRequest("Email already exists");
            }

            // Generate unique ETag barcode
            var eTagBarcode = GenerateETagBarcode();

            // Hash the password
            var passwordHash = HashPassword(dto.Password);

            var assistant = new Assistant
            {
                FullName = dto.FullName,
                Email = dto.Email,
                Username = dto.Username,
                PasswordHash = passwordHash,
                PhoneNumber = dto.PhoneNumber,
                ETagBarcode = eTagBarcode,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            _context.Assistants.Add(assistant);
            await _context.SaveChangesAsync();

            var response = new AssistantResponseDto
            {
                Id = assistant.Id,
                FullName = assistant.FullName,
                Email = assistant.Email,
                Username = assistant.Username,
                PhoneNumber = assistant.PhoneNumber,
                ETagBarcode = assistant.ETagBarcode,
                CreatedAt = assistant.CreatedAt,
                IsActive = assistant.IsActive
            };

            return CreatedAtAction(nameof(GetAssistant), new { id = assistant.Id }, response);
        }

        // POST: api/assistants/login
        [HttpPost("login")]
        public async Task<ActionResult<AssistantResponseDto>> Login(AssistantLoginDto dto)
        {
            Console.WriteLine($"[AssistantsController] Login attempt - Username/Barcode: '{dto.Username}', Password length: {dto.Password?.Length ?? 0}");

            // Try to find assistant by username OR barcode
            var assistant = await _context.Assistants
                .FirstOrDefaultAsync(a => a.Username == dto.Username || a.ETagBarcode == dto.Username);

            if (assistant == null)
            {
                Console.WriteLine($"[AssistantsController] User not found with username/barcode: {dto.Username}");
                return Unauthorized("Invalid username or password");
            }

            Console.WriteLine($"[AssistantsController] User found: {assistant.Username}, verifying password...");
            if (string.IsNullOrEmpty(dto.Password) || !VerifyPassword(dto.Password, assistant.PasswordHash))
            {
                Console.WriteLine($"[AssistantsController] Password verification failed");
                return Unauthorized("Invalid username or password");
            }

            if (!assistant.IsActive)
            {
                Console.WriteLine($"[AssistantsController] Account is deactivated");
                return Unauthorized("Account is deactivated");
            }

            Console.WriteLine($"[AssistantsController] Login successful for: {assistant.Username}");

            var response = new AssistantResponseDto
            {
                Id = assistant.Id,
                FullName = assistant.FullName,
                Email = assistant.Email,
                Username = assistant.Username,
                PhoneNumber = assistant.PhoneNumber,
                ETagBarcode = assistant.ETagBarcode,
                CreatedAt = assistant.CreatedAt,
                IsActive = assistant.IsActive
            };

            return Ok(response);
        }

        // POST: api/assistants/login-barcode
        [HttpPost("login-barcode")]
        public async Task<ActionResult<AssistantResponseDto>> LoginByBarcode([FromBody] string barcode)
        {
            var assistant = await _context.Assistants
                .FirstOrDefaultAsync(a => a.ETagBarcode == barcode);

            if (assistant == null)
            {
                return Unauthorized("Invalid barcode");
            }

            if (!assistant.IsActive)
            {
                return Unauthorized("Account is deactivated");
            }

            var response = new AssistantResponseDto
            {
                Id = assistant.Id,
                FullName = assistant.FullName,
                Email = assistant.Email,
                Username = assistant.Username,
                PhoneNumber = assistant.PhoneNumber,
                ETagBarcode = assistant.ETagBarcode,
                CreatedAt = assistant.CreatedAt,
                IsActive = assistant.IsActive
            };

            return Ok(response);
        }

        // GET: api/assistants/qrcode/{email}
        [HttpGet("qrcode/{email}")]
        public async Task<IActionResult> GetQRCode(string email)
        {
            var assistant = await _context.Assistants
                .FirstOrDefaultAsync(a => a.Email == email);

            if (assistant == null)
            {
                return NotFound("Assistant not found");
            }

            using var qrGenerator = new QRCodeGenerator();
            using var qrCodeData = qrGenerator.CreateQrCode(assistant.ETagBarcode, QRCodeGenerator.ECCLevel.Q);
            using var qrCode = new PngByteQRCode(qrCodeData);
            var qrCodeImage = qrCode.GetGraphic(20);

            return File(qrCodeImage, "image/png");
        }

        // GET: api/assistants/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<AssistantResponseDto>> GetAssistant(int id)
        {
            var assistant = await _context.Assistants.FindAsync(id);

            if (assistant == null)
            {
                return NotFound();
            }

            var response = new AssistantResponseDto
            {
                Id = assistant.Id,
                FullName = assistant.FullName,
                Email = assistant.Email,
                Username = assistant.Username,
                PhoneNumber = assistant.PhoneNumber,
                ETagBarcode = assistant.ETagBarcode,
                CreatedAt = assistant.CreatedAt,
                IsActive = assistant.IsActive
            };

            return Ok(response);
        }

        // GET: api/assistants
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AssistantResponseDto>>> GetAssistants()
        {
            var assistants = await _context.Assistants
                .Where(a => a.IsActive)
                .Select(a => new AssistantResponseDto
                {
                    Id = a.Id,
                    FullName = a.FullName,
                    Email = a.Email,
                    Username = a.Username,
                    PhoneNumber = a.PhoneNumber,
                    ETagBarcode = a.ETagBarcode,
                    CreatedAt = a.CreatedAt,
                    IsActive = a.IsActive
                })
                .ToListAsync();

            return Ok(assistants);
        }

        // Helper methods
        private string GenerateETagBarcode()
        {
            // Format: AST-YYYYMMDD-XXXXX (AST = Assistant)
            var date = DateTime.UtcNow.ToString("yyyyMMdd");
            var random = new Random();
            var number = random.Next(10000, 99999);
            return $"AST-{date}-{number}";
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        private bool VerifyPassword(string password, string hash)
        {
            var hashOfInput = HashPassword(password);
            return hashOfInput == hash;
        }
    }
}
