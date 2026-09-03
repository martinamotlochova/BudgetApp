using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BudgetApp.Data;
using BudgetApp.Models;
using BudgetApp.DTOs;



namespace BudgetApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController :ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
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

    }
}
