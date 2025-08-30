using Microsoft.EntityFrameworkCore;
using SmartWings.Domain;
using SmartWings.Infrastructure.Contracts;
using SmartWings.Infrastructure.DataContext;
using System;
using System.Threading.Tasks;

namespace SmartWings.Infrastructure.Repositories
{
    /// <summary>
    /// Repository for managing booking-related data access operations.
    /// </summary>
    public class BookingRepository : IBookingRepository
    {
        private readonly FlightDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="BookingRepository"/> class.
        /// </summary>
        /// <param name="context">Database context for bookings.</param>
        public BookingRepository(FlightDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Creates a new booking record with generated reference ID and sets default values.
        /// </summary>
        /// <param name="booking">Booking entity to be created.</param>
        /// <returns>The created booking with reference ID.</returns>
        public async Task<Booking> CreateBookingAsync(Booking booking)
        {
            try
            {
                // Generate a unique booking reference ID
                booking.BookingReferenceId = GenerateReferenceId();

                // Set the booking date to current UTC time
                booking.BookingDate = DateTime.UtcNow;

                // Set default status as Confirmed
                booking.Status = "Confirmed";

                // Add booking to the database
                await _context.Bookings.AddAsync(booking);

                // Commit the transaction
                await _context.SaveChangesAsync();

                return booking;
            }
            catch (Exception ex)
            {
                // Rethrow with contextual message
                throw new Exception("An error occurred while creating the booking.", ex);
            }
        }

        /// <summary>
        /// Retrieves a booking by reference ID including related flight and user data.
        /// </summary>
        /// <param name="bookingreferenceId">Booking reference ID.</param>
        /// <returns>Booking object if found; otherwise, null.</returns>
        public async Task<Booking> GetBookingByReferenceIdAsync(string bookingreferenceId)
        {
            try
            {
                return await _context.Bookings
                    .Include(b => b.Flight) // Eager load Flight data
                    .Include(b => b.User)   // Eager load User data
                    .Include(b => b.Passengers) // ✅ Add this line
                    .FirstOrDefaultAsync(b => b.BookingReferenceId == bookingreferenceId);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the booking by reference ID.", ex);
            }
        }

        /// <summary>
        /// Cancels a booking by updating its status to "Cancelled".
        /// </summary>
        /// <param name="bookingreferenceId">Booking reference ID.</param>
        /// <returns>True if cancellation successful; otherwise, false.</returns>
        public async Task<bool> CancelBookingAsync(string bookingreferenceId)
        {
            try
            {
                // Fetch the booking
                var booking = await GetBookingByReferenceIdAsync(bookingreferenceId);

                // Return false if booking doesn't exist or already cancelled
                if (booking == null || booking.Status == "Cancelled")
                    return false;

                // Mark as cancelled
                booking.Status = "Cancelled";

                // Save the change
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while cancelling the booking.", ex);
            }
        }

      
        public async Task<List<Seat>> GetAvailableSeatsAsync(Guid flightId, string travelClass, int count)
        {
            try
            {
                return await _context.Seats
                    .Where(s => s.FlightId == flightId
                                && s.Class == travelClass
                                && !s.IsBooked)
                    .Take(count)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving available seats.", ex);
            }
        }



        public async Task MarkSeatsAsBookedAsync(List<Guid> seatIds)
        {
            try
            {
                var seats = await _context.Seats
                    .Where(s => seatIds.Contains(s.SeatId))
                    .ToListAsync();

                foreach (var seat in seats)
                {
                    seat.IsBooked = true;
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while marking seats as booked.", ex);
            }
        }

        public async Task<List<Booking>> GetBookingsByUserAsync(Guid userId)
        {
            try
            {
                return await _context.Bookings
                    .Include(b => b.Flight)
                    .Where(b => b.UserId == userId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving bookings for the user.", ex);
            }
        }

        public async Task<Booking> GetBookingByIdAsync(Guid bookingId)
        {
            try
            {
                return await _context.Bookings
                    .Include(b => b.Flight)
                    .Include(b => b.User)
                    .FirstOrDefaultAsync(b => b.BookingId == bookingId);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the booking by BookingId.", ex);
            }
        }

        public async Task<List<Booking>> GetAllBookingsByUserIdAsync(Guid userId)
        {
            try
            {
                return await _context.Bookings
                .Include(b => b.Flight)
                .Include(b => b.Passengers)
                .Where(b => b.UserId == userId)
                .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving bookings for the user.", ex);
            }
        }



        /// <summary>
        /// Generates a unique reference ID for a booking using the current UTC timestamp and random number.
        /// </summary>
        /// <returns>Formatted booking reference ID.</returns>
        private string GenerateReferenceId()
        {
            // Generate a random 3-digit suffix for uniqueness
            var suffix = new Random().Next(100, 999);

            // Format: BK-yyMMddHHmmss-suffix (e.g., BK-250727101522-723)
            return $"BK-{DateTime.UtcNow:yyMMddHHmmss}-{suffix}";
        }
    }
}
