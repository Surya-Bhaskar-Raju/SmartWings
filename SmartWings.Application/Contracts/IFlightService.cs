using SmartWings.Application.DTO;
using SmartWings.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWings.Application.Contracts
{
    public interface IFlightService
    {
        Task<AirCraftReadDto> AddAircraftAsync(AirCraftCreateDto dto);
        Task<AirCraftReadDto?> UpdateAircraftAsync(AirCraftUpdateDto dto);
        Task<bool> DeleteAircraftAsync(Guid aircraftId);
        Task<AirCraftReadDto> GetAircraftByIdAsync(Guid aircraftId);
        Task<IEnumerable<AirCraftReadDto>> GetAllAircraftsAsync();

        // Flight
        Task<FlightReadDto> ScheduleFlightAsync(FlightCreateDto dto);
        Task<FlightReadDto> UpdateFlightAsync(FlightUpdateDto dto);
        Task<FlightReadDto> GetFlightByIdAsync(Guid flightId);
        Task<IEnumerable<FlightReadDto>> GetAllFlightsAsync(string? origin, string? destination, DateTime? departureTime);
        Task<IEnumerable<FlightReadDto>> GetAvailableFlightsAsync();
    }
}
