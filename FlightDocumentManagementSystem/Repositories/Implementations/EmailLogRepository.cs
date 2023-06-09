using FlightDocumentManagementSystem.Contexts;
using FlightDocumentManagementSystem.Models;
using FlightDocumentManagementSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FlightDocumentManagementSystem.Repositories.Implementations
{
    public class EmailLogRepository : RepositoryBase<EmailLog>, IEmailLogRepository
    {
        public EmailLogRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<bool?> CheckVerifyAsync(Guid emailLogId, Guid accountId)
        {
            var result = await _dbSet.FirstOrDefaultAsync(el => el.EmailLogId == emailLogId && el.AccountId == accountId);
            if (result != null)
            {
                return result.Status;
            }
            return null;
        }

        public async Task<EmailLog?> FindEmailLogByIdAsync(Guid id)
        {
            var result = await FindByIdAsync(id);
            return result;
        }

        public async Task<string> InsertEmailLogAsync(Guid accountId)
        {
            var newEmailLog = new EmailLog()
            {
                EmailLogId = Guid.NewGuid(),
                Status = false,
                AccountId = accountId,
                TimeCreate = DateTime.UtcNow,
                TimeUpdate = DateTime.UtcNow
            };
            await InsertAsync(newEmailLog);
            return newEmailLog.EmailLogId.ToString();
        }

        public async Task UpdateEmailLogAsync(EmailLog emailLog)
        {
            emailLog.Status = true;
            emailLog.TimeUpdate = DateTime.UtcNow;
            await UpdateAsync(emailLog);
        }
    }
}
