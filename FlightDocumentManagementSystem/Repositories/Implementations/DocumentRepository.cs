using FlightDocumentManagementSystem.Contexts;
using FlightDocumentManagementSystem.Data;
using FlightDocumentManagementSystem.Helpers;
using FlightDocumentManagementSystem.Models;
using FlightDocumentManagementSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FlightDocumentManagementSystem.Repositories.Implementations
{
    public class DocumentRepository : RepositoryBase<Document>, IDocumentRepository
    {
        public DocumentRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<bool> CheckDocumentReturnAsync(Document document)
        {
            var result = await _dbSet.FirstOrDefaultAsync(d => d.FlightId == document.FlightId && d.Version == document.Version && d.Action == true);
            if (result != null)
            {
                return false;
            }
            return true;
        }

        public async Task DeleteDocumentAsync(Document document)
        {
            await DeleteAsync(document);
        }

        public async Task<Document?> FindDocumentById(Guid id)
        {
            var result = await FindByIdAsync(id);
            if (result != null)
            {
                await _dbSet.Entry(result).Reference(d => d.Flight).LoadAsync();
                await _dbSet.Entry(result).Reference(d => d.Category).LoadAsync();
                await _dbSet.Entry(result).Reference(d => d.Account).Query().Include(a => a.Role).LoadAsync();
            }
            return result;
        }

        public async Task<List<Document>> GetAllDocumentAsync()
        {
            var result = await GetAllWithIncludeAsync(d => d.Flight!, d => d.Category!, d => d.Account!.Role!);
            return result;
        }

        public async Task<string> GetNextVersion(DocumentDTO document)
        {
            var documents = await _dbSet.Where(d => d.FlightId == document.FlightId && d.Name! == document.File!.FileName && d.CategoryId == document.CategoryId)
                .OrderByDescending(d => d.Version)
                .Select(d => d.Version).
                FirstOrDefaultAsync();

            if (documents == null)
            {
                return "1.0";
            }
            else
            {
                string[] versionParts = documents.Split('.');
                int majorVersion = int.Parse(versionParts[0]);
                int minorVersion = int.Parse(versionParts[1]);

                // Tăng giá trị minor lên 0.1
                minorVersion += 1;
                if (minorVersion >= 10)
                {
                    minorVersion = 0;
                    majorVersion += 1;
                }

                // Ghép lại chuỗi version mới
                string newVersionString = string.Join(".", majorVersion.ToString(), minorVersion.ToString());
                return newVersionString;

            }
        }

        public async Task<Document> InsertReturnDocumentAsync(Document document, Account creator, IFormFile file)
        {
            var fileName = "Return_" + document.Flight!.FlightNo + "_" + Path.GetFileNameWithoutExtension(document.Name) + "_v" + document.Version;
            var url = await Cloudinary.Upload(fileName, file);
            var newDocument = new Document()
            {
                DocumentId = Guid.NewGuid(),
                Name = document.Name,
                Url = url.SecureUrl.ToString(),
                PublicUrl = url.PublicId.ToString(),
                Version = document.Version,
                Action = true,
                CategoryId = document.CategoryId,
                FlightId = document.FlightId,
                AccountId = creator.AccountId,
                TimeCreate = DateTime.UtcNow,
                TimeUpdate = DateTime.UtcNow
            };
            await InsertAsync(newDocument);
            return newDocument;
        }

        public async Task<Document> InsertSentDocumentAsync(DocumentDTO document, Account creator, Flight flight)
        {
            var version = await GetNextVersion(document);
            var fileName = "Sent_" + flight.FlightNo + "_" + Path.GetFileNameWithoutExtension(document.File!.FileName) + "_v" + version;
            var url = await Cloudinary.Upload(fileName, document.File);
            var newDocument = new Document()
            {
                DocumentId = Guid.NewGuid(),
                Name = document.File!.FileName,
                Url = url.SecureUrl.ToString(),
                PublicUrl = url.PublicId.ToString(),
                Version = version,
                Action = false,
                CategoryId = document.CategoryId,
                FlightId = document.FlightId,
                AccountId = creator.AccountId,
                TimeCreate = DateTime.UtcNow,
                TimeUpdate = DateTime.UtcNow
            };
            await InsertAsync(newDocument);
            return newDocument;
        }
    }
}
