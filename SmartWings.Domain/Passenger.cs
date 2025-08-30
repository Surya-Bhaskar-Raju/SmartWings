using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartWings.Domain
{
    public class Passenger
    {
        [Key]
        public Guid PassengerId { get; set; } // Primary key

        [Required, StringLength(100)]
        public string FullName { get; set; } // Passenger's full name

        [Range(0, 120)]
        public int Age { get; set; } // Age range validation

        [Required, StringLength(10)]
        public string Gender { get; set; } // Gender (Male, Female, Other)

        [Required, StringLength(20)]
        public string PassportNumber { get; set; } // Unique passport number

        [Required, MaxLength(5)]
        public string SeatNumber { get; set; }


        [ForeignKey("Booking")]
        public Guid BookingId { get; set; } // Foreign key to Booking

        public Booking Booking { get; set; } // Navigation property

        [ForeignKey("Seat")]
        public Guid SeatId { get; set; } // Assigned seat

        public Seat Seat { get; set; }   // Navigation property

    }
}
