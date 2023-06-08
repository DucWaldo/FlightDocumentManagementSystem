using FlightDocumentManagementSystem.Contexts;
using FlightDocumentManagementSystem.Helpers;
using FlightDocumentManagementSystem.Models;
using FlightDocumentManagementSystem.Repositories.Interfaces;

namespace FlightDocumentManagementSystem.Repositories.Implementations
{
    public class DisplayRepository : RepositoryBase<Display>, IDisplayRepository
    {
        public DisplayRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<List<Display>> GetAllDisplaysAsync()
        {
            var result = await GetAllAsync();
            return result;
        }

        public async Task<Display> InsertDisplayAsync(IFormFile file, bool status)
        {
            if (file == null || file.Length == 0)
            {
                var display = new Display()
                {
                    DisplayId = Guid.NewGuid(),
                    LogoUrl = null,
                    PublicLogoUrl = null,
                    CaptchaStatus = status,
                    TimeCreate = DateTime.UtcNow,
                    TimeUpdate = DateTime.UtcNow
                };
                await InsertAsync(display);
                return display;
            }

            var url = await Cloudinary.Upload("Logo", file);
            var newDisplay = new Display()
            {
                DisplayId = Guid.NewGuid(),
                LogoUrl = url.SecureUrl.ToString(),
                PublicLogoUrl = url.PublicId.ToString(),
                CaptchaStatus = false,
                TimeCreate = DateTime.UtcNow,
                TimeUpdate = DateTime.UtcNow
            };
            await InsertAsync(newDisplay);
            return newDisplay;
        }

        public async Task<Display> UpdateCaptchaAsync(bool status, Display display)
        {
            display.CaptchaStatus = status;
            await UpdateAsync(display);
            return display;
        }

        public async Task<Display> UpdateLogoAsync(IFormFile file, Display display)
        {
            var url = await Cloudinary.Upload("Logo", file);
            display.LogoUrl = url.SecureUrl.ToString();
            display.PublicLogoUrl = url.PublicId.ToString();
            await UpdateAsync(display);
            return display;
        }
    }
}
