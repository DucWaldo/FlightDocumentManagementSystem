using FlightDocumentManagementSystem.Data;
using FlightDocumentManagementSystem.Models;

namespace FlightDocumentManagementSystem.Repositories.Interfaces
{
    public interface IDocumentRepository : IRepository<Document>
    {
        public Task<List<Document>> GetAllDocumentAsync();
        public Task<PagingDTO<Document>> GetAllDocumentPagingAsync(int pageNumber, int pageSize);
        public Task<string> GetNextVersion(DocumentDTO document);
        public Task<Document?> FindDocumentById(Guid id);
        public Task<Document> InsertSentDocumentAsync(DocumentDTO document, Account creator, Flight flight);
        public Task<Document> InsertReturnDocumentAsync(Document document, Account creator, IFormFile file);
        public Task DeleteDocumentAsync(Document document);
        public Task<bool> CheckDocumentReturnAsync(Document document);
    }
}
