using CategoriesMVC.Models;

namespace CategoriesMVC.Services;

public interface ICategoryService
{
    Task<IEnumerable<CategoryViewModel>> GetCategories();
    Task<CategoryViewModel> GetCategoryById(int id);
    Task<CategoryViewModel> CreateCategory(CategoryViewModel categoryViewModel);
    Task<bool> UpdateCategory(int id, CategoryViewModel categoryViewModel);
    Task<bool> DeleteCategory(int id);
}
