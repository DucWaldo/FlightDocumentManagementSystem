using FlightDocumentManagementSystem.Models;

namespace FlightDocumentManagementSystem.Repositories.Interfaces
{
    public interface IDisplayRepository : IRepository<Display>
    {
        public Task<List<Display>> GetAllDisplaysAsync();
        public Task<Display> InsertDisplayAsync(IFormFile file, bool status);
        public Task<Display> UpdateLogoAsync(IFormFile file, Display display);
        public Task<Display> UpdateCaptchaAsync(bool status, Display display);
    }
}
