using System;

namespace SmartWings.Application.DTOs
{
    public class NotificationDto
    {
        // Related booking ID
        public Guid BookingId { get; set; }

        // User ID who receives the notification
        public Guid UserId { get; set; }

        // Notification message
        public string Message { get; set; }

        // Unique notification ID
        public Guid NotificationId { get; set; }

        // Creation date and time
        public DateTime CreatedAt { get; set; }

        // Read/unread status
        public bool IsRead { get; set; }
    }
}
