using FlightDocumentManagementSystem.Data;
using FlightDocumentManagementSystem.Helpers;
using FlightDocumentManagementSystem.Models;
using FlightDocumentManagementSystem.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlightDocumentManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "AdminPolicy")]
    public class RolesController : ControllerBase
    {
        private readonly IRoleRepository _roleRepository;
        public RolesController(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        // GET: api/Roles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Role>>> GetRoles()
        {
            var result = await _roleRepository.GetAllRolesAsync();
            return Ok(new Notification
            {
                Success = true,
                Message = "Get List Successfully",
                Data = result
            });
        }

        // GET: api/Roles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Role>> GetRole(Guid id)
        {
            var result = await _roleRepository.FindRoleByIdAsync(id);
            if (result == null)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "This role doesn't exist",
                    Data = null
                });
            }
            return Ok(new Notification
            {
                Success = true,
                Message = "Find Successfully",
                Data = result
            });
        }

        // PUT: api/Roles/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRole(Guid id, RoleDTO role)
        {
            var roles = await _roleRepository.FindRoleByIdAsync(id);
            if (roles == null)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "This role doesn't exist",
                    Data = null
                });
            }
            var result = await _roleRepository.UpdateRoleAsync(roles, role);
            return Ok(new Notification
            {
                Success = true,
                Message = "Update Successfully",
                Data = result
            });
        }

        // POST: api/Roles
        [HttpPost]
        public async Task<ActionResult<Role>> PostRole(RoleDTO role)
        {
            if (role.Name == string.Empty)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "Please enter the role name",
                    Data = null
                });
            }

            var isExist = await _roleRepository.CheckIsExistByName(role.Name!);
            if (isExist == true)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "This role name already exist",
                    Data = null
                });
            }

            var result = await _roleRepository.InsertRoleAsync(role);
            return Ok(new Notification
            {
                Success = true,
                Message = "Insert Successfully",
                Data = result
            });
        }

        // DELETE: api/Roles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(Guid id)
        {
            var role = await _roleRepository.FindRoleByIdAsync(id);
            if (role == null)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "This role doesn't exist"
                });
            }
            await _roleRepository.DeleteRoleAsync(role);
            return Ok(new Notification
            {
                Success = true,
                Message = "Delete Successfully"
            });
        }
    }
}
