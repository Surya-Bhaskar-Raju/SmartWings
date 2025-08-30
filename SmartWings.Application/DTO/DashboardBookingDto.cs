namespace SmartWings.Application.DTO
{
    public class DashboardBookingDto
    {
        public Guid BookingId { get; set; }
        public string UserFullName { get; set; }
        public string FlightNumber { get; set; }
        public string SeatNumbers { get; set; }
        public string Class { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public string PassengerDetails { get; set; }
    }
}
