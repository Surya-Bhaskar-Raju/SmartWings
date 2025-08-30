using SmartWings.Application.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartWings.Application.Contracts
{
    /// <summary>
    /// Booking service contract.
    /// </summary>
    public interface IBookingService
    {
        /// <summary>
        /// Create a booking.
        /// </summary>
        Task<BookingDetailsDto> CreateBookingAsync(BookingRequestDto dto);

        /// <summary>
        /// Get booking by reference ID.
        /// </summary>
        Task<BookingDetailsDto> GetBookingDetailsAsync(string bookingReferenceId);

        /// <summary>
        /// Cancel a booking by reference ID.
        /// </summary>
        Task<bool> CancelBookingAsync(string bookingReferenceId);

        /// <summary>
        /// Get booking by booking ID (GUID).
        /// </summary>
        Task<BookingDetailsDto> GetBookingByIdAsync(Guid bookingId);

        /// <summary>
        /// Get all bookings for a user.
        /// </summary>
        Task<List<BookingDetailsDto>> GetAllBookingsByUserIdAsync(Guid userId);
    }
}
