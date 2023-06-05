using FlightDocumentManagementSystem.Data;
using FlightDocumentManagementSystem.Helpers;
using FlightDocumentManagementSystem.Models;
using FlightDocumentManagementSystem.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FlightDocumentManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AircraftsController : ControllerBase
    {
        private readonly IAircraftRepository _aircraftRepository;

        public AircraftsController(IAircraftRepository aircraftRepository)
        {
            _aircraftRepository = aircraftRepository;
        }

        // GET: api/Aircrafts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Aircraft>>> GetAircrafts()
        {
            var result = await _aircraftRepository.GetAllAircraftAsync();
            return Ok(new Notification
            {
                Success = true,
                Message = "Get all successfully",
                Data = result
            });
        }

        // GET: api/Aircrafts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Aircraft>> GetAircraft(Guid id)
        {
            var result = await _aircraftRepository.FindAircraftByIdAsync(id);
            if (result == null)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "This Aircraft doesn't exist",
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

        //POST: api/Aircraft
        [HttpPost]
        public async Task<ActionResult<Aircraft>> PostAircraft(AircraftDTO aircraft)
        {
            if (_aircraftRepository.ValidateAircraftDTO(aircraft) == false)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "Please enter all valid information",
                    Data = null
                });
            }
            if (await _aircraftRepository.CheckSerialToInsertAsync(aircraft.Serial!) == false)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "This serial already exist",
                    Data = null
                });
            }
            if (aircraft.Status > 2)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "Please choose 0: Shut down, 1: Is maintained, 2: Active",
                    Data = null
                });
            }
            var result = await _aircraftRepository.InsertAircraftAsync(aircraft);
            return Ok(new Notification
            {
                Success = true,
                Message = "Insert successfully",
                Data = result
            });
        }

        // PUT: api/Aircrafts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAircraft(Guid id, AircraftDTO aircraft)
        {
            if (_aircraftRepository.ValidateAircraftDTO(aircraft) == false)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "Please enter all valid information",
                    Data = null
                });
            }
            var oldAircraft = await _aircraftRepository.FindAircraftByIdAsync(id);
            if (oldAircraft == null)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "This Aircraft doesn't exist",
                    Data = null
                });
            }
            if (await _aircraftRepository.CheckSerialToUpdateAsync(aircraft.Serial!, oldAircraft) == false)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "This serial already exist",
                    Data = null
                });
            }
            if (aircraft.Status > 2)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "Please choose 0: Shut down, 1: Is maintained, 2: Active",
                    Data = null
                });
            }
            var result = await _aircraftRepository.UpdateAircraftAsync(oldAircraft, aircraft);
            return Ok(new Notification
            {
                Success = true,
                Message = "Update successfully",
                Data = result
            });
        }

        // DELETE: api/Aircraft/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAircraft(Guid id)
        {
            var result = await _aircraftRepository.FindAircraftByIdAsync(id);
            if (result == null)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "This Aircraft doesn't exist",
                    Data = null
                });
            }
            await _aircraftRepository.DeleteAircraftAsync(result);
            return Ok(new Notification
            {
                Success = true,
                Message = "Delete successfully",
                Data = result
            });
        }
    }
}
