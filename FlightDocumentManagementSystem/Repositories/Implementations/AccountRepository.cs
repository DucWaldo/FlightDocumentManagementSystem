using FlightDocumentManagementSystem.Contexts;
using FlightDocumentManagementSystem.Data;
using FlightDocumentManagementSystem.Helpers;
using FlightDocumentManagementSystem.Middlewares;
using FlightDocumentManagementSystem.Models;
using FlightDocumentManagementSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FlightDocumentManagementSystem.Repositories.Implementations
{
    public class AccountRepository : RepositoryBase<Account>, IAccountRepository
    {
        public AccountRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<bool> CheckIsExistByEmail(string email)
        {
            var result = await _dbSet.FirstOrDefaultAsync(a => a.Email == email);
            if (result == null)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> CheckLoginAsync(Auth auth)
        {
            var account = await _dbSet.FirstOrDefaultAsync(a => a.Email == auth.Email);
            if (account == null || account.Status == false)
            {
                return false;
            }
            if (PasswordEncryption.VerifyPassword(auth.Password ?? "", account.Password ?? "") == false)
            {
                return false;
            }
            return true;
        }

        public async Task DeleteAccountAsync(Account account)
        {
            await DeleteAsync(account);
        }

        public async Task<Account?> FindAccountByIdAsync(Guid accountId)
        {
            var result = await FindByIdAsync(accountId);
            if (result != null)
            {
                await _dbSet.Entry(result).Reference(a => a.Role).LoadAsync();
            }
            return result;
        }

        public async Task<Account?> GetAccountByEmailAsync(string email)
        {
            var result = await _dbSet.FirstOrDefaultAsync(a => a.Email == email);
            return result;
        }

        public async Task<List<Account>> GetAllAccountsAsync()
        {
            return await GetAllWithIncludeAsync(a => a.Role!);
        }

        public async Task<Account> InsertAccountAsync(AccountDTO account)
        {
            var newAccount = new Account()
            {
                AccountId = Guid.NewGuid(),
                Email = account.Email,
                Password = PasswordEncryption.EncryptPassword(account.Password!, Generate.GetSalt()),
                Status = true,
                TimeCreate = DateTime.UtcNow,
                TimeUpdate = DateTime.UtcNow,
                RoleId = account.RoleId,
            };
            await InsertAsync(newAccount);
            return newAccount;
        }

        public async Task UpdatePasswordAsync(Account account, string password)
        {
            account.Password = PasswordEncryption.EncryptPassword(password, Generate.GetSalt());
            account.TimeUpdate = DateTime.UtcNow;
            await UpdateAsync(account);
        }

        public async Task UpdateRoleAdminAsync(Account oldAccount, Account newAccount, Role adminRole, Role staffRole)
        {
            oldAccount.RoleId = staffRole.RoleId;
            oldAccount.TimeUpdate = DateTime.UtcNow;
            newAccount.RoleId = adminRole.RoleId;
            newAccount.TimeUpdate = DateTime.UtcNow;
            await UpdateAsync(oldAccount);
            await UpdateAsync(newAccount);
        }
    }
}