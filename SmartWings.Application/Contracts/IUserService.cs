using SmartWings.Application.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartWings.Application.Contracts
{
    public interface IUserService
    {

        Task<UserReadDto> GetUserByIdAsync(Guid userId); // Retrieve user by unique identifier

        Task<IEnumerable<UserReadDto>> GetAllUsersAsync(); // Retrieve all active users

        Task<UserReadDto> UpdateUserAsync(UpdateUserDto dto); // Update user details, including username, email, role, and password hash

        Task<bool> DeleteUserByEmailAsync(string email); // Delete user

    }
}
