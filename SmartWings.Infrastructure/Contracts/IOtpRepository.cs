using SmartWings.Domain;

namespace SmartWings.Infrastructure.Contracts;

public interface IOtpRepository
{
    Task AddAsync(OtpRecord otpRecord); // Add a new OTP record (For Password Reset)
    Task<OtpRecord> GetValidOtpAsync(string email, string otp); // Get a valid OTP record by email and OTP (For Password Reset)
    Task UpdateAsync(OtpRecord otpRecord); // Update the OTP record (For Password Reset)
    Task<OtpRecord> GetByOtpAsync(string otp); // Get an OTP record by OTP (For pre-registration validation)
}