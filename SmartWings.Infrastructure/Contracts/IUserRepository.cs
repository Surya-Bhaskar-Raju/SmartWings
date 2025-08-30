using SmartWings.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartWings.Infrastructure.Contracts
{
    public interface IUserRepository
    {

        Task<User> GetUserByIdAsync(Guid userId); // Retrieve user by unique identifier
        Task<User> UpdateUserAsync(User user); // Update user details, including username, email, role, and password hash
        Task<bool> DeleteUserByEmailAsync(string email); // Soft delete by setting IsActive to false
        Task<List<User>> GetAllUsersAsync();   // Retrieve all active users

    }
}
