using Microsoft.EntityFrameworkCore;
using SmartWings.Domain;
using SmartWings.Infrastructure.Contracts;
using SmartWings.Infrastructure.DataContext;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartWings.Infrastructure.Repositories
{
    // Summary:
    // The UserRepository class implements IUserRepository to manage user data in the database.
    public class UserRepository : IUserRepository // Repository for user data management
    {
        private readonly FlightDbContext _context; // Database context for accessing user data

        public UserRepository(FlightDbContext context) // Constructor to initialize the repository with the database context
        {
            _context = context;
        }


        // Retrieves a user by their unique identifier.
        // Returns the user if found and active, otherwise returns null.
        public async Task<User> GetUserByIdAsync(Guid userId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId && u.IsActive);
        }

        // Retrieves all active users from the database.
        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users
                .Where(u => u.IsActive)
                .ToListAsync();
        }

        // Updates an existing user's details.
        // Returns the updated user if successful, otherwise returns null.
        // If the user does not exist or is inactive, it returns null.
        // If the password hash is not provided, it retains the existing hash.

        public async Task<User> UpdateUserAsync(User updatedUser)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.UserId == updatedUser.UserId);

            if (existingUser == null || !existingUser.IsActive)
                return null;

            // Update fields
            existingUser.UserName = updatedUser.UserName;
            existingUser.Email = updatedUser.Email;
            existingUser.Role = updatedUser.Role;
            existingUser.PasswordHash = updatedUser.PasswordHash ?? existingUser.PasswordHash;

            await _context.SaveChangesAsync();
            return existingUser;
        }

        // Deletes a user by setting IsActive to false based on their email.
        // Returns true if the user was found and deleted, otherwise returns false.
        public async Task<bool> DeleteUserByEmailAsync(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.IsActive);
            if (user == null) return false;

            user.IsActive = false; // soft delete
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
