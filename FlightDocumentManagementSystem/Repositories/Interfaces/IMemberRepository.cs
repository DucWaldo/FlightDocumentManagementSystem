using FlightDocumentManagementSystem.Data;
using FlightDocumentManagementSystem.Models;

namespace FlightDocumentManagementSystem.Repositories.Interfaces
{
    public interface IMemberRepository : IRepository<Member>
    {
        public Task<List<Member>> GetAllMembersAsync();
        public Task<Member?> FindMemberByIdAsync(Guid id);
        public Task<List<Member>> FindMemberByGroupAsync(Guid groupId);
        public Task<Member?> FindMemberByPermissionAsync(Permission permission, Account account);
        public Task<Member> InsertMemberAsync(MemberDTO member);
        public Task<Member> UpdateMemberAsync(Member member);
        public Task DeleteMemberAsync(Member member);
        public Task<bool> CheckMemberInGroupAsync(MemberDTO member);
    }
}
