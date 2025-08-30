using System;

namespace SmartWings.Application.DTO
{
    public class PassengerRequestDto
    {
        // Passenger full name
        public string FullName { get; set; }

        // Passenger age
        public int Age { get; set; }

        // Passenger gender
        public string Gender { get; set; }

        // Passport number
        public string PassportNumber { get; set; }
    }

    public class PassengerDetailsDto
    {
        // Passenger full name
        public string FullName { get; set; }

        // Passenger age
        public int Age { get; set; }

        // Passenger gender
        public string Gender { get; set; }

        // Passport number
        public string PassportNumber { get; set; }

        // Assigned seat number
        public string SeatNumber { get; set; }
    }
}
