using SmartWings.Application.Contracts;
using SmartWings.Application.DTO;
using SmartWings.Domain;
using SmartWings.Infrastructure.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SmartWings.Application.Services
{
    // Service for managing user operations such as retrieval, update, and deletion.
    public class UserService : IUserService // Implements IUserService to provide user management functionalities
    {
        private readonly IUserRepository _userRepository; // Repository for accessing user data

        public UserService(IUserRepository userRepository) // Constructor to initialize the service with the user repository
        {
            _userRepository = userRepository; // Initialize the user repository
        }


        // Retrieves a user by their unique identifier.
        // Returns a UserReadDto if found, otherwise returns null.
        public async Task<UserReadDto> GetUserByIdAsync(Guid userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            return user == null ? null : MapToReadDto(user);
        }

        // Retrieves all active users from the repository.
        // Returns a collection of UserReadDto.
        // If no users are found, returns an empty collection.
        public async Task<IEnumerable<UserReadDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return users.Select(MapToReadDto);
        }

        // Updates an existing user's details.
        // Accepts an UpdateUserDto containing the user's new details.
        // If the user is found and the current password is valid, updates the user's details.
        public async Task<UserReadDto> UpdateUserAsync(UpdateUserDto dto)
        {
            var user = await _userRepository.GetUserByIdAsync(dto.UserId);
            if (user == null) return null;

            // Optionally validate current password
            if (!string.IsNullOrEmpty(dto.CurrentPassword))
            {
                var isValid = VerifyPassword(dto.CurrentPassword, user.PasswordHash);
                if (!isValid) return null;
            }

            user.UserName = dto.UserName;
            user.Email = dto.Email;

            if (!string.IsNullOrWhiteSpace(dto.NewPassword))
            {
                user.PasswordHash = HashPassword(dto.NewPassword);
            }

            var updatedUser = await _userRepository.UpdateUserAsync(user);
            return MapToReadDto(updatedUser);
        }

        // Deletes a user by setting IsActive to false based on their email.
        // Returns true if the user was found and deleted, otherwise returns false.
        public async Task<bool> DeleteUserByEmailAsync(string email)
        {
            return await _userRepository.DeleteUserByEmailAsync(email);
        }

        // Helpers
        // Maps a User entity to a UserReadDto for returning user data.
        // This is used to convert the domain model to a data transfer object for API responses.
        // This method extracts only the necessary fields to be returned in the API response.
        private UserReadDto MapToReadDto(User user)
        {
            return new UserReadDto
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Email = user.Email,
                Role = user.Role,
                CreatedAt = user.CreatedAt,
                IsActive = user.IsActive
            };
        }

        // Hashes a password using SHA256 and returns the base64 encoded hash.
        // This method is used to securely store passwords in the database.
        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        // Verifies the provided password against the stored hash.
        // Returns true if the password matches, otherwise returns false.
        private bool VerifyPassword(string password, string storedHash)
        {
            var hash = HashPassword(password);
            return hash == storedHash;
        }

    }
}
