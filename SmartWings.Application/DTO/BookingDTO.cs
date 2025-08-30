using System;
using System.Collections.Generic;

namespace SmartWings.Application.DTO
{
    // DTO used when a user submits a booking request
    public class BookingRequestDto
    {
        public Guid UserId { get; set; }           // ID of the user making the booking
        public Guid FlightId { get; set; }         // ID of the selected flight
        public string Class { get; set; }          // Travel class: Economy, Business, etc.
        public List<PassengerRequestDto> Passengers { get; set; } // List of passengers to be booked
    }

    // DTO used to return clean booking data back to the client
    public class BookingDetailsDto
    {

        public Guid Userid { get; set; }
        public Guid BookingId { get; set; }              // Unique identifier for the booking
        public string BookingReference { get; set; }     // Booking reference ID shown to the user
        public string FlightNumber { get; set; }         // Flight number (e.g., AI203)
        public string Origin { get; set; }               // Flight departure location
        public string Destination { get; set; }          // Flight arrival location

        public DateTime DepartureTime { get; set; }   // Scheduled departure time
        public DateTime ArrivalTime { get; set; }     // Scheduled arrival time
        public string Class { get; set; }                // Travel class of the booking
        public decimal TotalAmount { get; set; }         // Total fare charged
        public string Status { get; set; }               // Current booking status
        public DateTime BookingDate { get; set; }        // Date and time when booking was made
        public int PassengerCount { get; set; }          // Total number of passengers
        public List<PassengerDetailsDto> Passengers { get; set; } // List of passengers
    }
}
