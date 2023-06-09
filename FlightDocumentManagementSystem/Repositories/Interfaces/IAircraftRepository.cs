using FlightDocumentManagementSystem.Data;
using FlightDocumentManagementSystem.Models;

namespace FlightDocumentManagementSystem.Repositories.Interfaces
{
    public interface IAircraftRepository : IRepository<Aircraft>
    {
        public Task<List<Aircraft>> GetAllAircraftAsync();
        public Task<PagingDTO<Aircraft>> GetAllAircraftPagingAsync(int pageNumber, int pageSize);
        public Task<Aircraft?> FindAircraftByIdAsync(Guid id);
        public Task<Aircraft> InsertAircraftAsync(AircraftDTO aircraft);
        public Task<Aircraft> UpdateAircraftAsync(Aircraft oldAircraft, AircraftDTO newAircraft);
        public Task DeleteAircraftAsync(Aircraft aircraft);
        public bool ValidateAircraftDTO(AircraftDTO aircraft);
        public Task<bool> CheckSerialToInsertAsync(string serial);
        public Task<bool> CheckSerialToUpdateAsync(string serial, Aircraft aircraft);
    }
}
