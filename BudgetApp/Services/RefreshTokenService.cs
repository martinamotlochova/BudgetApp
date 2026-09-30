using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BudgetApp.Models;
using System.Security.Cryptography;
using BudgetApp.Data;

namespace BudgetApp.Services
{
    public class RefreshTokenService
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;

        public RefreshTokenService(IConfiguration configuration, AppDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public async Task<string> CreateRefreshTokenAsync(User user)
        {
            var rawToken = GenerateRefreshToken();

            var days = double.Parse(_configuration["Jwt:RefreshTokenExpirationDays"]!);

            var entity = new RefreshToken
            {
                UserId = user.Id,
                TokenHash = HashToken(rawToken),
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(days)
            };

            _context.RefreshTokens.Add(entity);
            await _context.SaveChangesAsync();

            return rawToken;
        }

        public string GenerateRefreshToken()
        {
            var bytes = RandomNumberGenerator.GetBytes(32);
            return Convert.ToBase64String(bytes);
        }

        public string HashToken(string token)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(token));
            return Convert.ToHexString(bytes);
        }

    }
}
