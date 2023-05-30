using FlightDocumentManagementSystem.Contexts;
using FlightDocumentManagementSystem.Data;
using FlightDocumentManagementSystem.Middlewares;
using FlightDocumentManagementSystem.Models;
using FlightDocumentManagementSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace FlightDocumentManagementSystem.Repositories.Implementations
{
    public class AccountRepository : RepositoryBase<Account>, IAccountRepository
    {
        public AccountRepository(ApplicationDbContext context) : base(context)
        {
        }

        public bool CheckIsEmailValid(string email)
        {
            // Regular expression to check format email@vietjetair.com
            string emailPattern = @"^[a-zA-Z0-9_.+-]+@vietjetair\.com$";

            // Check email format using Regex.IsMatch()
            bool isValid = Regex.IsMatch(email, emailPattern);

            return isValid;
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
                Password = PasswordEncryption.EncryptPassword(account.Password!),
                Status = true,
                TimeCreate = DateTime.UtcNow,
                TimeUpdate = DateTime.UtcNow,
                RoleId = account.RoleId,
            };
            await InsertAsync(newAccount);
            return newAccount;
        }
    }
}