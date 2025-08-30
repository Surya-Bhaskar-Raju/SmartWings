using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartWings.Domain
{
    public class Flight
    {
        // Primary key for the Flight table
        [Key]
        public Guid FlightId { get; set; }

        // Unique flight number (e.g., AI101), max 50 characters, required
        [Required]
        [StringLength(50)]
        public string FlightNumber { get; set; }

        // City or airport from where the flight departs, max 100 characters, required
        [Required, StringLength(100)]
        public string Origin { get; set; }

        // City or airport where the flight arrives, max 100 characters, required
        [Required, StringLength(100)]
        public string Destination { get; set; }

        // Scheduled departure date and time
        public DateTime DepartureTime { get; set; }

        // Scheduled arrival date and time
        public DateTime ArrivalTime { get; set; }

        // Foreign key pointing to the related Aircraft
        [ForeignKey("AirCraft")]
        public Guid AircraftId { get; set; }

        // Status of the flight (e.g., Scheduled, Cancelled, Delayed), max 20 characters, required
        [Required, StringLength(20)]
        public string Status { get; set; }

        // Ticket price for economy class, stored as decimal with precision (10,2)
        [Column(TypeName = "decimal(10,2)")]
        public decimal PriceEconomy { get; set; }

        // Ticket price for business class, stored as decimal with precision (10,2)
        [Column(TypeName = "decimal(10,2)")]
        public decimal PriceBusiness { get; set; }

        // Navigation property for related AirCraft entity
        public AirCraft AirCraft { get; set; }

        // Navigation property for related Seats (one-to-many relationship)
        public ICollection<Seat> Seats { get; set; }

        // Navigation property for related Bookings (one-to-many relationship)
        public ICollection<Booking> Bookings { get; set; }
    }
}
