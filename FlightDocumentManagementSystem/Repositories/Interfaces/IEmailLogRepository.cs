using FlightDocumentManagementSystem.Models;

namespace FlightDocumentManagementSystem.Repositories.Interfaces
{
    public interface IEmailLogRepository : IRepository<EmailLog>
    {
        public Task<EmailLog?> FindEmailLogByIdAsync(Guid id);
        public Task<string> InsertEmailLogAsync(Guid accountId);
        public Task UpdateEmailLogAsync(EmailLog emailLog);
        public Task<bool?> CheckVerifyAsync(Guid emailLogId, Guid accountId);
    }
}
