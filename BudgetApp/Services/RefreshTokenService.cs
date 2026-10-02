using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BudgetApp.Models;
using System.Security.Cryptography;
using BudgetApp.Data;
using BudgetApp.DTOs;
using Microsoft.EntityFrameworkCore;

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

        public async Task<(User user, string RefreshToken)?> RotateRefreshTokenAsync(string rawToken)
        {
            var hash = HashToken(rawToken);

            var stored = await _context.RefreshTokens.Include(t => t.User).FirstOrDefaultAsync(t => t.TokenHash == hash);

            if (stored == null) return null;

            if (stored.RevokedAt != null)
            {
                var active = await _context.RefreshTokens.Where(t => t.UserId == stored.UserId && stored.RevokedAt == null).ToListAsync();

                foreach (var token in active)
                {
                    token.RevokedAt = DateTime.UtcNow;
                }

                await _context.SaveChangesAsync();
                return null;
            }

            if (stored.ExpiresAt < DateTime.UtcNow) return null;

            stored.RevokedAt = DateTime.UtcNow;
            var newRawToken = await CreateRefreshTokenAsync(stored.User);

            return (stored.User, newRawToken);
        }

        public async Task RevokeRefreshTokenAsync(string rawToken)
        {
            var hash = HashToken(rawToken);

            var stored = await _context.RefreshTokens.FirstOrDefaultAsync(t => t.TokenHash == hash);

            if (stored == null || stored.RevokedAt != null) return;

            stored.RevokedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
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
