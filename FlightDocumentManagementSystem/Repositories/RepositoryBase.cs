using FlightDocumentManagementSystem.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FlightDocumentManagementSystem.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<List<TEntity>> GetAllAsync();
        Task<List<TEntity>> GetAllWithIncludeAsync(params Expression<Func<TEntity, object>>[] includeExpressions);
        Task InsertAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
        Task RemoveAsync(List<TEntity> entity);
        Task<TEntity?> FindByIdAsync(Guid id);
    }

    public class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly ApplicationDbContext _context;
        public readonly DbSet<TEntity> _dbSet;
        public RepositoryBase(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public async Task DeleteAsync(TEntity entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<List<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<TEntity?> FindByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task InsertAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TEntity entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<List<TEntity>> GetAllWithIncludeAsync(params Expression<Func<TEntity, object>>[] includeExpressions)
        {
            var query = _dbSet.AsQueryable();

            if (includeExpressions != null)
            {
                query = includeExpressions.Aggregate(query, (current, includeExpressions) => current.Include(includeExpressions));
            }

            return await query.ToListAsync();
        }

        public async Task RemoveAsync(List<TEntity> entity)
        {
            _dbSet.RemoveRange(entity);
            await _context.SaveChangesAsync();
        }
    }
}
