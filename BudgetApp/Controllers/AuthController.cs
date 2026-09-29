using BudgetApp.Data;
using BudgetApp.DTOs;
using BudgetApp.Models;
using BudgetApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;



namespace BudgetApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController :ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly JwtService _jwtService;
        private readonly AuthValidationService _validationService = new AuthValidationService();

        public AuthController(AppDbContext context, JwtService jwtService, AuthValidationService validationService)
        {
            _context = context;
            _jwtService = jwtService;
            _validationService = validationService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var validationError = _validationService.ValidateRegisterRequest(request);
            if (validationError != null)
            {
                return BadRequest(validationError);
            }

            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
            {
                return BadRequest("Email is already in use.");
            }
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            var user = request.ToEntity(passwordHash);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok(user.ToDto());
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var validationError = _validationService.ValidateLoginRequest(request);
            if (validationError != null)
            {
                return BadRequest(validationError);
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null)
            {
                return BadRequest("Wrong email or password.");
            }

            bool isCorrect = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
            if (!isCorrect)
            {
                return BadRequest("Wrong email or password.");
            }

            var result = _jwtService.GenerateToken(user);

            return Ok(new LoginResponse { Token = result.Token, User = user.ToDto() });
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            string? userIdText = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int userId = int.Parse(userIdText!);

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user.ToDto());
        }

    }
}
