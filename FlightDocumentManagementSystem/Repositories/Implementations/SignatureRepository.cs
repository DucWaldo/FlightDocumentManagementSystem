using FlightDocumentManagementSystem.Contexts;
using FlightDocumentManagementSystem.Helpers;
using FlightDocumentManagementSystem.Models;
using FlightDocumentManagementSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FlightDocumentManagementSystem.Repositories.Implementations
{
    public class SignatureRepository : RepositoryBase<Signature>, ISignatureRepository
    {
        public SignatureRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task DeleteSignatureAsync(Signature signature)
        {
            await DeleteAsync(signature);
        }

        public async Task<Signature?> FindSignatureByDocumentId(Guid documentId)
        {
            var result = await _dbSet.FirstOrDefaultAsync(s => s.DocumentId == documentId);
            return result;
        }

        public async Task InsertSignatureAsync(IFormFile signature, Document document)
        {
            var url = await Cloudinary.Upload(Guid.NewGuid().ToString(), signature!);
            var newSignature = new Signature()
            {
                SignatureId = Guid.NewGuid(),
                Url = url.SecureUrl.ToString(),
                PublicUrl = url.PublicId.ToString(),
                TimeCreate = DateTime.UtcNow,
                TimeUpdate = DateTime.UtcNow,
                DocumentId = document.DocumentId
            };
            await InsertAsync(newSignature);
        }
    }
}
