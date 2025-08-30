using Microsoft.AspNetCore.Mvc;
using SmartWings.Application.Contracts;
using SmartWings.Application.DTO;

namespace SmartWings.API.Controllers
{
    [ApiController] // Base controller for API endpoints
    [Route("api/users")] // Route for user-related operations
    public class UserController : ControllerBase // Controller for managing user operations
    {
        private readonly IUserService _userService; // Service for user management operations

        public UserController(IUserService userService) // Constructor to inject the user service dependency
        {
            _userService = userService; // Initialize the user service
        }


        // Get all users
        // This endpoint retrieves all active users from the system.
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        // Get user by ID
        // This endpoint retrieves a user by their unique identifier.
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserById(Guid userId)
        {
            var user = await _userService.GetUserByIdAsync(userId);
            return user == null ? NotFound() : Ok(user);
        }

        // Update user
        // This endpoint updates a user's details, including username, email, role, and password hash.
        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUser(Guid userId, [FromBody] UpdateUserDto dto)
        {
            if (userId != dto.UserId)
                return BadRequest("User ID mismatch.");

            var updatedUser = await _userService.UpdateUserAsync(dto);
            return updatedUser == null ? NotFound() : Ok(updatedUser);
        }

        // Delete user
        // This endpoint deletes a user by setting their IsActive status to false based on their email.
        [HttpDelete("by-email")]
        public async Task<IActionResult> DeleteUserByEmail([FromBody] UserDeleteByEmailDto dto)
        {
            var deleted = await _userService.DeleteUserByEmailAsync(dto.Email);
            return deleted ? NoContent() : NotFound("User not found or already inactive.");
        }

    }
}
