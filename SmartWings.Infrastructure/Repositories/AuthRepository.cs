using Microsoft.EntityFrameworkCore;
using SmartWings.Domain;
using SmartWings.Infrastructure.Contracts;
using SmartWings.Infrastructure.DataContext;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SmartWings.Infrastructure.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly FlightDbContext _context;

        public AuthRepository(FlightDbContext context)
        {
            _context = context;
        }

        public async Task<User> RegisterAsync(User user, string password)
        {
            user.PasswordHash = HashPassword(password);
            user.CreatedAt = DateTime.UtcNow;
            user.IsActive = true;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<User> LoginAsync(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null || !VerifyPassword(password, user.PasswordHash))
                return null;

            return user;
        }

        public async Task<bool> UserExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public Task LogoutAsync()
        {
            // Placeholder for logout — will be useful when JWT or session is added.
            return Task.CompletedTask;
        }

        public async Task<bool> ResetPasswordAsync(string email, string newPassword)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return false;

            user.PasswordHash = HashPassword(newPassword);
            await _context.SaveChangesAsync();
            return true;
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            var hash = HashPassword(password);
            return hash == storedHash;
        }
    }
}