using FlightDocumentManagementSystem.Data;
using FlightDocumentManagementSystem.Helpers;
using FlightDocumentManagementSystem.Models;
using FlightDocumentManagementSystem.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FlightDocumentManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AirportsController : ControllerBase
    {
        private readonly IAirportRepository _airportRepository;

        public AirportsController(IAirportRepository airportRepository)
        {
            _airportRepository = airportRepository;
        }

        // GET: api/Airports
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Airport>>> GetAirports()
        {
            var result = await _airportRepository.GetAllAirportsAsync();
            return Ok(new Notification
            {
                Success = true,
                Message = "Get All Successfully",
                Data = result
            });
        }

        // GET: api/Airports
        [HttpGet("Paging")]
        public async Task<ActionResult<IEnumerable<Airport>>> GetAirportsPaging(int pageNumber, int pageSize)
        {
            var result = await _airportRepository.GetAllAirportsPagingAsync(pageNumber, pageSize);
            return Ok(new Notification
            {
                Success = true,
                Message = "Get All Successfully",
                Data = result
            });
        }

        // GET: api/Airports/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Airport>> GetAirport(Guid id)
        {
            var result = await _airportRepository.FindAirportAsync(id);
            if (result == null)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "This Airport doesn't exist",
                    Data = null
                });
            }
            return Ok(new Notification
            {
                Success = true,
                Message = "Get All Successfully",
                Data = result
            });
        }

        // POST: api/Airports
        [HttpPost]
        public async Task<ActionResult<Airport>> PostAirport(AirportDTO airport)
        {
            if (_airportRepository.ValidateAirportDTO(airport) == false)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "Please enter all information",
                    Data = null
                });
            }
            if (await _airportRepository.CheckAirportToInsertAsync(airport) == false)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "Name or IATACode or ICAOCode already exist",
                    Data = null
                });
            }
            if (Check.IsEmail(airport.Email!) == false)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "Invalid Email",
                    Data = null
                });
            }

            var result = await _airportRepository.InsertAirportAsync(airport);
            return Ok(new Notification
            {
                Success = true,
                Message = "Insert Successfully",
                Data = result
            });
        }

        // PUT: api/Airports/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAirport(Guid id, AirportDTO airport)
        {
            if (_airportRepository.ValidateAirportDTO(airport) == false)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "Please enter all information",
                    Data = null
                });
            }

            var oldAirport = await _airportRepository.FindAirportAsync(id);
            if (oldAirport == null)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "This Airport doesn't exist",
                    Data = null
                });
            }

            if (await _airportRepository.CheckAirportToUpdateAsync(airport, id) == false)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "Name or IATACode or ICAOCode already exist",
                    Data = null
                });
            }

            if (Check.IsEmail(airport.Email!) == false)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "Invalid Email",
                    Data = null
                });
            }

            var result = await _airportRepository.UpdateAirportAsync(oldAirport, airport);
            return Ok(new Notification
            {
                Success = true,
                Message = "Update Successfully",
                Data = result
            });
        }

        // DELETE: api/Airports/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAirport(Guid id)
        {
            var airport = await _airportRepository.FindAirportAsync(id);
            if (airport == null)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "This Airport doesn't exist",
                    Data = null
                });
            }
            await _airportRepository.DeleteAirportAsync(airport);
            return Ok(new Notification
            {
                Success = true,
                Message = "Delete Successfully",
                Data = null
            });
        }
    }
}
