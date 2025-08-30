using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWings.Application.DTO
{
    public class UpcomingFlightDto
    {
        public Guid FlightId { get; set; }
        public string FlightNumber { get; set; } = string.Empty;
        public string Origin { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;
        public DateTime DepartureTime { get; set; }
        public string AircraftModel { get; set; } = string.Empty;
        public int AvailableSeats { get; set; }
        public decimal BasePrice { get; set; }
    }

}
