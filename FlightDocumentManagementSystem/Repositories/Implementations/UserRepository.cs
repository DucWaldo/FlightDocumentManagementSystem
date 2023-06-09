using FlightDocumentManagementSystem.Contexts;
using FlightDocumentManagementSystem.Data;
using FlightDocumentManagementSystem.Helpers;
using FlightDocumentManagementSystem.Models;
using FlightDocumentManagementSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace FlightDocumentManagementSystem.Repositories.Implementations
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {

        private readonly IAccountRepository _accountRepository;
        public UserRepository(ApplicationDbContext context, IAccountRepository accountRepository) : base(context)
        {
            _accountRepository = accountRepository;

        }

        public async Task DeleteUserAsync(User user)
        {
            await DeleteAsync(user);
        }

        public async Task<User?> FindUserByIdAsync(Guid id)
        {
            var result = await FindByIdAsync(id);
            if (result != null)
            {
                await _dbSet.Entry(result).Reference(u => u.Account).Query().Include(a => a.Role).LoadAsync();
            }
            return result;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            var result = await GetAllWithIncludeAsync(u => u.Account!.Role!);
            return result;
        }

        public async Task<PagingDTO<User>> GetAllUsersPagingAsync(int pageNumber, int pageSize)
        {
            var result = await GetPagingAsync(pageNumber, pageSize, u => u.StaffCode!, false);
            return result;
        }

        public async Task<User> InsertUserAsync(UserDTO user)
        {
            var staffCode = "vj" + Generate.GetStaffCode();
            var newUser = new User()
            {
                UserId = Guid.NewGuid(),
                StaffCode = staffCode,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Birthday = DateTime.ParseExact(user.Birthday!, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                Gender = user.Gender,
                Address = user.Address,
                City = user.City,
                PhoneNumber = user.PhoneNumber,
                DateStart = DateTime.ParseExact(user.DateStart!, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                Status = true,
                TimeCreate = DateTime.UtcNow,
                TimeUpdate = DateTime.UtcNow,
                AccountId = _accountRepository.InsertAccountAsync(new AccountDTO
                {
                    Email = staffCode + "@vietjetair.com",
                    Password = staffCode,
                    RoleId = user.RoleId
                }).Result.AccountId
            };
            await InsertAsync(newUser);
            return newUser;
        }

        public async Task<User> UpdateUserAsync(User oldUser, UserDTO newUser)
        {
            oldUser.FirstName = newUser.FirstName;
            oldUser.LastName = newUser.LastName;
            oldUser.Birthday = DateTime.Parse(newUser.Birthday ?? "");
            oldUser.Gender = newUser.Gender;
            oldUser.Address = newUser.Address;
            oldUser.City = newUser.City;
            oldUser.DateStart = DateTime.Parse(newUser.DateStart ?? "");
            oldUser.TimeUpdate = DateTime.UtcNow;

            await UpdateAsync(oldUser);
            return oldUser;
        }

        public bool ValidateUserDTO(UserDTO user)
        {
            if (string.IsNullOrEmpty(user.FirstName))
            {
                return false;
            }

            if (string.IsNullOrEmpty(user.LastName))
            {
                return false;
            }

            if (string.IsNullOrEmpty(user.Birthday))
            {
                return false;
            }

            if (string.IsNullOrEmpty(user.Address))
            {
                return false;
            }

            if (string.IsNullOrEmpty(user.City))
            {
                return false;
            }

            if (string.IsNullOrEmpty(user.PhoneNumber))
            {
                return false;
            }

            if (string.IsNullOrEmpty(user.DateStart))
            {
                return false;
            }

            return true;
        }
    }
}
