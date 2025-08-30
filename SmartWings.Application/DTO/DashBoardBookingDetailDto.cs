namespace SmartWings.Application.DTO
{
    public class DashboardBookingDetailsDto
    {
        public Guid BookingId { get; set; }
         public string BookingReferenceId { get; set; }

        public Guid FlightId { get; set; }
        public string FlightNumber { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime BookingDate { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string AircraftModel { get; set; }
        public string Status { get; set; }
        public decimal Price { get; set; }
        public List<PassengerDashboardDto> Passengers { get; set; }
    }
}
