using FlightDocumentManagementSystem.Data;
using FlightDocumentManagementSystem.Models;

namespace FlightDocumentManagementSystem.Repositories.Interfaces
{
    public interface IPermissionRepository : IRepository<Permission>
    {
        public Task<List<Permission>> GetAllPermissionsAsync();
        public Task<List<Permission>> GetPermissionByCategoryAsync(Guid categoryId);
        public Task<Permission?> FindPermissionByIdAsync(Guid id);
        public Task<Permission> InsertPermissionAsync(PermissionDTO permission);
        public Task<Permission> UpdatePermissionAsync(Permission oldPermission, string function);
        public Task DeletePermissionAsync(Permission permission);
        public bool CheckFunction(string function);
        public Task<bool> CheckGroupInCategory(Guid groupId, Guid categoryId);
    }
}
