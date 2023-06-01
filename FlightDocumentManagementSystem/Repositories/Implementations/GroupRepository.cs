using FlightDocumentManagementSystem.Contexts;
using FlightDocumentManagementSystem.Data;
using FlightDocumentManagementSystem.Models;
using FlightDocumentManagementSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FlightDocumentManagementSystem.Repositories.Implementations
{
    public class GroupRepository : RepositoryBase<Group>, IGroupRepository
    {
        public GroupRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<bool> CheckNameForInsert(string name)
        {
            var result = await _dbSet.FirstOrDefaultAsync(g => g.Name == name);
            if (result != null)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> CheckNameForUpdate(string name, Group group)
        {
            var result = await _dbSet.FirstOrDefaultAsync(g => g.Name == name && g.GroupId != group.GroupId);
            if (result != null)
            {
                return false;
            }
            return true;
        }

        public async Task DeleteGroupAsync(Group group)
        {
            await DeleteAsync(group);
        }

        public async Task<Group?> FindGroupByIdAsync(Guid id)
        {
            var result = await FindByIdAsync(id);
            return result;
        }

        public async Task<List<Group>> GetAllGroupsAsync()
        {
            var result = await GetAllAsync();
            return result;
        }

        public async Task<Group> InsertGroupAsync(GroupDTO group, string creator)
        {
            var newGroup = new Group()
            {
                GroupId = Guid.NewGuid(),
                Name = group.Name,
                Note = group.Note,
                Creator = creator,
                TimeCreate = DateTime.UtcNow,
                TimeUpdate = DateTime.UtcNow
            };
            await InsertAsync(newGroup);
            return newGroup;
        }

        public async Task<Group> UpdateGroupAsync(Group oldGroup, GroupDTO newGroup)
        {
            oldGroup.Name = newGroup.Name;
            oldGroup.Note = newGroup.Note;
            oldGroup.TimeUpdate = DateTime.UtcNow;
            await UpdateAsync(oldGroup);
            return oldGroup;
        }

        public bool ValidateGroupDTO(GroupDTO group)
        {
            if (string.IsNullOrEmpty(group.Name))
            {
                return false;
            }

            if (string.IsNullOrEmpty(group.Note))
            {
                return false;
            }

            return true;
        }
    }
}
