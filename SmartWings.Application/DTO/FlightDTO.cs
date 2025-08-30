using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWings.Application.DTO
{
    public class FlightCreateDto
    {
        public string FlightNumber { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public Guid AircraftId { get; set; }
        public string Status { get; set; }
        public decimal PriceEconomy { get; set; }
        public decimal PriceBusiness { get; set; }
    }

    public class FlightReadDto
    {
        public Guid FlightId { get; set; }
        public string FlightNumber { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }

        public Guid AircraftId {get;set;}
        public string Status { get; set; }
        public decimal PriceEconomy { get; set; }
        public decimal PriceBusiness { get; set; }
    }

    public class FlightUpdateDto
    {
        public Guid FlightId { get; set; }
        public string FlightNumber { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public Guid AircraftId { get; set; }
        public string Status { get; set; }
        public decimal PriceEconomy { get; set; }
        public decimal PriceBusiness { get; set; }
    }


}
