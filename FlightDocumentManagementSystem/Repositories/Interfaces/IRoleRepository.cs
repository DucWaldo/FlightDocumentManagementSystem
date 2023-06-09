using FlightDocumentManagementSystem.Data;
using FlightDocumentManagementSystem.Models;

namespace FlightDocumentManagementSystem.Repositories.Interfaces
{
    public interface IRoleRepository : IRepository<Role>
    {
        public Task<List<Role>> GetAllRolesAsync();
        public Task<Role?> FindRoleByIdAsync(Guid id);
        public Task<Role?> FindRoleByNameAsync(string name);
        public Task<Role> InsertRoleAsync(RoleDTO role);
        public Task<Role> UpdateRoleAsync(Role oldRole, RoleDTO newRole);
        public Task DeleteRoleAsync(Role role);
        public Task<bool> CheckIsExistByName(string name);
    }
}
