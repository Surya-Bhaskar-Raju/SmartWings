using System;
using System.Threading.Tasks;
using SmartWings.Domain;

namespace SmartWings.Infrastructure.Contracts
{
    /// <summary>
    /// Contract for booking-related data operations.
    /// </summary>
    public interface IBookingRepository
    {
        /// <summary>
        /// Creates a new booking record.
        /// </summary>
        /// <param name="booking">The booking entity to be created.</param>
        /// <returns>The created booking with generated reference ID.</returns>
        Task<Booking> CreateBookingAsync(Booking booking);

        /// <summary>
        /// Retrieves a booking by its reference ID.
        /// </summary>
        /// <param name="referenceId">The unique booking reference ID.</param>
        /// <returns>The booking if found; otherwise, null.</returns>
        Task<Booking> GetBookingByReferenceIdAsync(string referenceId);

        /// <summary>
        /// Cancels a booking using the reference ID.
        /// </summary>
        /// <param name="referenceId">The booking reference ID.</param>
        /// <returns>True if cancellation is successful; otherwise, false.</returns>
        Task<bool> CancelBookingAsync(string referenceId);

    


        /// <summary>
        /// Retrieves available seats for a given flight.
        /// </summary>
        /// <param name="flightId">The flight ID.</param>
        /// <param name="count">Number of seats needed.</param>
        /// <returns>List of available seats.</returns>
        Task<List<Seat>> GetAvailableSeatsAsync(Guid flightId, string travelClass, int count);

        /// <summary>
        /// Marks the specified seats as booked.
        /// </summary>
        /// <param name="seatIds">List of seat IDs to mark as booked.</param>
        /// <returns>Task representing the operation.</returns>
        Task MarkSeatsAsBookedAsync(List<Guid> seatIds);

        Task<Booking> GetBookingByIdAsync(Guid bookingId);


        /// <summary>
        /// Retrieves all bookings for a specific user.
        /// </summary>
        /// <param name="userId">The user's unique ID.</param>
        /// <returns>List of bookings made by the user.</returns>
        Task<List<Booking>> GetAllBookingsByUserIdAsync(Guid userId);

    }
}
