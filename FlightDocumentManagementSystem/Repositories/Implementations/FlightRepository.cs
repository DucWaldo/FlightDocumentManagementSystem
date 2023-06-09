using FlightDocumentManagementSystem.Contexts;
using FlightDocumentManagementSystem.Data;
using FlightDocumentManagementSystem.Models;
using FlightDocumentManagementSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace FlightDocumentManagementSystem.Repositories.Implementations
{
    public class FlightRepository : RepositoryBase<Flight>, IFlightRepository
    {
        public FlightRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<bool> CheckFlightNoToInsertAsync(string flightNo)
        {
            var result = await _dbSet.FirstOrDefaultAsync(f => f.FlightNo == flightNo);
            if (result != null)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> CheckFlightNoToUpdateAsync(string flightNo, Flight flight)
        {
            var result = await _dbSet.FirstOrDefaultAsync(f => f.FlightNo == flightNo && f.FlightId != flight.FlightId);
            if (result != null)
            {
                return false;
            }
            return true;
        }

        public async Task DeleteFlightAsync(Flight flight)
        {
            await DeleteAsync(flight);
        }

        public async Task<Flight?> FindFlightByIdAsync(Guid id)
        {
            var result = await FindByIdAsync(id);
            return result;
        }

        public async Task<List<Flight>> GetAllFlightAsync()
        {
            var result = await GetAllAsync();
            return result;
        }

        public async Task<PagingDTO<Flight>> GetAllFlightPagingAsync(int pageNumber, int pageSize)
        {
            var result = await GetPagingAsync(pageNumber, pageSize, f => f.FlightNo!, false);
            return result;
        }

        public async Task<Flight> InsertFlightAsync(FlightDTO flight)
        {
            var newFlight = new Flight()
            {
                FlightId = Guid.NewGuid(),
                FlightNo = flight.FlightNo,
                Date = DateTime.ParseExact(flight.Date!, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                TimeCreate = DateTime.UtcNow,
                TimeUpdate = DateTime.UtcNow
            };
            await InsertAsync(newFlight);
            return newFlight;
        }

        public async Task<Flight> UpdateFlightAsync(Flight oldFlight, FlightDTO newFlight)
        {
            oldFlight.FlightNo = newFlight.FlightNo;
            oldFlight.Date = DateTime.ParseExact(newFlight.Date!, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            oldFlight.TimeUpdate = DateTime.UtcNow;

            await UpdateAsync(oldFlight);
            return oldFlight;
        }

        public bool ValidateFlightDTO(FlightDTO flight)
        {
            if (string.IsNullOrEmpty(flight.FlightNo))
            {
                return false;
            }

            if (string.IsNullOrEmpty(flight.Date))
            {
                return false;
            }

            DateTime dateTimeValue;
            if (!DateTime.TryParseExact(flight.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTimeValue))
            {
                return false;
            }
            return true;
        }
    }
}
