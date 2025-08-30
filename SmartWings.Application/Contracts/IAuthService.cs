using SmartWings.Application.DTO;
using SmartWings.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWings.Application.Contracts
{
    public interface IAuthService
    {
        Task<UserReadDto> RegisterAsync(UserRegisterDto dto); // Register a new user with a hashed password
        Task<UserReadDto> LoginAsync(UserLoginDto dto); // Log in a user by verifying email and password
        Task LogoutAsync(); // Log out a user (placeholder for future implementation)
        Task<bool> RequestOtpAsync(RequestOtpDto dto); // Request an OTP for password reset
        Task<bool> VerifyOtpAsync(VerifyOtpDto dto); // Verify the OTP sent to the user
        Task<ResetPasswordResult> ResetPasswordWithOtpAsync(ResetPasswordWithOtpDto dto); // Reset password using OTP verification
        Task<bool> RequestOtpPreRegistrationAsync(RequestOtpDto dto); // Request an OTP for pre-registration
        Task<bool> VerifyOtpPreRegistrationAsync(string otp); // Verify the OTP for pre-registration
        Task<bool> UserExistsAsync(UserExistsDto dto); // Check if a user exists by email
    }
}