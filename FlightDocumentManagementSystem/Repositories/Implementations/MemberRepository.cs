using FlightDocumentManagementSystem.Contexts;
using FlightDocumentManagementSystem.Data;
using FlightDocumentManagementSystem.Models;
using FlightDocumentManagementSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FlightDocumentManagementSystem.Repositories.Implementations
{
    public class MemberRepository : RepositoryBase<Member>, IMemberRepository
    {
        public MemberRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<bool> CheckMemberInGroupAsync(MemberDTO member)
        {
            var result = await _dbSet.FirstOrDefaultAsync(m => m.GroupId == member.GroupId && m.AccountId == member.AccountId);
            if (result != null)
            {
                return false;
            }
            return true;
        }

        public async Task DeleteMemberAsync(Member member)
        {
            await DeleteAsync(member);
        }

        public async Task<Member?> FindMemberByPermissionAsync(Permission permission, Account account)
        {
            var result = await _dbSet.FirstOrDefaultAsync(m => m.GroupId == permission.GroupId && m.AccountId == account.AccountId);
            return result;
        }

        public async Task<List<Member>> FindMemberByGroupAsync(Guid groupId)
        {
            var result = await _dbSet.Include(m => m.Group).Include(m => m.Account!.Role).Where(m => m.GroupId == groupId).ToListAsync();
            return result;
        }

        public async Task<Member?> FindMemberByIdAsync(Guid id)
        {
            var result = await FindByIdAsync(id);
            return result;
        }

        public async Task<List<Member>> GetAllMembersAsync()
        {
            var result = await GetAllWithIncludeAsync(m => m.Account!.Role!, m => m.Group!);
            return result;
        }

        public async Task<Member> InsertMemberAsync(MemberDTO member)
        {
            var newMember = new Member()
            {
                MemberId = Guid.NewGuid(),
                TimeCreate = DateTime.UtcNow,
                TimeUpdate = DateTime.UtcNow,
                AccountId = member.AccountId,
                GroupId = member.GroupId
            };
            await InsertAsync(newMember);
            return newMember;
        }

        public Task<Member> UpdateMemberAsync(Member member)
        {
            throw new NotImplementedException();
        }
    }
}
