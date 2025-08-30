using System;

namespace SmartWings.Application.DTOs
{
    public class PaymentRequestDto
    {
        // Related booking ID
        public Guid BookingId { get; set; }

        // User making the payment
        public Guid UserId { get; set; }

        // UPI ID (optional)
        public string? UpiId { get; set; }

        // Card number (optional)
        public string? CardNumber { get; set; }

        // CVV for card payment (optional)
        public string? CVV { get; set; }

        // Payment method (default: UPI)
        public string PaymentMethod { get; set; } = "UPI";

        // Payment amount
        public decimal Amount { get; set; }
    }

    public class PaymentResponseDto
    {
        // Unique payment ID
        public Guid PaymentId { get; set; }

        // Related booking ID
        public Guid BookingId { get; set; }

        // UPI ID (if used)
        public string? UpiId { get; set; }

        // Paid amount
        public decimal Amount { get; set; }

        // Payment status (default: Pending)
        public string Status { get; set; } = "Pending";

        // Payment date & time
        public DateTime PaidAt { get; set; }
    }
}
