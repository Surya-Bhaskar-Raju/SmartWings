using System;

namespace SmartWings.Domain
{
    public class Payment
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid BookingId { get; set; }
        public Guid UserId { get; set; }
        public string Status { get; set; } = "Completed";
        public string PaymentMethod { get; set; } = "UPI";
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
        public decimal Amount { get; set; }
        public Booking? Booking { get; set; }
        public User? User { get; set; }
    }
}