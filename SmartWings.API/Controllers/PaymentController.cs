using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmartWings.Application.Contracts;
using SmartWings.Application.DTOs;
using SmartWings.Application.Interfaces;

namespace SmartWings.API.Controllers
{
    /// <summary>
    /// Controller responsible for handling payment operations such as processing a payment,
    /// validating booking & user details, and sending confirmations.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly INotificationService _notificationService;
        private readonly IBookingService _bookingService;
        private readonly IUserService _userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentController"/> class.
        /// </summary>
        public PaymentController(
            IPaymentService paymentService,
            INotificationService notificationService,
            IBookingService bookingService,
            IUserService userService)
        {
            _paymentService = paymentService;
            _notificationService = notificationService;
            _bookingService = bookingService;
            _userService = userService;
        }

        /// <summary>
        /// Processes a payment for a booking and confirms the reservation if successful.
        /// </summary>
        /// <param name="dto">The payment request data transfer object.</param>
        /// <returns>
        /// Returns a success response with booking details if the payment is successful,
        /// otherwise returns an appropriate error response.
        /// </returns>
        [HttpPost("process")]
        public async Task<IActionResult> ProcessPayment([FromBody] PaymentRequestDto dto)
        {
            // Validate request model
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Attempt payment processing
            var isSuccess = await _paymentService.ProcessPaymentAsync(
                dto.BookingId,
                dto.UserId,
                dto.UpiId,
                dto.CardNumber,
                dto.CVV,
                dto.PaymentMethod,
                dto.Amount);

            if (!isSuccess)
                return BadRequest(new { message = $"{dto.PaymentMethod} Payment Failed. Invalid credentials." });

            // Fetch booking & user details to ensure valid references
            var booking = await _bookingService.GetBookingByIdAsync(dto.BookingId);
            var user = await _userService.GetUserByIdAsync(dto.UserId);

            if (booking == null || user == null)
                return NotFound("Booking or user details not found.");

            // Create in-app notification for user
            await _notificationService.CreateNotificationAsync(
                dto.UserId,
                dto.BookingId,
                $"Your booking for flight {booking.FlightNumber} from {booking.Origin} to {booking.Destination} has been successfully confirmed."
            );

            // Send confirmation email to user
            await _notificationService.SendEmailAsync(
                user.Email,
                "Booking Confirmed ✈️",
                $"Dear {user.UserName},\n\nYour booking for flight {booking.FlightNumber} from {booking.Origin} to {booking.Destination} has been successfully confirmed.\nYour booking reference number is {booking.BookingReference}.\n\nThank you for choosing SmartWings!"
            );

            // Return success response
            return Ok(new
            {
                message = "Payment completed and confirmation sent!",
                bookingId = booking.BookingId
            });
        }
    }
}
