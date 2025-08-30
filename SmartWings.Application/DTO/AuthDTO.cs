using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SmartWings.Application.DTO
{
    // DTO used during user registration
    public class UserRegisterDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } // "User" or "Admin"
    }

    // DTO used for login
    public class UserLoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    // DTO used to reset password
    public class ResetPasswordDto
    {
        public string Email { get; set; }
        public string NewPassword { get; set; }
    }
    
    // RequestOtpDto
    public class RequestOtpDto
    {
        public string Email { get; set; }
    }
    
    // VerifyOtpDto
    public class VerifyOtpDto
    {
        public string Email { get; set; }
        public string Otp { get; set; }
    }
    
    // ResetPasswordWithOtpDto
    public class ResetPasswordWithOtpDto
    {
        public string Email { get; set; }
        public string Otp { get; set; }
        public string NewPassword { get; set; }
    }
    
    // ResetPasswordResult
    public enum ResetPasswordResult
    {
        Success,
        UserNotFound,
        InvalidOtp
    }
    
    // DTO used to request OTP for pre-registration
    public class RequestOtpPreRegistrationDto
    {
        public string Email { get; set; }
    }
    
    // DTO used to verify OTP for pre-registration
    public class VerifyOtpPreRegistrationDto
    {
        public string Otp { get; set; }
    }

    // DTO used to check if a user exists by email
    public class UserExistsDto
    {
        public string Email { get; set; }
    }
}