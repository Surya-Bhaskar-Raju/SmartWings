using SmartWings.Domain;
using SmartWings.Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;
using SmartWings.Infrastructure.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartWings.Infrastructure.Repositories
{
    // Repository implementation for admin dashboard-related operations
    public class AdminDashboardRepository : IAdminDashboardContract
    {
        private readonly FlightDbContext _context;

        // Injecting the database context through constructor
        public AdminDashboardRepository(FlightDbContext context)
        {
            _context = context;
        }

        public decimal GetTotalRevenueByAircraftModel(string aircraftModel)
        {
            return _context.Bookings
                .Where(b => b.Flight.AirCraft.Model.ToLower() == aircraftModel.ToLower() && (b.Status == "Confirmed" || b.Status == "Visited"))
                .Sum(b => b.TotalAmount);
        }

        public async Task<IEnumerable<Booking>> GetBookingsByFlightNumberAndDepartureDateAsync(
            string flightNumber, DateTime? departureDate, string aircraftModel)
        {
            if (string.IsNullOrWhiteSpace(flightNumber))
                throw new ArgumentException("Flight number cannot be empty.", nameof(flightNumber));

            flightNumber = flightNumber.ToLower();

            var query = _context.Bookings
                .Include(b => b.User) // Include booking owner
                .Include(b => b.Flight)
                .ThenInclude(f => f.AirCraft) // Include flight details
                .Include(b => b.Passengers) // Include passengers
                    .ThenInclude(p => p.Seat) // Include seat for each passenger
                .Where(b => b.Flight.FlightNumber.ToLower() == flightNumber && b.Flight.AirCraft.Model==aircraftModel && (b.Status=="Confirmed" || b.Status=="Visited"));

            if (departureDate.HasValue)
            {
                // Keep date-only match for filtering
                query = query.Where(b => b.Flight.DepartureTime.Date == departureDate.Value.Date);
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Flight>> GetUpcomingFlightsAsync(DateTime fromDate)
        {
            return await _context.Flights
                .Include(f => f.AirCraft)
                .Where(f => f.DepartureTime >= fromDate)
                .OrderBy(f => f.DepartureTime)
                .ToListAsync();
        }
        public async Task <List<Flight>> GetAllflightWithAircraftModel(string aircraftmodel)
        {
            return await _context.Flights.Include(f => f.AirCraft).Where(f=>f.AirCraft.Model == aircraftmodel).ToListAsync();
            
        }

    }
}
