using FlightDocumentManagementSystem.Data;
using FlightDocumentManagementSystem.Helpers;
using FlightDocumentManagementSystem.Models;
using FlightDocumentManagementSystem.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlightDocumentManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "AdminOrStaffPolicy")]
    public class SchedulesController : ControllerBase
    {
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IAircraftRepository _aircraftRepository;
        private readonly IAirportRepository _airportRepository;
        private readonly IFlightRepository _flightRepository;

        public SchedulesController(IScheduleRepository scheduleRepository, IAircraftRepository aircraftRepository, IAirportRepository airportRepository, IFlightRepository flightRepository)
        {
            _scheduleRepository = scheduleRepository;
            _aircraftRepository = aircraftRepository;
            _airportRepository = airportRepository;
            _flightRepository = flightRepository;
        }

        // GET: api/Schedules
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Schedule>>> GetSchedules()
        {
            var result = await _scheduleRepository.GetAllScheduleAsync();
            return Ok(new Notification
            {
                Success = true,
                Message = "Get all successfully",
                Data = result
            });
        }

        // GET: api/Schedules/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Schedule>> GetSchedule(Guid id)
        {
            var result = await _scheduleRepository.FindScheduleByIdAsync(id);
            if (result == null)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "This schedule doesn't exist",
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

        // POST: api/Schedules
        [HttpPost]
        public async Task<ActionResult<Schedule>> PostSchedule([FromForm] ScheduleDTO schedule)
        {
            if (schedule.Point < 0 || schedule.Point > 1)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "Choose 0: Point of Loading, 1: Point of Unloading",
                    Data = null
                });
            }

            if (await _aircraftRepository.FindAircraftByIdAsync(schedule.AircraftId) == null)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "This Aircraft doesn't exist",
                    Data = null
                });
            }

            if (await _airportRepository.FindAirportAsync(schedule.AirportId) == null)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "This Airport doesn't exist",
                    Data = null
                });
            }

            if (await _flightRepository.FindFlightByIdAsync(schedule.FlightId) == null)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "This Flight doesn't exist",
                    Data = null
                });
            }

            if (await _scheduleRepository.CheckFlightToInsertAsync(schedule.FlightId) == false)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "This Flight already has 2 Airport",
                    Data = null
                });
            }

            if (await _scheduleRepository.CheckAirportToInsertAsync(schedule) == false)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "This Airport with this point and this Flight already exist",
                    Data = null
                });
            }

            if (await _scheduleRepository.CheckPointToInsertAsync(schedule) == false)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "Invalid this point",
                    Data = null
                });
            }

            var result = await _scheduleRepository.InsertScheduleAsync(schedule);
            return Ok(new Notification
            {
                Success = true,
                Message = "Insert successfully",
                Data = result
            });
        }

        // PUT: api/Schedules/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSchedule(Guid id, [FromForm] ScheduleDTO schedule)
        {
            var oldSchedule = await _scheduleRepository.FindScheduleByIdAsync(id);
            if (oldSchedule == null)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "This schedule doesn't exist",
                    Data = null
                });
            }
            if (schedule.Point < 0 || schedule.Point > 1)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "Choose 0: Point of Loading, 1: Point of Unloading",
                    Data = null
                });
            }

            if (await _aircraftRepository.FindAircraftByIdAsync(schedule.AircraftId) == null)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "This Aircraft doesn't exist",
                    Data = null
                });
            }

            if (await _airportRepository.FindAirportAsync(schedule.AirportId) == null)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "This Airport doesn't exist",
                    Data = null
                });
            }

            if (await _flightRepository.FindFlightByIdAsync(schedule.FlightId) == null)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "This Flight doesn't exist",
                    Data = null
                });
            }

            if (await _scheduleRepository.CheckFlightToUpdateAsync(schedule.FlightId, oldSchedule) == false)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "This Flight already has 2 Airport",
                    Data = null
                });
            }

            if (await _scheduleRepository.CheckAirportToUpdateAsync(schedule, oldSchedule) == false)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "This Airport with this point and this Flight already exist",
                    Data = null
                });
            }

            if (await _scheduleRepository.CheckPointToUpdateAsync(schedule, oldSchedule) == false)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "Invalid this point",
                    Data = null
                });
            }

            var result = await _scheduleRepository.UpdateScheduleAsync(oldSchedule, schedule);
            return Ok(new Notification
            {
                Success = true,
                Message = "Update successfully",
                Data = result
            });
        }

        // DELETE: api/Schedules/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSchedule(Guid id)
        {
            var result = await _scheduleRepository.FindScheduleByIdAsync(id);
            if (result == null)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "This schedule doesn't exist",
                    Data = null
                });
            }
            await _scheduleRepository.DeleteScheduleAsync(result);
            return Ok(new Notification
            {
                Success = true,
                Message = "Delete successfully",
                Data = null
            });
        }
    }
}
