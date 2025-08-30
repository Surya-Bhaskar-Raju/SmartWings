using SmartWings.Application.Contracts;
using SmartWings.Application.DTO;
using SmartWings.Domain;
using SmartWings.Infrastructure.Contracts;
using System;
using System.Threading.Tasks;

namespace SmartWings.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IOtpRepository _otpRepository;
        private readonly IEmailService _emailService;

        public AuthService(IAuthRepository authRepository, IOtpRepository otpRepository, IEmailService emailService)
        {
            _authRepository = authRepository;
            _otpRepository = otpRepository;
            _emailService = emailService;
        }

        public async Task<UserReadDto> RegisterAsync(UserRegisterDto dto)
        {
            var user = new User
            {
                UserId = Guid.NewGuid(),
                UserName = dto.UserName,
                Email = dto.Email,
                Role = dto.Role
            };

            var createdUser = await _authRepository.RegisterAsync(user, dto.Password);

            return new UserReadDto
            {
                UserId = createdUser.UserId,
                UserName = createdUser.UserName,
                Email = createdUser.Email,
                Role = createdUser.Role,
                CreatedAt = createdUser.CreatedAt,
                IsActive = createdUser.IsActive
            };

        }

        public async Task<UserReadDto> LoginAsync(UserLoginDto dto)
        {
            var user = await _authRepository.LoginAsync(dto.Email, dto.Password);
            if (user == null) return null;

            return new UserReadDto
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Email = user.Email,
                Role = user.Role
            };
        }

        public async Task LogoutAsync()
        {
            await _authRepository.LogoutAsync();
        }

        // Request an OTP for password reset
        public async Task<bool> RequestOtpAsync(RequestOtpDto dto)
        {
            var userExists = await UserExistsAsync(new UserExistsDto { Email = dto.Email });
            if (!userExists) return false;
        
            var otp = GenerateOtp();
            var otpRecord = new OtpRecord
            {
                Email = dto.Email,
                Otp = otp,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddMinutes(5),
                IsUsed = false
            };
        
            // Save OTP to database
            await _otpRepository.AddAsync(otpRecord);
            
            // Send OTP via email
            return await _emailService.SendOtpEmailAsync(dto.Email, otp);
        }
        
        public async Task<bool> VerifyOtpAsync(VerifyOtpDto dto)
        {
            var otpRecord = await _otpRepository.GetValidOtpAsync(dto.Email, dto.Otp);
            return otpRecord != null && !otpRecord.IsUsed && otpRecord.ExpiresAt > DateTime.UtcNow;
        }
        
        public async Task<ResetPasswordResult> ResetPasswordWithOtpAsync(ResetPasswordWithOtpDto dto)
        {
            var userExists = await UserExistsAsync(new UserExistsDto { Email = dto.Email });
            if (!userExists) return ResetPasswordResult.UserNotFound;
        
            var otpRecord = await _otpRepository.GetValidOtpAsync(dto.Email, dto.Otp);
            if (otpRecord == null || otpRecord.IsUsed || otpRecord.ExpiresAt <= DateTime.UtcNow)
                return ResetPasswordResult.InvalidOtp;
        
            // Mark OTP as used
            otpRecord.IsUsed = true;
            await _otpRepository.UpdateAsync(otpRecord);
        
            // Reset password
            var resetResult = await ResetPasswordAsync(new ResetPasswordDto 
            { 
                Email = dto.Email, 
                NewPassword = dto.NewPassword 
            });
        
            return resetResult ? ResetPasswordResult.Success : ResetPasswordResult.UserNotFound;
        }
        
        private string GenerateOtp()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString();
        }
        
        public async Task<bool> ResetPasswordAsync(ResetPasswordDto dto)
        {
            return await _authRepository.ResetPasswordAsync(dto.Email, dto.NewPassword);
        }
        
        // 🔹 Request OTP for pre-registration
        public async Task<bool> RequestOtpPreRegistrationAsync(RequestOtpDto dto)
        {
            try
            {
                // Generate OTP
                var otp = GenerateOtp();
        
                // Create OTP record for pre-registration
                var otpRecord = new OtpRecord
                {
                    Email = dto.Email,
                    Otp = otp,
                    CreatedAt = DateTime.UtcNow,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(10),
                    IsUsed = false
                };
        
                // Save OTP record
                await _otpRepository.AddAsync(otpRecord);
        
                // Send OTP email
                await _emailService.SendOtpEmailAsync(dto.Email, otp);
        
                return true;
            }
            catch
            {
                return false;
            }
        }
        
        // 🔹 Verify OTP for pre-registration
        public async Task<bool> VerifyOtpPreRegistrationAsync(string otp)
        {
            try
            {
                var otpRecord = await _otpRepository.GetByOtpAsync(otp);
        
                if (otpRecord == null || otpRecord.IsUsed || otpRecord.ExpiresAt < DateTime.UtcNow)
                {
                    return false;
                }
        
                // Mark OTP as used
                otpRecord.IsUsed = true;
                await _otpRepository.UpdateAsync(otpRecord);
        
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UserExistsAsync(UserExistsDto dto)
        {
            return await _authRepository.UserExistsAsync(dto.Email);
        }
    }
}