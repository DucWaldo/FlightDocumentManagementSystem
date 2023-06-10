using FlightDocumentManagementSystem.Data;
using FlightDocumentManagementSystem.Helpers;
using FlightDocumentManagementSystem.Models;
using FlightDocumentManagementSystem.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace FlightDocumentManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "AdminOrStaffPolicy")]
    public class FlightsController : ControllerBase
    {
        private readonly IFlightRepository _flightRepository;

        public FlightsController(IFlightRepository flightRepository)
        {
            _flightRepository = flightRepository;
        }

        // GET: api/Flights
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Flight>>> GetFlights()
        {
            var result = await _flightRepository.GetAllFlightAsync();
            return Ok(new Notification
            {
                Success = true,
                Message = "Get all successfully",
                Data = result
            });
        }

        // GET: api/Flights/Paging
        [HttpGet("Paging")]
        public async Task<ActionResult<IEnumerable<Flight>>> GetFlights(int pageNumber, int pageSize)
        {
            var result = await _flightRepository.GetAllFlightPagingAsync(pageNumber, pageSize);
            return Ok(new Notification
            {
                Success = true,
                Message = "Get all successfully",
                Data = result
            });
        }

        // GET: api/Flights/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Flight>> GetFlight(Guid id)
        {
            var result = await _flightRepository.FindFlightByIdAsync(id);
            if (result == null)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "This flight doesn't exist",
                    Data = null
                });
            }
            return Ok(new Notification
            {
                Success = true,
                Message = "Find successfully",
                Data = result
            });
        }

        // POST: api/Flights
        [HttpPost]
        public async Task<ActionResult<Flight>> PostFlight(FlightDTO flight)
        {
            if (_flightRepository.ValidateFlightDTO(flight) == false)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "Please enter all valid information",
                    Data = null
                });
            }

            if (await _flightRepository.CheckFlightNoToInsertAsync(flight.FlightNo!) == false)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "FlightNo Already exist",
                    Data = null
                });
            }

            DateTime dateTimeValue;
            if (!DateTime.TryParseExact(flight.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTimeValue))
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "Invalid date",
                    Data = null
                });
            }
            if (dateTimeValue.Date < DateTime.Today)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "Date is less than current date",
                    Data = null
                });
            }

            var result = await _flightRepository.InsertFlightAsync(flight);
            return Ok(new Notification
            {
                Success = true,
                Message = "Insert successfully",
                Data = result
            });
        }

        // PUT: api/Flights/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFlight(Guid id, FlightDTO flight)
        {
            var oldFlight = await _flightRepository.FindFlightByIdAsync(id);
            if (oldFlight == null)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "This flight doesn't exist",
                    Data = null
                });
            }

            if (_flightRepository.ValidateFlightDTO(flight) == false)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "Please enter all valid information",
                    Data = null
                });
            }

            if (await _flightRepository.CheckFlightNoToUpdateAsync(flight.FlightNo!, oldFlight) == false)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "FlightNo Already exist",
                    Data = null
                });
            }

            DateTime dateTimeValue;
            if (!DateTime.TryParseExact(flight.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTimeValue))
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "Invalid date",
                    Data = null
                });
            }

            if (dateTimeValue.Date < DateTime.Today)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "Date is less than current date",
                    Data = null
                });
            }

            var result = await _flightRepository.UpdateFlightAsync(oldFlight, flight);
            return Ok(new Notification
            {
                Success = true,
                Message = "Update successfully",
                Data = result
            });
        }

        // DELETE: api/Flights/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFlight(Guid id)
        {
            var result = await _flightRepository.FindFlightByIdAsync(id);
            if (result == null)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "This flight doesn't exist",
                    Data = null
                });
            }
            await _flightRepository.DeleteFlightAsync(result);
            return Ok(new Notification
            {
                Success = true,
                Message = "Delete successfully",
                Data = null
            });
        }
    }
}
