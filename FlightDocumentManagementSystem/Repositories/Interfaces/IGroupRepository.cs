using FlightDocumentManagementSystem.Data;
using FlightDocumentManagementSystem.Models;

namespace FlightDocumentManagementSystem.Repositories.Interfaces
{
    public interface IGroupRepository : IRepository<Group>
    {
        public Task<List<Group>> GetAllGroupsAsync();
        public Task<PagingDTO<Group>> GetAllGroupsPagingAsync(int pageNumber, int pageSize);
        public Task<Group?> FindGroupByIdAsync(Guid id);
        public Task<Group> InsertGroupAsync(GroupDTO group, string creator);
        public Task<Group> UpdateGroupAsync(Group oldGroup, GroupDTO newGroup);
        public Task DeleteGroupAsync(Group group);
        public bool ValidateGroupDTO(GroupDTO group);
        public Task<bool> CheckNameForInsert(string name);
        public Task<bool> CheckNameForUpdate(string name, Group group);
    }
}
