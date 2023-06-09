using FlightDocumentManagementSystem.Contexts;
using FlightDocumentManagementSystem.Data;
using FlightDocumentManagementSystem.Models;
using FlightDocumentManagementSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FlightDocumentManagementSystem.Repositories.Implementations
{
    public class RoleRepository : RepositoryBase<Role>, IRoleRepository
    {
        public RoleRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<bool> CheckIsExistByName(string name)
        {
            var result = await _dbSet.FirstOrDefaultAsync(r => r.Name == name);
            if (result == null)
            {
                return false;
            }
            return true;
        }

        public async Task DeleteRoleAsync(Role role)
        {
            await DeleteAsync(role);
        }

        public async Task<Role?> FindRoleByIdAsync(Guid id)
        {
            var result = await FindByIdAsync(id);
            return result;
        }

        public async Task<Role?> FindRoleByNameAsync(string name)
        {
            var result = await _dbSet.FirstOrDefaultAsync(r => r.Name == name);
            return result;
        }

        public async Task<List<Role>> GetAllRolesAsync()
        {
            return await GetAllAsync();
        }

        public async Task<Role> InsertRoleAsync(RoleDTO role)
        {
            var newRole = new Role()
            {
                RoleId = Guid.NewGuid(),
                Name = role.Name,
                TimeCreate = DateTime.UtcNow,
                TimeUpdate = DateTime.UtcNow
            };
            await InsertAsync(newRole);
            return newRole;
        }

        public async Task<Role> UpdateRoleAsync(Role oldRole, RoleDTO newRole)
        {
            oldRole.Name = newRole.Name;
            oldRole.TimeUpdate = DateTime.UtcNow;
            await UpdateAsync(oldRole);
            return oldRole;
        }
    }
}
