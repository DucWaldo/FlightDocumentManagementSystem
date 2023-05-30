using FlightDocumentManagementSystem.Data;
using FlightDocumentManagementSystem.Helpers;
using FlightDocumentManagementSystem.Models;
using FlightDocumentManagementSystem.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FlightDocumentManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IRoleRepository _roleRepository;

        public AccountsController(IAccountRepository accountRepository, IRoleRepository roleRepository)
        {
            _accountRepository = accountRepository;
            _roleRepository = roleRepository;
        }

        // GET: api/Roles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Account>>> GetAccounts()
        {
            var result = await _accountRepository.GetAllAccountsAsync();
            return Ok(new Notification
            {
                Success = true,
                Message = "Get List Successfully",
                Data = result
            });
        }

        // GET: api/Roles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Account>> FindAccount(Guid id)
        {
            var result = await _accountRepository.FindAccountByIdAsync(id);
            if (result == null)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "This account doesn't exist",
                    Data = null
                });
            }
            return Ok(new Notification
            {
                Success = true,
                Message = "Find Account Successfully",
                Data = result
            });
        }

        // POST: api/Roles
        [HttpPost]
        public async Task<ActionResult<Account>> PostAccount([FromForm] AccountDTO account)
        {
            if (account.Email == null || account.Password == null || account.RoleId == Guid.Empty)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "Please Enter All Information",
                    Data = null
                });
            }

            var role = await _roleRepository.FindRoleByIdAsync(account.RoleId);
            if (role == null)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "This role doesn't exist",
                    Data = null
                });
            }
            if (_accountRepository.CheckIsEmailValid(account.Email!) == false)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "Invalid email format email@vietjetair.com",
                    Data = null
                });
            }
            var checkEmailExist = await _accountRepository.CheckIsExistByEmail(account.Email);
            if (checkEmailExist == true)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "This account already exist",
                    Data = null
                });
            }

            var result = await _accountRepository.InsertAccountAsync(account);
            return Ok(new Notification
            {
                Success = true,
                Message = "Insert Successfully",
                Data = result
            });
        }

        // DELETE: api/Roles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(Guid id)
        {
            var account = await _accountRepository.FindAccountByIdAsync(id);
            if (account == null)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "This account doesn't exist",
                    Data = null
                });
            }

            await _accountRepository.DeleteAccountAsync(account);
            return Ok(new Notification
            {
                Success = true,
                Message = "Delete Successfully",
                Data = null
            });
        }
    }
}
