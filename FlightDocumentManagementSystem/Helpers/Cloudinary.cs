using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace FlightDocumentManagementSystem.Helpers
{
    public static class Cloudinary
    {
        private static IConfiguration _configuration;
        private static CloudinaryDotNet.Cloudinary _cloudinary;

        static Cloudinary()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            _configuration = builder.Build();
            var _cloudName = _configuration["Cloudinary:CloudName"] ?? "";
            var _apiKey = _configuration["Cloudinary:ApiKey"] ?? "";
            var _apiSecret = _configuration["Cloudinary:ApiSecret"] ?? "";

            var account = new Account(_cloudName, _apiKey, _apiSecret);
            _cloudinary = new CloudinaryDotNet.Cloudinary(account);
        }

        public static async Task<ImageUploadResult> Upload(string fileName, IFormFile file)
        {
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(fileName, file.OpenReadStream()),
                PublicId = fileName,
                Folder = "FlightDocumentManagementSystem"
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            return uploadResult;
        }

        public static async Task<bool> Destroy(string publicId)
        {
            var destroyParams = new DeletionParams(publicId);
            var destroyResult = await _cloudinary.DestroyAsync(destroyParams);

            if (destroyResult.Result == "ok")
            {
                return true;
            }
            return false;
        }
    }
}
