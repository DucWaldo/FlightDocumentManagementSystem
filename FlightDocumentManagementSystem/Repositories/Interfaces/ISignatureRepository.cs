using FlightDocumentManagementSystem.Models;

namespace FlightDocumentManagementSystem.Repositories.Interfaces
{
    public interface ISignatureRepository : IRepository<Signature>
    {
        public Task<Signature?> FindSignatureByDocumentId(Guid documentId);
        public Task InsertSignatureAsync(IFormFile signature, Document document);
        public Task DeleteSignatureAsync(Signature signature);
    }
}
