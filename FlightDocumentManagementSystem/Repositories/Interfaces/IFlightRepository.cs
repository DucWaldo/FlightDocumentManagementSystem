using FlightDocumentManagementSystem.Data;
using FlightDocumentManagementSystem.Models;

namespace FlightDocumentManagementSystem.Repositories.Interfaces
{
    public interface IFlightRepository : IRepository<Flight>
    {
        public Task<List<Flight>> GetAllFlightAsync();
        public Task<PagingDTO<Flight>> GetAllFlightPagingAsync(int pageNumber, int pageSize);
        public Task<Flight?> FindFlightByIdAsync(Guid id);
        public Task<Flight> InsertFlightAsync(FlightDTO flight);
        public Task<Flight> UpdateFlightAsync(Flight oldFlight, FlightDTO newFlight);
        public Task DeleteFlightAsync(Flight flight);
        public bool ValidateFlightDTO(FlightDTO flight);
        public Task<bool> CheckFlightNoToInsertAsync(string flightNo);
        public Task<bool> CheckFlightNoToUpdateAsync(string flightNo, Flight flight);

    }
}
