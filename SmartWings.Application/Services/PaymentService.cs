using System;
using System.Threading.Tasks;
using SmartWings.Application.Interfaces;
using SmartWings.Domain;
using SmartWings.Domain.Interfaces;

namespace SmartWings.Application.Services
{
    /// <summary>
    /// Handles payment processing logic for bookings.
    /// </summary>
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;

        /// <summary>
        /// Initializes a new instance of <see cref="PaymentService"/>.
        /// </summary>
        /// <param name="paymentRepository">Repository to manage payment data.</param>
        public PaymentService(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        /// <summary>
        /// Processes a payment for a booking using either UPI or Card.
        /// </summary>
        /// <param name="bookingId">The ID of the booking.</param>
        /// <param name="userId">The ID of the user making the payment.</param>
        /// <param name="upiId">The UPI ID if using UPI payment.</param>
        /// <param name="cardNumber">The card number if using card payment.</param>
        /// <param name="cvv">The CVV if using card payment.</param>
        /// <param name="paymentMethod">The payment method ("UPI" or "Card").</param>
        /// <param name="amount">The payment amount.</param>
        /// <returns>True if payment is valid and processed, otherwise false.</returns>
        public async Task<bool> ProcessPaymentAsync(Guid bookingId, Guid userId, string? upiId, string? cardNumber, string? cvv, string paymentMethod, decimal amount)
        {
            bool isValid = false;
            var method = paymentMethod.ToLower();

            // ✅ Validate based on payment method
            switch (method)
            {
                case "upi":
                    // UPI should not be empty and must contain '@'
                    isValid = !string.IsNullOrWhiteSpace(upiId) && upiId.Contains("@");
                    break;

                case "card":
                    // Card should be 16 digits and CVV should be 3 digits
                    isValid =
                        !string.IsNullOrWhiteSpace(cardNumber) &&
                        cardNumber.Length == 16 &&
                        !string.IsNullOrWhiteSpace(cvv) &&
                        cvv.Length == 3;
                    break;

                default:
                    return false; // ❌ Unsupported payment method
            }

            if (!isValid) return false; // ❌ Stop if invalid

            // ✅ Create a new payment record
            var payment = new Payment
            {
                BookingId = bookingId,
                UserId = userId,
                PaymentMethod = method,
                PaymentDate = DateTime.UtcNow,
                Status = "Completed",
                Amount = amount
            };

            await _paymentRepository.AddAsync(payment); // Save payment to DB
            return true;
        }
    }
}
