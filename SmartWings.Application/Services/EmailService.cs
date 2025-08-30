using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using SmartWings.Application.Contracts; // For IEmailService

namespace SmartWings.Application.Services;

public class EmailService : IEmailService // Implements the IEmailService interface (For sending OTP emails) (For Password Reset)
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<bool> SendOtpEmailAsync(string email, string otp)
    {
        try
        {
            var smtpClient = new SmtpClient(_configuration["Email:SmtpServer"])
            {
                Port = int.Parse(_configuration["Email:Port"]),
                Credentials = new NetworkCredential(_configuration["Email:Username"], _configuration["Email:Password"]),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_configuration["Email:FromAddress"]),
                Subject = "SmartWings services OTP",
                Body = $"Your OTP for SmartWings Services is: {otp}. This OTP will expire in 5 minutes.",
                IsBodyHtml = false,
            };
            mailMessage.To.Add(email);

            await smtpClient.SendMailAsync(mailMessage);
            return true;
        }
        catch
        {
            return false;
        }
    }
}