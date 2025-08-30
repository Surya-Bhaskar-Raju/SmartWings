using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartWings.Domain
{
    public class Booking
    {
        [Key]
        public Guid BookingId { get; set; } // Primary key for the booking

        [ForeignKey("User")]
        public Guid UserId { get; set; } // Foreign key to the User entity

        [ForeignKey("Flight")]
        public Guid FlightId { get; set; } // Foreign key to the Flight entity

        [Required, StringLength(20)]
        public string Class { get; set; } // Travel class (e.g., Economy, Business)

        public DateTime BookingDate { get; set; } // Timestamp when booking was created

        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalAmount { get; set; } // Total fare of the booking

        [Required, StringLength(20)]
        public string Status { get; set; } // Booking status (Confirmed, Cancelled, etc.)

        [Required]
        [Range(1, 10, ErrorMessage = "Passenger count must be at least 1.")]
        public int PassengerCount { get; set; } // Total number of passengers in the booking


        [Required, StringLength(25)]
        public string BookingReferenceId { get; set; } // Unique reference ID for tracking

        public User User { get; set; } // Navigation property to User

        public Flight Flight { get; set; } // Navigation property to Flight

        public Payment Payment { get; set; } // Navigation property to Payment (one-to-one)

        public ICollection<Notification> Notifications { get; set; } = new List<Notification>(); // Related notifications; initialized to avoid null reference

        public ICollection<Passenger> Passengers { get; set; } = new List<Passenger>();

    }
}
