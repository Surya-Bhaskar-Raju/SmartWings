namespace SmartWings.Application.Contracts;

public interface IEmailService // This interface defines the contract for email service operations (For OTP and other email notifications) (For Password Reset)
{
    Task<bool> SendOtpEmailAsync(string email, string otp);
}