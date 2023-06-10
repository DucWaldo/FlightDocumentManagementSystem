using FlightDocumentManagementSystem.Data;
using FlightDocumentManagementSystem.Helpers;
using FlightDocumentManagementSystem.Models;
using FlightDocumentManagementSystem.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlightDocumentManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "AdminOrStaffPolicy")]
    public class PermissionsController : ControllerBase
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly ICategoryRepository _categoryRepository;

        public PermissionsController(IPermissionRepository permissionRepository, IGroupRepository groupRepository, ICategoryRepository categoryRepository)
        {
            _permissionRepository = permissionRepository;
            _groupRepository = groupRepository;
            _categoryRepository = categoryRepository;
        }


        // GET: api/Permissions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Permission>>> GetAllPermissions()
        {
            var result = await _permissionRepository.GetAllPermissionsAsync();
            return Ok(new Notification
            {
                Success = true,
                Message = "Get All Successfully",
                Data = result
            });
        }

        // GET: api/Permissions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Permission>> FindPermission(Guid id)
        {
            var result = await _permissionRepository.FindPermissionByIdAsync(id);
            if (result == null)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "This permission doesn't exist",
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

        // GET: api/Permissions/GetByCategory/5
        [HttpGet("GetByCategory/{categoryId}")]
        public async Task<ActionResult<Permission>> GetPermissionsByCategory(Guid categoryId)
        {
            if (await _categoryRepository.FindCategoryByIdAsync(categoryId) == null)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "This category doesn't exist",
                    Data = null
                });
            }
            var result = await _permissionRepository.GetPermissionByCategoryAsync(categoryId);
            return Ok(new Notification
            {
                Success = true,
                Message = "Get Successfully",
                Data = result
            });
        }

        // POST: api/Permissions
        [HttpPost]
        public async Task<ActionResult<Permission>> PostPermission([FromForm] PermissionDTO permission)
        {
            if (string.IsNullOrEmpty(permission.Function))
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "Please enter all information",
                    Data = null
                });
            }
            if (await _permissionRepository.CheckGroupInCategory(permission.GroupId, permission.CategoryId) == false)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "This Group already exist in this Category",
                    Data = null
                });
            }
            if (_permissionRepository.CheckFunction(permission.Function) == false)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "Function malformed, Please enter \"Read and modify\", \"Read only\", \"No Permission\"",
                    Data = null
                });
            }
            if (await _groupRepository.FindGroupByIdAsync(permission.GroupId) == null)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "This group doesn't exist",
                    Data = null
                });
            }
            if (await _categoryRepository.FindCategoryByIdAsync(permission.CategoryId) == null)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "This category doesn't exist",
                    Data = null
                });
            }

            var result = await _permissionRepository.InsertPermissionAsync(permission);
            return Ok(new Notification
            {
                Success = true,
                Message = "Insert Successfully",
                Data = result
            });
        }

        // PUT: api/Permissions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPermission(Guid id, string function)
        {
            var oldPermission = await _permissionRepository.FindPermissionByIdAsync(id);
            if (oldPermission == null)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "This permission doesn't exist",
                    Data = null
                });
            }
            if (_permissionRepository.CheckFunction(function) == false)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "Function malformed, Please enter \"Read and modify\", \"Read only\", \"No Permission\"",
                    Data = null
                });
            }

            var result = await _permissionRepository.UpdatePermissionAsync(oldPermission, function);
            return Ok(new Notification
            {
                Success = true,
                Message = "Update Successfully",
                Data = result
            });
        }

        // DELETE: api/Permissions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePermission(Guid id)
        {
            var result = await _permissionRepository.FindPermissionByIdAsync(id);
            if (result == null)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "This permission doesn't exist",
                    Data = null
                });
            }
            await _permissionRepository.DeletePermissionAsync(result);
            return Ok(new Notification
            {
                Success = true,
                Message = "Delete Successfully",
                Data = null
            });
        }
    }
}
