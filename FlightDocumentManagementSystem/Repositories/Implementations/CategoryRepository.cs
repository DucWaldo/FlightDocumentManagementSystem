using FlightDocumentManagementSystem.Contexts;
using FlightDocumentManagementSystem.Data;
using FlightDocumentManagementSystem.Models;
using FlightDocumentManagementSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FlightDocumentManagementSystem.Repositories.Implementations
{
    public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<bool> CheckCategoryNameToInsertAsync(string name)
        {
            var result = await _dbSet.FirstOrDefaultAsync(c => c.Name == name);
            if (result != null)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> CheckCategoryNameToUpdateAsync(string name, Category category)
        {
            var result = await _dbSet.FirstOrDefaultAsync(c => c.Name == name && c.CategoryId != category.CategoryId);
            if (result != null)
            {
                return false;
            }
            return true;
        }

        public async Task DeleteCategoryAsync(Category category)
        {
            await DeleteAsync(category);
        }

        public async Task<Category?> FindCategoryByIdAsync(Guid id)
        {
            var result = await FindByIdAsync(id);
            return result;
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            var result = await GetAllAsync();
            return result;
        }

        public async Task<Category> InsertCategoryAsync(CategoryDTO category)
        {
            var newCategory = new Category()
            {
                CategoryId = Guid.NewGuid(),
                Name = category.Name,
                TimeCreate = DateTime.UtcNow,
                TimeUpdate = DateTime.UtcNow
            };
            await InsertAsync(newCategory);
            return newCategory;
        }

        public async Task<Category> UpdateCategoryAsync(Category oldCategory, CategoryDTO newCategory)
        {
            oldCategory.Name = newCategory.Name;
            oldCategory.TimeUpdate = DateTime.UtcNow;

            await UpdateAsync(oldCategory);
            return oldCategory;
        }
    }
}
