using SmartWings.Application.Contracts;
using SmartWings.Domain;
using SmartWings.Infrastructure.Contracts;
using SmartWings.Infrastructure.DataContext;
using System;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SmartWings.Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly FlightDbContext _context;

        public NotificationService(INotificationRepository notificationRepository, FlightDbContext context)
        {
            _notificationRepository = notificationRepository;
            _context = context;
        }

        /// <summary>
        /// Creates a notification for a user and sends an email.
        /// </summary>
        /// <param name="userId">User ID who will receive the notification</param>
        /// <param name="bookingId">Booking ID related to the notification</param>
        /// <param name="message">Notification message</param>
        public async Task CreateNotificationAsync(Guid userId, Guid bookingId, string message)
        {
            // Get the user details from DB
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);

            // If user not found or email is missing → skip email sending
            if (user == null || string.IsNullOrWhiteSpace(user.Email))
            {
                Console.WriteLine("Email sending skipped: User email not found.");
                return;
            }

            // Create a new notification object
            var notification = new Notification
            {
                UserId = userId,
                BookingId = bookingId,
                Message = message,
                CreatedAt = DateTime.UtcNow, // store in UTC
                IsRead = false
            };

            // Save notification to database
            await _notificationRepository.AddNotificationAsync(notification);

            // Send email notification
            await SendEmailAsync(user.Email, "SmartWings - Booking Notification", message);
        }

        /// <summary>
        /// Sends an email using SMTP.
        /// </summary>
        /// <param name="to">Receiver email address</param>
        /// <param name="subject">Subject of email</param>
        /// <param name="body">Email body content</param>
        public async Task SendEmailAsync(string to, string subject, string body)
        {
            try
            {
                var fromEmail = "pardhipragya135@gmail.com"; // sender email
                var fromPassword = "pmmz nyax flcn wttp";   // app password (not plain password)

                // Create the mail message
                var mail = new MailMessage(fromEmail, to, subject, body)
                {
                    IsBodyHtml = true // allows HTML content in mail
                };

                // Configure SMTP client for Gmail
                var smtpClient = new SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new NetworkCredential(fromEmail, fromPassword),
                    EnableSsl = true // use SSL
                };

                // Send email asynchronously
                await smtpClient.SendMailAsync(mail);
            }
            catch (Exception ex)
            {
                // Log failure if email not sent
                Console.WriteLine($"Email failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets all notifications for a specific user.
        /// </summary>
        /// <param name="userId">User ID to fetch notifications</param>
        /// <returns>List of notifications</returns>
        public async Task<IEnumerable<Notification>> GetNotificationsByUserIdAsync(Guid userId)
        {
            // Fetch notifications from repository
            return await _notificationRepository.GetNotificationsByUserIdAsync(userId);
        }
    }
}
