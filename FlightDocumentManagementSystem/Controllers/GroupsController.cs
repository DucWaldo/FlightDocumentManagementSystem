using FlightDocumentManagementSystem.Data;
using FlightDocumentManagementSystem.Helpers;
using FlightDocumentManagementSystem.Models;
using FlightDocumentManagementSystem.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FlightDocumentManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GroupsController : ControllerBase
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IMemberRepository _memberRepository;

        public GroupsController(IGroupRepository groupRepository, IAccountRepository accountRepository, IMemberRepository memberRepository)
        {
            _groupRepository = groupRepository;
            _accountRepository = accountRepository;
            _memberRepository = memberRepository;
        }

        // GET: api/Groups
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Group>>> GetGroups()
        {
            var result = await _groupRepository.GetAllGroupsAsync();
            return Ok(new Notification
            {
                Success = true,
                Message = "Get All Successfully",
                Data = result
            });
        }

        // GET: api/Groups/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Group>> FindGroup(Guid id)
        {
            var result = await _groupRepository.FindGroupByIdAsync(id);
            if (result == null)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "This Group doesn't exist",
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

        // PUT: api/Groups/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGroup(Guid id, GroupDTO group)
        {
            if (_groupRepository.ValidateGroupDTO(group) == false)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "Please enter all information",
                    Data = null
                });
            }

            var oldGroup = await _groupRepository.FindGroupByIdAsync(id);
            if (oldGroup == null)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "This Group doesn't exist",
                    Data = null
                });
            }

            var account = await _accountRepository.FindAccountByIdAsync(Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            if (account == null)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "This Account doesn't exist or invalid",
                    Data = null
                });
            }
            if (account.Email != oldGroup.Creator)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "You don't have permission to update this group",
                    Data = null
                });
            }

            if (await _groupRepository.CheckNameForUpdate(group.Name!, oldGroup) == false)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "Group Name already exist",
                    Data = null
                });
            }

            var result = await _groupRepository.UpdateGroupAsync(oldGroup, group);
            return Ok(new Notification
            {
                Success = true,
                Message = "Update Successfully",
                Data = result
            });
        }

        //POST: api/Groups
        [HttpPost]
        public async Task<ActionResult<Group>> PostGroup(GroupDTO group)
        {
            if (_groupRepository.ValidateGroupDTO(group) == false)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "Please enter all information",
                    Data = null
                });
            }

            var account = await _accountRepository.FindAccountByIdAsync(Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            if (account == null)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "This Account doesn't exist or invalid",
                    Data = null
                });
            }

            if (await _groupRepository.CheckNameForInsert(group.Name!) == false)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "Group Name already exist",
                    Data = null
                });
            }

            var result = await _groupRepository.InsertGroupAsync(group, account.Email!);
            return Ok(new Notification
            {
                Success = true,
                Message = "Insert Successfully",
                Data = result
            });
        }

        // DELETE: api/Groups/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGroup(Guid id)
        {
            var result = await _groupRepository.FindGroupByIdAsync(id);
            if (result == null)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "This Group doesn't exist",
                    Data = null
                });
            }

            var account = await _accountRepository.FindAccountByIdAsync(Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            if (account == null)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "This Account doesn't exist or invalid",
                    Data = null
                });
            }
            if (account.Email != result.Creator)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "You don't have permission to delete this group",
                    Data = null
                });
            }
            var members = await _memberRepository.FindMemberByGroupAsync(id);
            if (members.Count > 0)
            {
                foreach (var item in members)
                {
                    await _memberRepository.DeleteMemberAsync(item);
                }
            }
            await _groupRepository.DeleteGroupAsync(result);
            return Ok(new Notification
            {
                Success = true,
                Message = "Delete Successfully",
                Data = null
            });
        }
    }
}
