using NutriDish.Models;

namespace NutriDish.Services;

public interface IRecipeService
{
    Task<List<Recipe>> GetAllRecipesAsync();
    Task<Recipe?> GetRecipeByIdAsync(int id);
    Task<Recipe> AddRecipeAsync(Recipe recipe, List<RecipeIngredient> ingredients);
    Task<Recipe?> UpdateRecipeAsync(Recipe recipe, List<RecipeIngredient> ingredients);
    Task<bool> DeleteRecipeAsync(int id);
    Task<List<Recipe>> GetRecipesByCategoryAsync(int categoryId);
    Task<List<Recipe>> GetRecipesByCuisineTypeAsync(int cuisineTypeId);

}