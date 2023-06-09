using FlightDocumentManagementSystem.Data;
using FlightDocumentManagementSystem.Helpers;
using FlightDocumentManagementSystem.Models;
using FlightDocumentManagementSystem.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FlightDocumentManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoriesController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            var result = await _categoryRepository.GetAllCategoriesAsync();
            return Ok(new Notification
            {
                Success = true,
                Message = "Get all successfully",
                Data = result
            });
        }

        // GET: api/Categories/Paging
        [HttpGet("Paging")]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategoriesPaging(int pageNumber, int pageSize)
        {
            var result = await _categoryRepository.GetAllCategoriesPagingAsync(pageNumber, pageSize);
            return Ok(new Notification
            {
                Success = true,
                Message = "Get all successfully",
                Data = result
            });
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> FindCategory(Guid id)
        {
            var result = await _categoryRepository.FindCategoryByIdAsync(id);
            if (result == null)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "This category doesn't exist",
                    Data = null
                });
            }
            return Ok(new Notification
            {
                Success = true,
                Message = "Find successfully",
                Data = result
            });
        }

        // PUT: api/Categories/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(Guid id, CategoryDTO newCategory)
        {
            if (string.IsNullOrEmpty(newCategory.Name))
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "Please enter all information",
                    Data = null
                });
            }
            var oldCategory = await _categoryRepository.FindCategoryByIdAsync(id);
            if (oldCategory == null)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "This category doesn't exist",
                    Data = null
                });
            }

            if (await _categoryRepository.CheckCategoryNameToUpdateAsync(newCategory.Name, oldCategory) == false)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "New category name already exist",
                    Data = null
                });
            }

            var result = await _categoryRepository.UpdateCategoryAsync(oldCategory, newCategory);
            return Ok(new Notification
            {
                Success = true,
                Message = "Update successfully",
                Data = result
            });
        }

        // POST: api/Categories
        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory(CategoryDTO category)
        {
            if (string.IsNullOrEmpty(category.Name))
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "Please enter all information",
                    Data = null
                });
            }

            if (await _categoryRepository.CheckCategoryNameToInsertAsync(category.Name) == false)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "New category name already exist",
                    Data = null
                });
            }

            var result = await _categoryRepository.InsertCategoryAsync(category);
            return Ok(new Notification
            {
                Success = true,
                Message = "Insert successfully",
                Data = result
            });
        }

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            var category = await _categoryRepository.FindCategoryByIdAsync(id);
            if (category == null)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "This category doesn't exist",
                    Data = null
                });
            }

            await _categoryRepository.DeleteCategoryAsync(category);
            return Ok(new Notification
            {
                Success = true,
                Message = "Delete successfully",
                Data = null
            });
        }
    }
}
