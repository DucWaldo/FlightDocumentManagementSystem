using FlightDocumentManagementSystem.Data;
using FlightDocumentManagementSystem.Models;

namespace FlightDocumentManagementSystem.Repositories.Interfaces
{
    public interface IAirportRepository : IRepository<Airport>
    {
        public Task<List<Airport>> GetAllAirportsAsync();
        public Task<Airport?> FindAirportAsync(Guid id);
        public Task<Airport> InsertAirportAsync(AirportDTO airport);
        public Task<Airport> UpdateAirportAsync(Airport oldAirport, AirportDTO newAirport);
        public Task DeleteAirportAsync(Airport airport);
        public bool ValidateAirportDTO(AirportDTO airport);
        public Task<bool> CheckAirportToInsertAsync(AirportDTO airport);
        public Task<bool> CheckAirportToUpdateAsync(AirportDTO airport, Guid airportId);
    }
}
