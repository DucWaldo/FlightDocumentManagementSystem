using FlightDocumentManagementSystem.Data;
using FlightDocumentManagementSystem.Middlewares;
using FlightDocumentManagementSystem.Models;

namespace FlightDocumentManagementSystem.Repositories.Interfaces
{
    public interface IAccountRepository : IRepository<Account>
    {
        public Task<List<Account>> GetAllAccountsAsync();
        public Task<PagingDTO<Account>> GetAllAccountsPagingAsync(int pageNumber, int pageSize);
        public Task<Account?> GetAccountByEmailAsync(string email);
        public Task<Account?> FindAccountByIdAsync(Guid accountId);
        public Task<Account> InsertAccountAsync(AccountDTO account);
        public Task UpdatePasswordAsync(Account account, string password);
        public Task UpdateRoleAdminAsync(Account oldAccount, Account newAccount, Role adminRole, Role staffRole);
        public Task DeleteAccountAsync(Account account);
        public Task<bool> CheckIsExistByEmail(string email);
        public Task<bool> CheckLoginAsync(Auth auth);
    }
}
