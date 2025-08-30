using System;
using System.Threading.Tasks;

namespace SmartWings.Application.Interfaces
{
    /// <summary>
    /// Payment service contract.
    /// </summary>
    public interface IPaymentService
    {
        /// <summary>
        /// Process a payment for a booking.
        /// </summary>
        Task<bool> ProcessPaymentAsync(
            Guid bookingId,
            Guid userId,
            string? upiId,
            string? cardNumber,
            string? cvv,
            string paymentMethod,
            decimal amount);
    }
}
