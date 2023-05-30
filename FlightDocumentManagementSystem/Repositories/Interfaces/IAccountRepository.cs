using FlightDocumentManagementSystem.Data;
using FlightDocumentManagementSystem.Models;

namespace FlightDocumentManagementSystem.Repositories.Interfaces
{
    public interface IAccountRepository : IRepository<Account>
    {
        public Task<List<Account>> GetAllAccountsAsync();
        public Task<Account?> FindAccountByIdAsync(Guid accountId);
        public Task<Account> InsertAccountAsync(AccountDTO account);
        public Task DeleteAccountAsync(Account account);
        public Task<bool> CheckIsExistByEmail(string email);
        public bool CheckIsEmailValid(string email);
    }
}
