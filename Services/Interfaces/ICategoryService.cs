using NutriDish.Models;

namespace NutriDish.Services;

public interface ICategoryService
{
    Task<List<Category>> GetAllCategoriesAsync();

    // Task<(List<Category> Categories, int TotalCount)> GetPagedCategoriesAsync(int pageNumber, int pageSize);

    Task<Category?> GetCategoryByIdAsync(int id);

    Task<Category> AddCategoryAsync(Category category);

    Task<Category?> UpdateCategoryAsync(Category category);

    Task<bool> DeleteCategoryAsync(int id);

    Task<List<Category>> SearchCategoriesAsync(string keyword);


}