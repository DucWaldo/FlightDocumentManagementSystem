using FlightDocumentManagementSystem.Data;
using FlightDocumentManagementSystem.Models;

namespace FlightDocumentManagementSystem.Repositories.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        public Task<List<Category>> GetAllCategoriesAsync();
        public Task<PagingDTO<Category>> GetAllCategoriesPagingAsync(int pageNumber, int pageSize);
        public Task<Category?> FindCategoryByIdAsync(Guid id);
        public Task<Category> InsertCategoryAsync(CategoryDTO category);
        public Task<Category> UpdateCategoryAsync(Category oldCategory, CategoryDTO newCategory);
        public Task DeleteCategoryAsync(Category category);
        public Task<bool> CheckCategoryNameToInsertAsync(string name);
        public Task<bool> CheckCategoryNameToUpdateAsync(string name, Category category);
    }
}
