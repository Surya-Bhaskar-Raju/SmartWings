using SmartWings.Application.Contracts;
using SmartWings.Application.DTO;
using SmartWings.Domain;
using SmartWings.Infrastructure.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartWings.Application.Services
{
    /// <summary>
    /// Service for handling booking-related operations such as creation, retrieval, and cancellation.
    /// </summary>
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepo; // Repository for bookings
        private readonly IFlightRepository _flightRepo;   // Repository for flights

        private const int MaxPassengersPerBooking = 6; // Limit for one booking

        /// <summary>
        /// Constructor to inject required repositories.
        /// </summary>
        public BookingService(IBookingRepository bookingRepo, IFlightRepository flightRepo)
        {
            _bookingRepo = bookingRepo;
            _flightRepo = flightRepo;
        }

        /// <summary>
        /// Creates a new booking with passenger details, seat assignment, and fare calculation.
        /// </summary>
        public async Task<BookingDetailsDto> CreateBookingAsync(BookingRequestDto dto)
        {
            try
            {
                var flight = await _flightRepo.GetFlightByIdAsync(dto.FlightId);
                if (flight == null) return null; // Flight not found

                // Validate passenger count
                if (dto.Passengers == null || dto.Passengers.Count is < 1 or > MaxPassengersPerBooking)
                    throw new ArgumentOutOfRangeException(nameof(dto.Passengers),
                        $"You can book between 1 and {MaxPassengersPerBooking} passengers.");

                // Check seat availability
                var availableSeats = await _bookingRepo.GetAvailableSeatsAsync(dto.FlightId, dto.Class, dto.Passengers.Count);
                if (availableSeats.Count < dto.Passengers.Count)
                    throw new InvalidOperationException(
                        $"Only {availableSeats.Count} seats available in '{dto.Class}'.");

                // Assign seats to passengers
                var assignedSeats = availableSeats.Take(dto.Passengers.Count).ToList();
                var passengers = dto.Passengers.Select((p, index) => new Passenger
                {
                    FullName = p.FullName,
                    Age = p.Age,
                    Gender = p.Gender,
                    PassportNumber = p.PassportNumber,
                    SeatId = assignedSeats[index].SeatId,     // Link to seat entity
                    SeatNumber = assignedSeats[index].SeatNumber // Display to user
                }).ToList();

                // Calculate total fare
                var farePerPassenger = CalculateFare(dto.Class, flight);
                var totalFare = farePerPassenger * passengers.Count;

                // Build booking entity
                var booking = new Booking
                {
                    BookingId = Guid.NewGuid(),
                    BookingReferenceId = GenerateReferenceId(),
                    UserId = dto.UserId,
                    FlightId = dto.FlightId,
                    Class = dto.Class,
                    TotalAmount = totalFare,
                    Status = "Confirmed",
                    BookingDate = DateTime.UtcNow,
                    PassengerCount = passengers.Count,
                    Passengers = passengers
                };

                // Save booking & update seat status
                var savedBooking = await _bookingRepo.CreateBookingAsync(booking);
                await _bookingRepo.MarkSeatsAsBookedAsync(assignedSeats.Select(s => s.SeatId).ToList());

                return MapToDto(savedBooking, flight);
            }
            catch (Exception)
            {
                throw; // Ideally log and rethrow
            }
        }

        /// <summary>
        /// Retrieves booking details by booking reference ID.
        /// </summary>
        public async Task<BookingDetailsDto> GetBookingDetailsAsync(string referenceId)
        {
            var booking = await _bookingRepo.GetBookingByReferenceIdAsync(referenceId);
            if (booking == null) return null;

            var flight = await _flightRepo.GetFlightByIdAsync(booking.FlightId);
            return MapToDto(booking, flight);
        }

        /// <summary>
        /// Cancels a booking by reference ID.
        /// </summary>
        public async Task<bool> CancelBookingAsync(string referenceId)
        {
            return await _bookingRepo.CancelBookingAsync(referenceId);
        }

        /// <summary>
        /// Retrieves booking details by unique booking ID.
        /// </summary>
        public async Task<BookingDetailsDto> GetBookingByIdAsync(Guid bookingId)
        {
            var booking = await _bookingRepo.GetBookingByIdAsync(bookingId);
            if (booking == null) return null;

            // Map booking to DTO
            return new BookingDetailsDto
            {
                BookingId = booking.BookingId,
                BookingReference = booking.BookingReferenceId,
                FlightNumber = booking.Flight.FlightNumber,
                Origin = booking.Flight.Origin,
                Destination = booking.Flight.Destination,
                Class = booking.Class,
                TotalAmount = booking.TotalAmount,
                Status = booking.Status,
                BookingDate = booking.BookingDate,
                PassengerCount = booking.Passengers?.Count ?? 0,
                Passengers = booking.Passengers?.Select(p => new PassengerDetailsDto
                {
                    FullName = p.FullName,
                    Age = p.Age,
                    Gender = p.Gender,
                    PassportNumber = p.PassportNumber,
                    SeatNumber = p.SeatNumber
                }).ToList()
            };
        }

        /// <summary>
        /// Retrieves all bookings for a specific user.
        /// </summary>
        public async Task<List<BookingDetailsDto>> GetAllBookingsByUserIdAsync(Guid userId)
        {
            var bookings = await _bookingRepo.GetAllBookingsByUserIdAsync(userId);
            var bookingDtos = new List<BookingDetailsDto>();

            foreach (var booking in bookings)
            {
                var flight = await _flightRepo.GetFlightByIdAsync(booking.FlightId);
                if (flight == null) continue; // Skip if flight not found

                // Convert booking into DTO
                bookingDtos.Add(new BookingDetailsDto
                {
                    BookingId = booking.BookingId,
                    BookingReference = booking.BookingReferenceId,
                    Userid = booking.UserId,
                    FlightNumber = flight.FlightNumber,
                    Origin = flight.Origin,
                    Destination = flight.Destination,
                    Class = booking.Class,
                    TotalAmount = booking.TotalAmount,
                    Status = booking.Status,
                    BookingDate = booking.BookingDate,
                    DepartureTime = flight.DepartureTime,
                    ArrivalTime = flight.ArrivalTime,
                    PassengerCount = booking.Passengers?.Count ?? 0,
                    Passengers = booking.Passengers?.Select(p => new PassengerDetailsDto
                    {
                        FullName = p.FullName,
                        Age = p.Age,
                        Gender = p.Gender,
                        PassportNumber = p.PassportNumber,
                        SeatNumber = p.SeatNumber
                    }).ToList()
                });
            }

            return bookingDtos;
        }

        // ------------------------ Helpers ------------------------

        /// <summary>
        /// Generates a short booking reference ID.
        /// </summary>
        private string GenerateReferenceId() =>
            Guid.NewGuid().ToString().Substring(0, 8).ToUpper();

        /// <summary>
        /// Calculates fare based on travel class and flight.
        /// </summary>
        private decimal CalculateFare(string travelClass, Flight flight) =>
            travelClass switch
            {
                "Business" => flight.PriceBusiness, // Higher price for business class
                _ => flight.PriceEconomy
            };

        /// <summary>
        /// Maps a Booking entity and Flight to BookingDetailsDto.
        /// </summary>
        private BookingDetailsDto MapToDto(Booking b, Flight flight) => new BookingDetailsDto
        {
            BookingId = b.BookingId,
            BookingReference = b.BookingReferenceId,
            Userid = b.UserId,
            FlightNumber = flight.FlightNumber,
            Origin = flight.Origin,
            Destination = flight.Destination,
            Class = b.Class,
            TotalAmount = b.TotalAmount,
            Status = b.Status,
            BookingDate = b.BookingDate,
            DepartureTime = flight.DepartureTime,
            ArrivalTime = flight.ArrivalTime,
            PassengerCount = b.PassengerCount,
            Passengers = b.Passengers.Select(p => new PassengerDetailsDto
            {
                FullName = p.FullName,
                Age = p.Age,
                Gender = p.Gender,
                PassportNumber = p.PassportNumber,
                SeatNumber = p.SeatNumber
            }).ToList()
        };
    }
}
