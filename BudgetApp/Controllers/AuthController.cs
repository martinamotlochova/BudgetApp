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
        private readonly RefreshTokenService _refreshTokenService;
        private readonly AuthValidationService _validationService = new AuthValidationService();

        public AuthController(AppDbContext context, JwtService jwtService, RefreshTokenService refreshTokenService, AuthValidationService validationService)
        {
            _context = context;
            _jwtService = jwtService;
            _refreshTokenService = refreshTokenService;
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

            var access = _jwtService.GenerateToken(user);
            var refreshtoken = await _refreshTokenService.CreateRefreshTokenAsync(user);

            return Ok(new LoginResponse
            {
                AccessToken = access.Token,
                AccessTokenExpiresAt = access.ExpiresAt,
                RefreshToken = refreshtoken,
                User = user.ToDto()
            });
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

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh (RefreshRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.RefreshToken)) return BadRequest("Refresh token is required.");

            var result = await _refreshTokenService.RotateRefreshTokenAsync(request.RefreshToken);
            if (result == null) return Unauthorized();

            var access = _jwtService.GenerateToken(result.Value.user);

            return Ok(new LoginResponse
            {
                AccessToken = access.Token,
                AccessTokenExpiresAt = access.ExpiresAt,
                RefreshToken = result.Value.RefreshToken,
                User = result.Value.user.ToDto()
            });
        }

    }
}
