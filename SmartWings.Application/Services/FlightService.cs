using SmartWings.Application.Contracts;
using SmartWings.Application.DTO;
using SmartWings.Domain;
using SmartWings.Infrastructure.Contracts;

namespace SmartWings.Application.Services
{
    public class FlightService : IFlightService
    {
        private readonly IFlightRepository _adminRepository;

        public FlightService(IFlightRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        // ------- Aircraft -------

        public async Task<AirCraftReadDto> AddAircraftAsync(AirCraftCreateDto dto)
        {
            var aircraft = new AirCraft
            {
                AirCraftId = Guid.NewGuid(),
                Model = dto.Model,
                TotalSeats = dto.TotalSeats,
                EconomySeats = dto.EconomySeats,
                BusinessSeats = dto.BusinessSeats
            };

            var created = await _adminRepository.AddAircraftAsync(aircraft);

            return new AirCraftReadDto
            {
                AirCraftId = created.AirCraftId,
                Model = created.Model,
                TotalSeats = created.TotalSeats,
                EconomySeats = created.EconomySeats,
                BusinessSeats = created.BusinessSeats
            };
        }

        public async Task<AirCraftReadDto?> UpdateAircraftAsync(AirCraftUpdateDto dto)
        {
            var aircraft = new AirCraft
            {
                AirCraftId = dto.AirCraftId,
                Model = dto.Model,
                TotalSeats = dto.TotalSeats,
                EconomySeats = dto.EconomySeats,
                BusinessSeats = dto.BusinessSeats
            };

            var updated = await _adminRepository.UpdateAircraftAsync(aircraft);
            if (updated == null) return null;

            return new AirCraftReadDto
            {
                AirCraftId = updated.AirCraftId,
                Model = updated.Model,
                TotalSeats = updated.TotalSeats,
                EconomySeats = updated.EconomySeats,
                BusinessSeats = updated.BusinessSeats
            };
        }

        public async Task<bool> DeleteAircraftAsync(Guid aircraftId)
        {
            return await _adminRepository.DeleteAircraftAsync(aircraftId);
        }

        public async Task<AirCraftReadDto> GetAircraftByIdAsync(Guid aircraftId)
        {
            var aircraft = await _adminRepository.GetAircraftByIdAsync(aircraftId);
            if (aircraft == null) return null;

            return new AirCraftReadDto
            {
                AirCraftId = aircraft.AirCraftId,
                Model = aircraft.Model,
                TotalSeats = aircraft.TotalSeats,
                EconomySeats = aircraft.EconomySeats,
                BusinessSeats = aircraft.BusinessSeats
            };
        }

        public async Task<IEnumerable<AirCraftReadDto>> GetAllAircraftsAsync()
        {
            var list = await _adminRepository.GetAllAircraftsAsync();
            return list.Select(a => new AirCraftReadDto
            {
                AirCraftId = a.AirCraftId,
                Model = a.Model,
                TotalSeats = a.TotalSeats,
                EconomySeats = a.EconomySeats,
                BusinessSeats = a.BusinessSeats
            });
        }

        // ------- Flight -------

        public async Task<FlightReadDto> ScheduleFlightAsync(FlightCreateDto dto)
        {
            Flight flight = new Flight
            {
                FlightId = Guid.NewGuid(),
                FlightNumber = dto.FlightNumber,
                Origin = dto.Origin,
                Destination = dto.Destination,
                DepartureTime = dto.DepartureTime,
                ArrivalTime = dto.ArrivalTime,
                AircraftId = dto.AircraftId,
                Status = dto.Status,
                PriceEconomy = dto.PriceEconomy,
                PriceBusiness = dto.PriceBusiness
            };

            var created = await _adminRepository.ScheduleFlightAsync(flight);

            return MapFlightToDto(created);
        }


        public async Task<FlightReadDto> UpdateFlightAsync(FlightUpdateDto dto)
        {
            var flight = new Flight
            {
                FlightId = dto.FlightId,
                FlightNumber = dto.FlightNumber,
                Origin = dto.Origin,
                Destination = dto.Destination,
                DepartureTime = dto.DepartureTime,
                ArrivalTime = dto.ArrivalTime,
                AircraftId = dto.AircraftId,
                Status = dto.Status,
                PriceEconomy = dto.PriceEconomy,
                PriceBusiness = dto.PriceBusiness
            };

            var updated = await _adminRepository.UpdateFlightAsync(flight);
            if (updated == null) return null;

            return MapFlightToDto(updated);
        }

        public async Task<FlightReadDto> GetFlightByIdAsync(Guid flightId)
        {
            var flight = await _adminRepository.GetFlightByIdAsync(flightId);
            return flight == null ? null : MapFlightToDto(flight);
        }

        public async Task<IEnumerable<FlightReadDto>> GetAllFlightsAsync(string? origin, string? destination, DateTime? departureTime)
        {
            var list = await _adminRepository.GetAllFlightsAsync(origin, destination, departureTime);
            return list.Select(MapFlightToDto);
        }

        public async Task<IEnumerable<FlightReadDto>> GetAvailableFlightsAsync()
        {
            var list = await _adminRepository.GetAvailableFlightsAsync();
            return list.Select((Flight f) => MapFlightToDto(f));
        }



        // Helper
        private FlightReadDto MapFlightToDto(Flight f)
        {
            return new FlightReadDto
            {
                FlightId = f.FlightId,
                FlightNumber = f.FlightNumber,
                Origin = f.Origin,
                Destination = f.Destination,
                DepartureTime = f.DepartureTime,
                ArrivalTime = f.ArrivalTime,
                AircraftId = f.AircraftId,  
                Status = f.Status,
                PriceEconomy = f.PriceEconomy,
                PriceBusiness = f.PriceBusiness
            };
        }
    }
}
