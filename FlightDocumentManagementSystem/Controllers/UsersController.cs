using FlightDocumentManagementSystem.Data;
using FlightDocumentManagementSystem.Helpers;
using FlightDocumentManagementSystem.Models;
using FlightDocumentManagementSystem.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Text.RegularExpressions;

namespace FlightDocumentManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IAccountRepository _accountRepository;

        public UsersController(IUserRepository userRepository, IRoleRepository roleRepository, IAccountRepository accountRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _accountRepository = accountRepository;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var result = await _userRepository.GetAllUsersAsync();
            return Ok(new Notification
            {
                Success = true,
                Message = "Get List Successfully",
                Data = result
            });
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(Guid id)
        {
            var result = await _userRepository.FindUserByIdAsync(id);
            if (result == null)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "This user doesn't exist",
                    Data = null
                });
            }
            return Ok(new Notification
            {
                Success = true,
                Message = "Get User Successfully",
                Data = result
            });
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(Guid id, UserDTO user)
        {
            var oldUser = await _userRepository.FindUserByIdAsync(id);
            if (oldUser == null)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "This user doesn't exist",
                    Data = null
                });
            }
            if (_userRepository.ValidateUserDTO(user) == false)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "Please input all information",
                    Data = null
                });
            }
            var role = await _roleRepository.FindRoleByIdAsync(user.RoleId);
            if (role == null)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "This Role doesn't exist",
                    Data = null
                });
            }
            DateTime dateTimeValue;
            if (!DateTime.TryParseExact(user.Birthday, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTimeValue))
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "Birthday Invalid",
                    Data = null
                });
            }
            if (!DateTime.TryParseExact(user.DateStart, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTimeValue))
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "DateStart Invalid",
                    Data = null
                });
            }
            if (Check.IsPhone(user.PhoneNumber!) == false)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "PhoneNumber Invalid",
                    Data = null
                });
            }

            var result = await _userRepository.UpdateUserAsync(oldUser, user);
            return Ok(new Notification
            {
                Success = true,
                Message = "Update Successfully",
                Data = result
            });
        }


        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<User>> PostUser([FromForm] UserDTO user)
        {
            if (_userRepository.ValidateUserDTO(user) == false)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "Please input all information",
                    Data = null
                });
            }
            var role = await _roleRepository.FindRoleByIdAsync(user.RoleId);
            if (role == null)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "This Role doesn't exist",
                    Data = null
                });
            }
            DateTime dateTimeValue;
            if (!DateTime.TryParseExact(user.Birthday, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTimeValue))
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "Birthday Invalid",
                    Data = null
                });
            }
            if (!DateTime.TryParseExact(user.DateStart, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTimeValue))
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "DateStart Invalid",
                    Data = null
                });
            }
            if (Regex.IsMatch(user.PhoneNumber!, @"^\d{10}$") == false)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "PhoneNumber Invalid",
                    Data = null
                });
            }

            var result = await _userRepository.InsertUserAsync(user);
            return Ok(new Notification
            {
                Success = true,
                Message = "Insert Successfully",
                Data = result
            });
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var user = await _userRepository.FindByIdAsync(id);
            if (user == null)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "This user doesn't exist",
                    Data = null
                });
            }
            var account = await _accountRepository.FindAccountByIdAsync(user.AccountId);
            if (account != null)
            {
                await _accountRepository.DeleteAccountAsync(account);
            }

            await _userRepository.DeleteUserAsync(user);
            return Ok(new Notification
            {
                Success = true,
                Message = "Delete Successfully",
                Data = null
            });
        }
    }
}
