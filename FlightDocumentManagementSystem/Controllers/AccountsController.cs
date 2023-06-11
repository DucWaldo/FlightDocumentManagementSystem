using FlightDocumentManagementSystem.Helpers;
using FlightDocumentManagementSystem.Middlewares;
using FlightDocumentManagementSystem.Models;
using FlightDocumentManagementSystem.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        // GET: api/Accounts
        [HttpGet]
        [Authorize(Policy = "AdminPolicy")]
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

        // GET: api/Accounts/Paging
        [HttpGet("Paging")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<ActionResult<IEnumerable<Account>>> GetAccountsPaging(int pageNumber, int pageSize)
        {
            var result = await _accountRepository.GetAllAccountsPagingAsync(pageNumber, pageSize);
            return Ok(new Notification
            {
                Success = true,
                Message = "Get List Successfully",
                Data = result
            });
        }

        // GET: api/Accounts/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Account>> GetAccount(Guid id)
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

        // PUT: api/Accounts/ChangeAdmin
        [HttpPut("ChangeAdmin")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<ActionResult<Account>> PutChangeAdmin(string email, string password)
        {
            var oldAdmin = await _accountRepository.FindAccountByIdAsync(Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            var newAdmin = await _accountRepository.GetAccountByEmailAsync(email);
            if (oldAdmin == null || newAdmin == null)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "Account doesn't exist or invalid",
                    Data = null
                });
            }
            if (PasswordEncryption.VerifyPassword(password, oldAdmin.Password!) == false)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "Invalid password",
                    Data = null
                });
            }
            var adminRole = await _roleRepository.FindRoleByNameAsync("Admin");
            var staffRole = await _roleRepository.FindRoleByNameAsync("Staff");
            if (adminRole == null || staffRole == null)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "This role doesn't exist",
                    Data = null
                });
            }
            await _accountRepository.UpdateRoleAdminAsync(oldAdmin, newAdmin, adminRole, staffRole);
            return Ok(new Notification
            {
                Success = true,
                Message = $"Admin role tranferred from account {oldAdmin.Email} to account {newAdmin.Email} successfully",
                Data = null
            });
        }
    }
}
