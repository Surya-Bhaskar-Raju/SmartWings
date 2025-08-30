using System;

namespace SmartWings.Application.DTO
{
    public class UserReadDto
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool IsActive { get; set; }
    }

    public class UpdateUserDto
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string CurrentPassword { get; set; }  // Optional -  validate before change
        public string NewPassword { get; set; }
    }
    public class UserDeleteByEmailDto
    {
        public string Email { get; set; }
    }
}
