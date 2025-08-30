using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SmartWings.Application.Contracts;
using SmartWings.Application.DTO;
using SmartWings.Domain;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SmartWings.API.Controllers
{
    [ApiController] // 🔹 API Controller for Authentication
    [Route("api/auth")] // 🔹 Base route for authentication endpoints
    public class AuthController : ControllerBase // 🔹 Controller for managing authentication operations
    {
        private readonly IAuthService _authService; // 🔹 Service for handling authentication logic
        private readonly IConfiguration _configuration; // 🔹 Configuration for JWT settings

        public AuthController(IAuthService authService, IConfiguration configuration) // 🔹 Constructor to inject dependencies
        {
            _authService = authService; // 🔹 Initialize the authentication service
            _configuration = configuration; // 🔹 Initialize the configuration for JWT settings
        }

        // Register a new user
        // This endpoint allows new users to register by providing their details.
        // It checks if the user already exists and returns an error if so.
        // If the registration is successful, it returns the created user details.
        [HttpPost("register")] // 🔹 Endpoint for user registration
        public async Task<IActionResult> Register([FromBody] UserRegisterDto dto)
        {
            var exists = await _authService.UserExistsAsync(new UserExistsDto { Email = dto.Email });
            if (exists)
                return BadRequest("Email already exists.");

            var user = await _authService.RegisterAsync(dto);
            return Ok(user);
        }

        // 🔹 Login and generate JWT token
        // This endpoint allows users to log in by providing their email and password.
        // If the credentials are valid, it generates a JWT token and returns it along with user details.
        // If the credentials are invalid, it returns an unauthorized error.
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] UserLoginDto login)
        {
            if (login == null || string.IsNullOrEmpty(login.Email) || string.IsNullOrEmpty(login.Password))
                return BadRequest("Email and Password are required.");

            var user = await _authService.LoginAsync(login); // Expected to return UserReadDto

            if (user == null)
                return Unauthorized("Invalid credentials.");

            var token = GenerateJwtToken(user);

            return Ok(new
            {
                Token = token,
                User = user
            });
        }

        // 🔹 Logout
        // This endpoint allows users to log out.
        // It calls the logout method in the authentication service.
        // It returns a success message upon successful logout.
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutAsync();
            return Ok("Logged out successfully.");
        }

        // 🔹 Request OTP for password reset 
        [HttpPost("request-otp")]
        [AllowAnonymous]
        public async Task<IActionResult> RequestOtp([FromBody] RequestOtpDto dto)
        {
            if (string.IsNullOrEmpty(dto.Email))
                return BadRequest("Email is required.");
        
            var result = await _authService.RequestOtpAsync(dto);
            return result ? Ok("OTP sent to your email.") : NotFound("User not found.");
        }
        
        // 🔹 Verify OTP for password reset
        [HttpPost("verify-otp")]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpDto dto)
        {
            if (string.IsNullOrEmpty(dto.Email) || string.IsNullOrEmpty(dto.Otp))
                return BadRequest("Email and OTP are required.");
        
            var result = await _authService.VerifyOtpAsync(dto);
            return result ? Ok("OTP verified successfully.") : BadRequest("Invalid or expired OTP.");
        }
        
        // 🔹 Reset password with OTP verification for password reset
        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordWithOtpDto dto)
        {
            if (string.IsNullOrEmpty(dto.Email) || string.IsNullOrEmpty(dto.Otp) || string.IsNullOrEmpty(dto.NewPassword))
                return BadRequest("Email, OTP, and new password are required.");
        
            var result = await _authService.ResetPasswordWithOtpAsync(dto);
            
            if (result == ResetPasswordResult.Success)
                return Ok("Password reset successfully.");
            else if (result == ResetPasswordResult.InvalidOtp)
                return BadRequest("Invalid or expired OTP.");
            else
                return NotFound("User not found.");
        }
        
        // 🔹 Request OTP for pre-registration
        [HttpPost("request-otp-pre-registration")]
       public async Task<IActionResult> RequestOtpPreRegistration([FromBody] RequestOtpPreRegistrationDto request)
       {
           try
           {
               if (string.IsNullOrEmpty(request.Email) || !IsValidEmailFormat(request.Email))
               {
                   return BadRequest(new { message = "Invalid email format" });
               }
       
               var userExists = await _authService.UserExistsAsync(new UserExistsDto { Email = request.Email });
               if (userExists)
               {
                   return BadRequest(new { message = "User already exists with this email" });
               }
       
               // Use the new pre-registration OTP method
               var result = await _authService.RequestOtpPreRegistrationAsync(new RequestOtpDto { Email = request.Email });
       
               if (result)
               {
                   return Ok(new { message = "OTP sent successfully for pre-registration verification" });
               }
               else
               {
                   return StatusCode(500, new { message = "Failed to send OTP" });
               }
           }
           catch (Exception ex)
           {
               return StatusCode(500, new { message = "Internal server error" });
           }
       }
       
       private bool IsValidEmailFormat(string email)
       {
           try
           {
               var addr = new System.Net.Mail.MailAddress(email);
               return addr.Address == email;
           }
           catch
           {
               return false;
           }
       }
       
        // 🔹 Verify OTP for pre-registration
        [HttpPost("verify-otp-pre-registration")]
        public async Task<IActionResult> VerifyOtpPreRegistration([FromBody] VerifyOtpPreRegistrationDto request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Otp))
                {
                    return BadRequest(new { message = "OTP is required" });
                }
        
                var result = await _authService.VerifyOtpPreRegistrationAsync(request.Otp);
        
                if (result)
                {
                    return Ok(new { message = "OTP verified successfully for pre-registration" });
                }
                else
                {
                    return BadRequest(new { message = "Invalid or expired OTP" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error" });
            }
        }


        // 🔹 Check if user exists
        // This endpoint checks if a user exists by their email.
        // It returns a boolean indicating whether the user exists or not.
        // This is useful for registration and login processes.
        [HttpPost("exists")]
        [AllowAnonymous]
        public async Task<IActionResult> UserExists([FromBody] UserExistsDto dto)
        {
            var exists = await _authService.UserExistsAsync(dto);
            return Ok(new { Exists = exists });
        }

        // 🔐 JWT Token Generator (API Layer)
        private string GenerateJwtToken(UserReadDto user)
        {
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
            //header part
            // // Create a signing key using the symmetric security key
            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256
            );
            //payload part
            // Create claims for the user
            var claims = new[]
            {
                        new Claim(ClaimTypes.Name,user.UserName),
                        new Claim(ClaimTypes.Role, user.Role),
                    };
            // Set the expiration time for the token
            var expires = DateTime.UtcNow.AddMinutes(10);
            //signature part
            // Create the JWT token
            var token = new JwtSecurityToken(
        issuer: issuer,
        audience: audience,
        claims: claims,
        expires: expires,
        signingCredentials: signingCredentials
    );
            // Serialize the token to a string
            var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenStr;
        }
    }
}
