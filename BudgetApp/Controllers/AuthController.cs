using BudgetApp.Data;
using BudgetApp.DTOs;
using BudgetApp.Models;
using BudgetApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;



namespace BudgetApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController :ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly JwtService _jwtService;

        public AuthController(AppDbContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
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

            string token = _jwtService.GenerateToken(user);

            return Ok(new { token, user = user.ToDto() });
        }

    }
}
