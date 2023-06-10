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
    [Authorize(Policy = "AdminOrStaffPolicy")]
    public class MembersController : ControllerBase
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IGroupRepository _groupRepository;

        public MembersController(IMemberRepository memberRepository, IAccountRepository accountRepository, IGroupRepository groupRepository)
        {
            _memberRepository = memberRepository;
            _accountRepository = accountRepository;
            _groupRepository = groupRepository;
        }

        // GET: api/Members
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Member>>> GetMembers()
        {
            var result = await _memberRepository.GetAllMembersAsync();
            return Ok(new Notification
            {
                Success = true,
                Message = "Get All Successfully",
                Data = result
            });
        }

        // GET: api/Members/GetInGroup/5
        [HttpGet("GetInGroup/{groupId}")]
        public async Task<ActionResult<IEnumerable<Member>>> GetMembersInGroup(Guid groupId)
        {
            var result = await _memberRepository.FindMemberByGroupAsync(groupId);
            if (result.Count < 1)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "There are no members in this group",
                    Data = null
                });
            }
            return Ok(new Notification
            {
                Success = true,
                Message = "Get all members in this group Successfully",
                Data = result
            });
        }

        // POST: api/Members
        [HttpPost]
        public async Task<ActionResult<Member>> PostMember([FromForm] MemberDTO member)
        {
            if (await _memberRepository.CheckMemberInGroupAsync(member) == false)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "This Account already exist in this group",
                    Data = null
                });
            }
            if (await _accountRepository.FindAccountByIdAsync(member.AccountId) == null)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "This Account doesn't exist",
                    Data = null
                });
            }
            if (await _groupRepository.FindGroupByIdAsync(member.GroupId) == null)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "This Group doesn't exist",
                    Data = null
                });
            }
            var result = await _memberRepository.InsertMemberAsync(member);
            return Ok(new Notification
            {
                Success = true,
                Message = "Insert Successfully",
                Data = result
            });
        }

        // POST: api/Members/ListMember/5
        [HttpPost("ListMember/{groupId}")]
        public async Task<ActionResult<Member>> PostMembers([FromForm] List<Guid> accountId, Guid groupId)
        {
            List<Member> ListMember = new List<Member>();
            int count = 0;

            if (accountId.Count < 1)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "There are no member to insert",
                    Data = null
                });
            }

            if (await _groupRepository.FindGroupByIdAsync(groupId) == null)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "This Group doesn't exist",
                    Data = null
                });
            }

            foreach (var item in accountId)
            {
                var account = await _accountRepository.FindAccountByIdAsync(item);
                if (account == null)
                {
                    return Ok(new Notification
                    {
                        Success = false,
                        Message = "A Account in list doesn't exist",
                        Data = null
                    });
                }
                if (await _memberRepository.CheckMemberInGroupAsync(new MemberDTO() { AccountId = item, GroupId = groupId }) == true)
                {
                    var result = await _memberRepository.InsertMemberAsync(new MemberDTO() { AccountId = item, GroupId = groupId });
                    ListMember.Add(result);
                }
                else
                {
                    count++;
                }
            }

            if (count != 0)
            {
                return Ok(new Notification
                {
                    Success = true,
                    Message = $"Insert Successfully, but {count} members already exist in the group",
                    Data = ListMember
                });
            }

            return Ok(new Notification
            {
                Success = true,
                Message = "Insert All List Successfully",
                Data = ListMember
            });
        }

        // DELETE: api/Member/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMember(Guid id)
        {
            var member = await _memberRepository.FindMemberByIdAsync(id);
            if (member == null)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "This Member doesn't exist",
                    Data = null
                });
            }
            await _memberRepository.DeleteMemberAsync(member);
            return Ok(new Notification
            {
                Success = true,
                Message = "Delete Successfully",
                Data = null
            });
        }
    }
}
