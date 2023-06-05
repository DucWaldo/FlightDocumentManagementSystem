using FlightDocumentManagementSystem.Contexts;
using FlightDocumentManagementSystem.Data;
using FlightDocumentManagementSystem.Models;
using FlightDocumentManagementSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FlightDocumentManagementSystem.Repositories.Implementations
{
    public class AirportRepository : RepositoryBase<Airport>, IAirportRepository
    {
        public AirportRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<bool> CheckAirportToInsertAsync(AirportDTO airport)
        {
            var result = await _dbSet.FirstOrDefaultAsync(a => a.Name == airport.Name || a.IATACode == airport.IATACode || a.ICAOCode == airport.ICAOCode);
            if (result != null)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> CheckAirportToUpdateAsync(AirportDTO airport, Guid airportId)
        {
            var result = await _dbSet.FirstOrDefaultAsync(a => (a.Name == airport.Name || a.IATACode == airport.IATACode || a.ICAOCode == airport.ICAOCode) && a.AirportId != airportId);
            if (result != null)
            {
                return false;
            }
            return true;
        }

        public async Task DeleteAirportAsync(Airport airport)
        {
            await DeleteAsync(airport);
        }

        public async Task<Airport?> FindAirportAsync(Guid id)
        {
            var result = await FindByIdAsync(id);
            return result;
        }

        public async Task<List<Airport>> GetAllAirportsAsync()
        {
            var result = await GetAllAsync();
            return result;
        }

        public async Task<Airport> InsertAirportAsync(AirportDTO airport)
        {
            var newAirport = new Airport()
            {
                AirportId = Guid.NewGuid(),
                Name = airport.Name,
                City = airport.City,
                Coutry = airport.Coutry,
                IATACode = airport.IATACode,
                ICAOCode = airport.ICAOCode,
                Latitude = airport.Latitude,
                Longitude = airport.Longitude,
                Timezone = airport.Timezone,
                Facility = airport.Facility,
                Contact = airport.Contact,
                Email = airport.Email,
                TimeCreate = DateTime.UtcNow,
                TimeUpdate = DateTime.UtcNow
            };
            await InsertAsync(newAirport);
            return newAirport;
        }

        public async Task<Airport> UpdateAirportAsync(Airport oldAirport, AirportDTO newAirport)
        {
            oldAirport.Name = newAirport.Name;
            oldAirport.City = newAirport.City;
            oldAirport.Coutry = newAirport.Coutry;
            oldAirport.IATACode = newAirport.IATACode;
            oldAirport.ICAOCode = newAirport.ICAOCode;
            oldAirport.Latitude = newAirport.Latitude;
            oldAirport.Longitude = newAirport.Longitude;
            oldAirport.Timezone = newAirport.Timezone;
            oldAirport.Facility = newAirport.Facility;
            oldAirport.Contact = newAirport.Contact;
            oldAirport.Email = newAirport.Email;
            oldAirport.TimeUpdate = DateTime.UtcNow;
            await UpdateAsync(oldAirport);
            return oldAirport;
        }

        public bool ValidateAirportDTO(AirportDTO airport)
        {
            if (string.IsNullOrEmpty(airport.Name))
            {
                return false;
            }

            if (string.IsNullOrEmpty(airport.City))
            {
                return false;
            }

            if (string.IsNullOrEmpty(airport.Coutry))
            {
                return false;
            }

            if (string.IsNullOrEmpty(airport.IATACode))
            {
                return false;
            }

            if (string.IsNullOrEmpty(airport.ICAOCode))
            {
                return false;
            }

            if (string.IsNullOrEmpty(airport.Latitude))
            {
                return false;
            }

            if (string.IsNullOrEmpty(airport.Longitude))
            {
                return false;
            }

            if (string.IsNullOrEmpty(airport.Timezone))
            {
                return false;
            }

            if (string.IsNullOrEmpty(airport.Facility))
            {
                return false;
            }

            if (string.IsNullOrEmpty(airport.Contact))
            {
                return false;
            }

            if (string.IsNullOrEmpty(airport.Email))
            {
                return false;
            }

            return true;
        }
    }
}
