using NutriDish.Models;

namespace NutriDish.Services;

public interface IIngredientService
{
    Task<List<Ingredient>> GetAllIngredientsAsync();
    Task<Ingredient?> GetIngredientByIdAsync(int id);
    Task<Ingredient> AddIngredientAsync(Ingredient ingredient);
    Task<Ingredient?> UpdateIngredientAsync(Ingredient ingredient);
    Task<bool> DeleteIngredientAsync(int id);
    Task<List<Ingredient>> SearchIngredientsAsync(string keyword);
}