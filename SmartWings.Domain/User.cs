using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartWings.Domain
{
    public class User
    {
        [Key]
        public Guid UserId { get; set; }

        [Required]
        [StringLength(100)]
        public string UserName { get; set; }

        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(100)]
        public string PasswordHash { get; set; }

        [Required]
        [StringLength(50)]
        public string Role { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();

        public ICollection<Payment> Payments { get; set; } = new List<Payment>();

        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }
}
