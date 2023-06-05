using FlightDocumentManagementSystem.Contexts;
using FlightDocumentManagementSystem.Data;
using FlightDocumentManagementSystem.Models;
using FlightDocumentManagementSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FlightDocumentManagementSystem.Repositories.Implementations
{
    public class AircraftRepository : RepositoryBase<Aircraft>, IAircraftRepository
    {
        public AircraftRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<bool> CheckSerialToInsertAsync(string serial)
        {
            var result = await _dbSet.FirstOrDefaultAsync(a => a.Serial == serial);
            if (result != null)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> CheckSerialToUpdateAsync(string serial, Aircraft aircraft)
        {
            var result = await _dbSet.FirstOrDefaultAsync(a => a.Serial == serial && a.AircraftId != aircraft.AircraftId);
            if (result != null)
            {
                return false;
            }
            return true;
        }

        public async Task DeleteAircraftAsync(Aircraft aircraft)
        {
            await DeleteAsync(aircraft);
        }

        public async Task<Aircraft?> FindAircraftByIdAsync(Guid id)
        {
            var result = await FindByIdAsync(id);
            return result;
        }

        public async Task<List<Aircraft>> GetAllAircraftAsync()
        {
            var result = await GetAllAsync();
            return result;
        }

        public async Task<Aircraft> InsertAircraftAsync(AircraftDTO aircraft)
        {
            var newAircraft = new Aircraft()
            {
                AircraftId = Guid.NewGuid(),
                Serial = aircraft.Serial,
                Manufacturer = aircraft.Manufacturer,
                Model = aircraft.Model,
                YearOfManufacure = aircraft.YearOfManufacure,
                Status = aircraft.Status,
                TimeCreate = DateTime.UtcNow,
                TimeUpdate = DateTime.UtcNow
            };
            await InsertAsync(newAircraft);
            return newAircraft;
        }

        public async Task<Aircraft> UpdateAircraftAsync(Aircraft oldAircraft, AircraftDTO newAircraft)
        {
            oldAircraft.Serial = newAircraft.Serial;
            oldAircraft.Manufacturer = newAircraft.Manufacturer;
            oldAircraft.Model = newAircraft.Model;
            oldAircraft.YearOfManufacure = newAircraft.YearOfManufacure;
            oldAircraft.Status = newAircraft.Status;
            oldAircraft.TimeUpdate = DateTime.UtcNow;

            await UpdateAsync(oldAircraft);
            return oldAircraft;
        }

        public bool ValidateAircraftDTO(AircraftDTO aircraft)
        {
            if (string.IsNullOrEmpty(aircraft.Serial))
            {
                return false;
            }

            if (string.IsNullOrEmpty(aircraft.Manufacturer))
            {
                return false;
            }

            if (string.IsNullOrEmpty(aircraft.Model))
            {
                return false;
            }

            if (aircraft.YearOfManufacure <= 0)
            {
                return false;
            }

            if (aircraft.Status < 0)
            {
                return false;
            }

            return true;
        }
    }
}
