using Microsoft.EntityFrameworkCore;
using SmartWings.Domain;
using SmartWings.Infrastructure.Contracts;
using SmartWings.Infrastructure.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace SmartWings.Infrastructure.Repositories
{
    public class FlightRepository : IFlightRepository
    {
        private readonly FlightDbContext _context;
        public FlightRepository(FlightDbContext context)
        {
            _context = context;
        }
        public async Task<AirCraft> AddAircraftAsync(AirCraft aircraft)
        {
            _context.AirCrafts.Add(aircraft);
            await _context.SaveChangesAsync();
            return aircraft;
        }

        public async Task<AirCraft> UpdateAircraftAsync(AirCraft aircraft)
        {
            var exists = await _context.AirCrafts.AnyAsync(a => a.AirCraftId == aircraft.AirCraftId);
            if (!exists) return null;

            _context.AirCrafts.Update(aircraft);
            await _context.SaveChangesAsync();
            return aircraft;
        }

        public async Task<bool> DeleteAircraftAsync(Guid aircraftId)
        {
            var entity = await _context.AirCrafts.FindAsync(aircraftId);
            if (entity == null) return false;

            _context.AirCrafts.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<AirCraft> GetAircraftByIdAsync(Guid aircraftId)
        {
            return await _context.AirCrafts
                                 .Include(a => a.Flights)
                                 .FirstOrDefaultAsync(a => a.AirCraftId == aircraftId);
        }

        public async Task<IEnumerable<AirCraft>> GetAllAircraftsAsync()
        {
            return await _context.AirCrafts
                                 .Include(a => a.Flights)
                                 .ToListAsync();
        }

        // -------- Flight Methods --------

        public async Task<Flight> ScheduleFlightAsync(Flight flight)
        {
            _context.Flights.Add(flight);
            await _context.SaveChangesAsync();

            var aircraft = await _context.AirCrafts.FindAsync(flight.AircraftId);
            if (aircraft != null)
            {
                await InitializeSeatsForFlightAsync(flight.FlightId);
            }

            return flight;
        }



        public async Task InitializeSeatsForFlightAsync(Guid flightId)
        {
            var flight = await _context.Flights
                                       .Include(f => f.AirCraft)
                                       .FirstOrDefaultAsync(f => f.FlightId == flightId);

            if (flight == null || flight.AirCraft == null)
                throw new InvalidOperationException("Flight or associated aircraft not found.");

            var seats = new List<Seat>();

            // Business Class Seats
            for (int i = 1; i <= flight.AirCraft.BusinessSeats; i++)
            {
                seats.Add(new Seat
                {
                    FlightId = flight.FlightId,
                    SeatNumber = $"B{i:D2}",      // B01, B02...
                    Class = "Business",
                    IsBooked = false
                });
            }

            // Economy Class Seats
            for (int i = 1; i <= flight.AirCraft.EconomySeats; i++)
            {
                seats.Add(new Seat
                {
                    FlightId = flight.FlightId,
                    SeatNumber = $"E{i:D2}",      // E01, E02...
                    Class = "Economy",
                    IsBooked = false
                });
            }

            await _context.Seats.AddRangeAsync(seats);
            await _context.SaveChangesAsync();
        }


        public async Task<Flight> UpdateFlightAsync(Flight flight)
        {
            var exists = await _context.Flights.AnyAsync(f => f.FlightId == flight.FlightId);
            if (!exists) return null;

            _context.Flights.Update(flight);
            await _context.SaveChangesAsync();
            return flight;
        }

        public async Task<Flight> GetFlightByIdAsync(Guid flightId)
        {
            return await _context.Flights
                                 .Include(f => f.AirCraft)
                                 .Include(f => f.Seats)
                                 .Include(f => f.Bookings)
                                 .FirstOrDefaultAsync(f => f.FlightId == flightId);
        }

        public async Task<IEnumerable<Flight>> GetAllFlightsAsync(string? origin, string? destination, DateTime? departureDate)
        {
            var flights = _context.Flights.AsQueryable();

            if (!string.IsNullOrEmpty(origin))
            {
                flights = flights.Where(f => f.Origin == origin);
            }

            if (!string.IsNullOrEmpty(destination))
            {
                flights = flights.Where(f => f.Destination == destination);
            }

            if (departureDate.HasValue)
                flights = flights.Where(f => f.DepartureTime.Date == departureDate.Value.Date);


            return await flights.Include(f => f.AirCraft)
                                 .Include(f => f.Seats)
                                 .Include(f => f.Bookings)
                                 .ToListAsync();

        }

        public async Task<IEnumerable<Flight>> GetAvailableFlightsAsync()
        {
            return await _context.Flights
                                 .Where(f => f.Status == "Available")
                                 .Include(f => f.AirCraft)
                                 .ToListAsync();
        }


    }
}
