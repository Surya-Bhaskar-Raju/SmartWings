using SmartWings.Domain;
using System;
using System.Threading.Tasks;

namespace SmartWings.Infrastructure.Contracts
{
    public interface IAuthRepository
    {
        Task<User> RegisterAsync(User user, string password);
        Task<User> LoginAsync(string email, string password);
        Task<bool> UserExistsAsync(string email);
        Task LogoutAsync();
        Task<bool> ResetPasswordAsync(string email, string newPassword);
    }
}