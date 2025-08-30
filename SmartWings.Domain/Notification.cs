using System;
using System.ComponentModel.DataAnnotations;

namespace SmartWings.Domain
{
    public class Notification
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid UserId { get; set; }

        public Guid BookingId { get; set; }

        [Required, MaxLength(100)]
        public string Message { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsRead { get; set; } = false;

        public User? User { get; set; }
        public Booking? Booking { get; set; }
    }
}