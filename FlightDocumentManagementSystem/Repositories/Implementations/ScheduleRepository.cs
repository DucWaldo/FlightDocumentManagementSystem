using FlightDocumentManagementSystem.Contexts;
using FlightDocumentManagementSystem.Data;
using FlightDocumentManagementSystem.Models;
using FlightDocumentManagementSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FlightDocumentManagementSystem.Repositories.Implementations
{
    public class ScheduleRepository : RepositoryBase<Schedule>, IScheduleRepository
    {
        public ScheduleRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<bool> CheckAirportToInsertAsync(ScheduleDTO schedule)
        {
            var result = await _dbSet.FirstOrDefaultAsync(s => s.Point == schedule.Point && s.AirportId == schedule.AirportId && s.FlightId == schedule.FlightId);
            if (result != null)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> CheckAirportToUpdateAsync(ScheduleDTO schedule, Schedule oldSchedule)
        {
            var result = await _dbSet.FirstOrDefaultAsync(s => s.Point == schedule.Point && s.AirportId == schedule.AirportId && s.FlightId == schedule.FlightId && s.ScheduleId != oldSchedule.ScheduleId);
            if (result != null)
            {
                return false;
            }
            var result1 = await _dbSet.FirstOrDefaultAsync(s => s.FlightId == schedule.FlightId && s.AirportId == schedule.AirportId && s.ScheduleId != oldSchedule.ScheduleId);
            if (result1 != null)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> CheckFlightToInsertAsync(Guid flightId)
        {
            var result = await _dbSet.Where(s => s.FlightId == flightId).ToListAsync();
            if (result.Count > 2)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> CheckFlightToUpdateAsync(Guid flightId, Schedule oldSchedule)
        {
            var result = await _dbSet.Where(s => s.FlightId == flightId && s.ScheduleId != oldSchedule.ScheduleId).ToListAsync();
            if (result.Count > 2)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> CheckPointToInsertAsync(ScheduleDTO schedule)
        {
            var result = await _dbSet.FirstOrDefaultAsync(s => s.Point == schedule.Point && s.FlightId == schedule.FlightId);
            if (result != null)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> CheckPointToUpdateAsync(ScheduleDTO schedule, Schedule oldSchedule)
        {
            var result = await _dbSet.FirstOrDefaultAsync(s => s.Point == schedule.Point && s.FlightId == schedule.FlightId && s.ScheduleId != oldSchedule.ScheduleId);
            if (result != null)
            {
                return false;
            }
            return true;
        }

        public async Task DeleteScheduleAsync(Schedule schedule)
        {
            await DeleteAsync(schedule);
        }

        public async Task<Schedule?> FindScheduleByIdAsync(Guid id)
        {
            var result = await FindByIdAsync(id);
            if (result != null)
            {
                await _dbSet.Entry(result).Reference(s => s.Airport).LoadAsync();
                await _dbSet.Entry(result).Reference(s => s.Flight).LoadAsync();
                await _dbSet.Entry(result).Reference(s => s.Aircraft).LoadAsync();
            }
            return result;
        }

        public async Task<List<Schedule>> GetAllScheduleAsync()
        {
            var result = await GetAllWithIncludeAsync(s => s.Airport!, s => s.Flight!, s => s.Aircraft!);
            return result;
        }

        public async Task<Schedule> InsertScheduleAsync(ScheduleDTO schedule)
        {
            var newSchedule = new Schedule()
            {
                ScheduleId = Guid.NewGuid(),
                Point = schedule.Point,
                AirportId = schedule.AirportId,
                FlightId = schedule.FlightId,
                AircraftId = schedule.AircraftId,
                TimeCreate = DateTime.UtcNow,
                TimeUpdate = DateTime.UtcNow
            };

            await InsertAsync(newSchedule);
            return newSchedule;
        }

        public async Task<Schedule> UpdateScheduleAsync(Schedule oldSchedule, ScheduleDTO newSchedule)
        {
            oldSchedule.Point = newSchedule.Point;
            oldSchedule.AirportId = newSchedule.AirportId;
            oldSchedule.FlightId = newSchedule.FlightId;
            oldSchedule.AircraftId = newSchedule.AircraftId;
            oldSchedule.TimeUpdate = DateTime.UtcNow;
            await UpdateAsync(oldSchedule);
            return oldSchedule;
        }
    }
}
