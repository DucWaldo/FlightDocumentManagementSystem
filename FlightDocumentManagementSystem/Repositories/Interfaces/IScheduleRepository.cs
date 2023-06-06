using FlightDocumentManagementSystem.Data;
using FlightDocumentManagementSystem.Models;

namespace FlightDocumentManagementSystem.Repositories.Interfaces
{
    public interface IScheduleRepository : IRepository<Schedule>
    {
        public Task<List<Schedule>> GetAllScheduleAsync();
        public Task<Schedule?> FindScheduleByIdAsync(Guid id);
        public Task<Schedule> InsertScheduleAsync(ScheduleDTO schedule);
        public Task<Schedule> UpdateScheduleAsync(Schedule oldSchedule, ScheduleDTO newSchedule);
        public Task DeleteScheduleAsync(Schedule schedule);
        public Task<bool> CheckFlightToInsertAsync(Guid flightId);
        public Task<bool> CheckAirportToInsertAsync(ScheduleDTO schedule);
        public Task<bool> CheckPointToInsertAsync(ScheduleDTO schedule);
        public Task<bool> CheckFlightToUpdateAsync(Guid flightId, Schedule oldSchedule);
        public Task<bool> CheckAirportToUpdateAsync(ScheduleDTO schedule, Schedule oldSchedule);
        public Task<bool> CheckPointToUpdateAsync(ScheduleDTO schedule, Schedule oldSchedule);
    }
}
