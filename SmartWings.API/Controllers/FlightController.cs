using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartWings.Application.Contracts;
using SmartWings.Application.DTO;

namespace SmartWings.API.Controllers
{
    /// <summary>
    /// Controller responsible for managing aircrafts and flights.
    /// </summary>
    [ApiController]
    [Route("api/flights")]
    public class FlightController : ControllerBase
    {
        private readonly IFlightService _flightService;

        /// <summary>
        /// Constructor injection of flight service.
        /// </summary>
        public FlightController(IFlightService flightService)
        {
            _flightService = flightService;
        }

        // ===========================
        // ✈ AIRCRAFT ENDPOINTS
        // ===========================

        /// <summary>
        /// Adds a new aircraft.
        /// Only accessible by Admins.
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpPost("aircrafts")]
        public async Task<ActionResult<AirCraftReadDto>> AddAircraft([FromBody] AirCraftCreateDto dto)
        {
            var result = await _flightService.AddAircraftAsync(dto);
            return CreatedAtAction(nameof(GetAircraftById), new { aircraftId = result.AirCraftId }, result);
        }

        /// <summary>
        /// Updates an existing aircraft by ID.
        /// Only accessible by Admins.
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpPut("aircrafts/{aircraftId}")]
        public async Task<ActionResult<AirCraftReadDto>> UpdateAircraft(Guid aircraftId, [FromBody] AirCraftUpdateDto dto)
        {
            if (aircraftId != dto.AirCraftId)
                return BadRequest("Mismatched ID");

            var result = await _flightService.UpdateAircraftAsync(dto);
            return result == null ? NotFound() : Ok(result);
        }

        /// <summary>
        /// Deletes an aircraft by ID.
        /// Only accessible by Admins.
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpDelete("aircrafts/{aircraftId}")]
        public async Task<IActionResult> DeleteAircraft(Guid aircraftId)
        {
            var deleted = await _flightService.DeleteAircraftAsync(aircraftId);
            return deleted ? NoContent() : NotFound();
        }

        /// <summary>
        /// Gets a list of all aircrafts.
        /// Accessible by both Admins and Users.
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpGet("aircrafts")]
        public async Task<ActionResult<IEnumerable<AirCraftReadDto>>> GetAllAircrafts()
        {
            var aircrafts = await _flightService.GetAllAircraftsAsync();
            return Ok(aircrafts);
        }

        /// <summary>
        /// Gets an aircraft by its ID.
        /// Accessible by both Admins and Users.
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpGet("aircrafts/{aircraftId}")]
        public async Task<ActionResult<AirCraftReadDto>> GetAircraftById(Guid aircraftId)
        {
            var aircraft = await _flightService.GetAircraftByIdAsync(aircraftId);
            return aircraft == null ? NotFound() : Ok(aircraft);
        }

        // ===========================
        // 🛫 FLIGHT ENDPOINTS
        // ===========================

        /// <summary>
        /// Schedules a new flight.
        /// Only accessible by Admins.
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<FlightReadDto>> ScheduleFlight([FromBody] FlightCreateDto dto)
        {
            var result = await _flightService.ScheduleFlightAsync(dto);
            return CreatedAtAction(nameof(GetFlightById), new { flightId = result.FlightId }, result);
        }

        /// <summary>
        /// Updates an existing flight by ID.
        /// Only accessible by Admins.
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpPut("{flightId}")]
        public async Task<ActionResult<FlightReadDto>> UpdateFlight(Guid flightId, [FromBody] FlightUpdateDto dto)
        {
            if (flightId != dto.FlightId)
                return BadRequest("Mismatched ID");

            var result = await _flightService.UpdateFlightAsync(dto);
            return result == null ? NotFound() : Ok(result);
        }

        /// <summary>
        /// Gets all flights, optionally filtered by origin, destination, and departure date.
        /// Accessible by both Admins and Users.
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FlightReadDto>>> GetAllFlights(
            [FromQuery] string? origin,
            [FromQuery] string? destination,
            [FromQuery] DateTime? departureDate)
        {
            var flights = await _flightService.GetAllFlightsAsync(origin, destination, departureDate);
            return Ok(flights);
        }

        /// <summary>
        /// Gets all flights that are currently available for booking.
        /// Accessible by both Admins and Users.
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpGet("available")]
        public async Task<ActionResult<IEnumerable<FlightReadDto>>> GetAvailableFlights()
        {
            var flights = await _flightService.GetAvailableFlightsAsync();
            return Ok(flights);
        }

        /// <summary>
        /// Gets a flight by its ID.
        /// Accessible by both Admins and Users.
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpGet("{flightId}")]
        public async Task<ActionResult<FlightReadDto>> GetFlightById(Guid flightId)
        {
            var flight = await _flightService.GetFlightByIdAsync(flightId);
            return flight == null ? NotFound() : Ok(flight);
        }
    }
}
