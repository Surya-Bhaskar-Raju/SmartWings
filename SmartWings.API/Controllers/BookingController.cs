using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartWings.Application.Contracts;
using SmartWings.Application.DTO;
using System;
using System.Threading.Tasks;

namespace SmartWings.API.Controllers
{
    /// <summary>
    /// Controller responsible for handling booking-related operations such as 
    /// creating, tracking, cancelling, and retrieving bookings.
    /// </summary>
    [Authorize(Roles = "User")] // Only users with the 'User' role are authorized
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly INotificationService _notificationService;
        private readonly IUserService _userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="BookingController"/> class.
        /// </summary>
        public BookingController(
            IBookingService bookingService,
            INotificationService notificationService,
            IUserService userService)
        {
            _bookingService = bookingService;
            _notificationService = notificationService;
            _userService = userService;
        }

        /// <summary>
        /// Books a new flight for a user.
        /// </summary>
        /// <param name="dto">The booking request data transfer object.</param>
        /// <returns>Booking details if successful, otherwise an error response.</returns>
        [HttpPost("book")]
        public async Task<IActionResult> BookFlight([FromBody] BookingRequestDto dto)
        {
            // Attempt to create a booking
            var result = await _bookingService.CreateBookingAsync(dto);

            if (result == null)
                return BadRequest("Invalid flight selection or booking failed.");

            // Fetch user details for potential notifications (future use)
            var userdetails = await _userService.GetUserByIdAsync(result.Userid);

            return Ok(result);
        }

        /// <summary>
        /// Retrieves booking details by reference ID.
        /// </summary>
        /// <param name="referenceId">The booking reference identifier.</param>
        /// <returns>Booking details if found, otherwise NotFound.</returns>
        [HttpGet("track/{referenceId}")]
        public async Task<IActionResult> TrackBooking(string referenceId)
        {
            // Look up the booking by reference
            var booking = await _bookingService.GetBookingDetailsAsync(referenceId);

            if (booking == null)
                return NotFound($"No booking found for reference: {referenceId}");

            return Ok(booking);
        }

        /// <summary>
        /// Cancels an existing booking.
        /// </summary>
        /// <param name="referenceId">The booking reference identifier.</param>
        /// <returns>Success or failure response.</returns>
        [HttpPut("cancel/{referenceId}")]
        public async Task<IActionResult> CancelBooking(string referenceId)
        {
            // Retrieve booking details
            var booking = await _bookingService.GetBookingDetailsAsync(referenceId);

            if (booking == null)
                return NotFound("Cannot cancel this booking.");

            // Retrieve user details for notification purposes
            var userdetails = await _userService.GetUserByIdAsync(booking.Userid);

            // Cancel the booking
            var success = await _bookingService.CancelBookingAsync(referenceId);

            if (success)
            {
                // Create an in-app notification
                await _notificationService.CreateNotificationAsync(
                    booking.Userid,
                    booking.BookingId,
                    $"Your booking (Ref: {booking.BookingReference}) for flight {booking.FlightNumber} from {booking.Origin} to {booking.Destination} has been cancelled."
                );

                // Send email notification
                await _notificationService.SendEmailAsync(
                    userdetails.Email,
                    "Booking Cancellation request accepted..",
                    $"Your booking (Ref: {booking.BookingReference}) for flight {booking.FlightNumber} from {booking.Origin} to {booking.Destination} has been cancelled."
                );
            }

            return success ? Ok("Booking cancelled successfully.")
                           : NotFound("Failed to cancel booking.");
        }

        /// <summary>
        /// Gets all bookings for a specific user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>List of bookings made by the user.</returns>
        [HttpGet("user/{userId}/bookings")]
        public async Task<IActionResult> GetBookingsByUserId(Guid userId)
        {
            var bookings = await _bookingService.GetAllBookingsByUserIdAsync(userId);
            return Ok(bookings);
        }
    }
}
