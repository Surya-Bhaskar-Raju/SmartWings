// Import the domain models used in the contract
using SmartWings.Domain;

namespace SmartWings.Infrastructure.Contracts
{
    /// <summary>
    /// Contract interface for admin dashboard-related operations.
    /// Provides methods for retrieving bookings, revenue, and grouped data for reporting.
    /// </summary>
    public interface IAdminDashboardContract
    {
        decimal GetTotalRevenueByAircraftModel(string aircraftName);
        Task<IEnumerable<Booking>> GetBookingsByFlightNumberAndDepartureDateAsync(
            string flightNumber,
            DateTime? departureDate,string aircraftModel
        );
        Task<IEnumerable<Flight>> GetUpcomingFlightsAsync(DateTime fromDate);
        Task<List<Flight>> GetAllflightWithAircraftModel(string aircraftmodel);


    }
}
