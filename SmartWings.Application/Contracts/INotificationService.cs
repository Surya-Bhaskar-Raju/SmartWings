using SmartWings.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartWings.Application.Contracts
{
    /// <summary>
    /// Notification service contract.
    /// </summary>
    public interface INotificationService
    {
        /// <summary>
        /// Create a new notification.
        /// </summary>
        Task CreateNotificationAsync(Guid userId, Guid bookingId, string message);

        /// <summary>
        /// Get all notifications for a user.
        /// </summary>
        Task<IEnumerable<Notification>> GetNotificationsByUserIdAsync(Guid userId);

        /// <summary>
        /// Send an email notification.
        /// </summary>
        Task SendEmailAsync(string to, string subject, string body);
    }
}
