using SmartWings.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWings.Infrastructure.Contracts
{
    public interface IFlightRepository
    {
        Task<AirCraft> AddAircraftAsync(AirCraft aircraft);
        Task<AirCraft> UpdateAircraftAsync(AirCraft aircraft);
        Task<bool> DeleteAircraftAsync(Guid aircraftId);
        Task<AirCraft> GetAircraftByIdAsync(Guid aircraftId);
        Task<IEnumerable<AirCraft>> GetAllAircraftsAsync();

        // Flight operations
        Task<Flight> ScheduleFlightAsync(Flight flight);
        Task<Flight> UpdateFlightAsync(Flight flight);
        Task<Flight> GetFlightByIdAsync(Guid flightId);
        Task<IEnumerable<Flight>> GetAllFlightsAsync(string? origin, string? destination, DateTime? departureDate);
        Task<IEnumerable<Flight>> GetAvailableFlightsAsync();

    }
}
