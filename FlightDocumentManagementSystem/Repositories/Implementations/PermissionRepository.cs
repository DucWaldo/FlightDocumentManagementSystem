using FlightDocumentManagementSystem.Contexts;
using FlightDocumentManagementSystem.Data;
using FlightDocumentManagementSystem.Models;
using FlightDocumentManagementSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FlightDocumentManagementSystem.Repositories.Implementations
{
    public class PermissionRepository : RepositoryBase<Permission>, IPermissionRepository
    {
        public PermissionRepository(ApplicationDbContext context) : base(context)
        {
        }

        public bool CheckFunction(string function)
        {
            string function1 = "Read and modify";
            string function2 = "Read only";
            string function3 = "No Permission";

            if (function1.Equals(function) || function2.Equals(function) || function3.Equals(function))
            {
                return true;
            }
            return false;
        }

        public async Task<bool> CheckGroupInCategory(Guid groupId, Guid categoryId)
        {
            var result = await _dbSet.FirstOrDefaultAsync(p => p.GroupId == groupId && p.CategoryId == categoryId);
            if (result == null)
            {
                return true;
            }
            return false;
        }

        public async Task DeletePermissionAsync(Permission permission)
        {
            await DeleteAsync(permission);
        }

        public async Task<Permission?> FindPermissionByIdAsync(Guid id)
        {
            var result = await FindByIdAsync(id);
            if (result != null)
            {
                await _dbSet.Entry(result).Reference(p => p.Group).LoadAsync();
                await _dbSet.Entry(result).Reference(p => p.Category).LoadAsync();
            }
            return result;
        }

        public async Task<List<Permission>> GetAllPermissionsAsync()
        {
            var result = await GetAllWithIncludeAsync(p => p.Group!, p => p.Category!);
            return result;
        }

        public async Task<List<Permission>> GetPermissionByCategoryAsync(Guid categoryId)
        {
            var result = await _dbSet.Include(p => p.Group).Include(p => p.Category).Where(p => p.CategoryId == categoryId).ToListAsync();
            return result;
        }

        public async Task<Permission> InsertPermissionAsync(PermissionDTO permission)
        {
            var newPermission = new Permission()
            {
                PermissionId = Guid.NewGuid(),
                Function = permission.Function,
                TimeCreate = DateTime.UtcNow,
                TimeUpdate = DateTime.UtcNow,
                GroupId = permission.GroupId,
                CategoryId = permission.CategoryId
            };
            await InsertAsync(newPermission);
            return newPermission;
        }

        public async Task<Permission> UpdatePermissionAsync(Permission oldPermission, string function)
        {
            oldPermission.Function = function;
            await UpdateAsync(oldPermission);
            return oldPermission;
        }
    }
}
