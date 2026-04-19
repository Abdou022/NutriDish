using Microsoft.EntityFrameworkCore;
using NutriDish.Models;

namespace NutriDish.Services;

public class RecipeService : IRecipeService
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _env;

    public RecipeService(AppDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    public async Task<List<Recipe>> GetAllRecipesAsync()
    {
        return await _context.Recipes
            .Include(r => r.Category)
            .Include(r => r.CuisineType)
            .Include(r => r.RecipeIngredients!)
                .ThenInclude(ri => ri.Ingredient)
                    .ThenInclude(i => i.Unit)
            .OrderBy(r => r.Name)
            .ToListAsync();
    }

    public async Task<Recipe?> GetRecipeByIdAsync(int id)
    {
        return await _context.Recipes
            .Include(r => r.Category)
            .Include(r => r.CuisineType)
            .Include(r => r.RecipeIngredients!)
                .ThenInclude(ri => ri.Ingredient)
                    .ThenInclude(i => i.Unit)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<Recipe> AddRecipeAsync(Recipe recipe, List<RecipeIngredient> ingredients)
    {
        var exists = await _context.Recipes
            .AnyAsync(r => r.Name.ToLower() == recipe.Name.ToLower());

        if (exists)
            throw new Exception("Une recette avec ce nom existe déjà");

        _context.Recipes.Add(recipe);
        await _context.SaveChangesAsync();

        foreach (var ingredient in ingredients)
        {
            ingredient.RecipeId = recipe.Id;
            _context.RecipeIngredients.Add(ingredient);
        }

        await _context.SaveChangesAsync();
        return recipe;
    }

    public async Task<Recipe?> UpdateRecipeAsync(Recipe recipe, List<RecipeIngredient> ingredients)
    {
        var existing = await _context.Recipes
            .Include(r => r.RecipeIngredients)
            .FirstOrDefaultAsync(r => r.Id == recipe.Id);

        if (existing == null)
            return null;

        var exists = await _context.Recipes
            .AnyAsync(r => r.Name.ToLower() == recipe.Name.ToLower()
                        && r.Id != recipe.Id);

        if (exists)
            throw new Exception("Une recette avec ce nom existe déjà");

        existing.Name = recipe.Name;
        existing.NumberOfPersons = recipe.NumberOfPersons;
        existing.CookingMethod = recipe.CookingMethod;
        existing.CuisineTypeId = recipe.CuisineTypeId;
        existing.CategoryId = recipe.CategoryId;

        if (recipe.ImageUrl != null)
            existing.ImageUrl = recipe.ImageUrl;

        // Replace ingredients
        _context.RecipeIngredients.RemoveRange(existing.RecipeIngredients!);

        foreach (var ingredient in ingredients)
        {
            ingredient.RecipeId = existing.Id;
            _context.RecipeIngredients.Add(ingredient);
        }

        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteRecipeAsync(int id)
    {
        var recipe = await _context.Recipes
            .Include(r => r.RecipeIngredients)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (recipe == null)
            return false;

        try
        {
            _context.RecipeIngredients.RemoveRange(recipe.RecipeIngredients!);
            _context.Recipes.Remove(recipe);
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            throw new Exception("Impossible de supprimer cette recette");
        }
    }

    public async Task<List<Recipe>> GetRecipesByCategoryAsync(int categoryId)
{
    return await _context.Recipes
        .Where(r => r.CategoryId == categoryId)
        .Include(r => r.Category)
        .Include(r => r.CuisineType)
        .Include(r => r.RecipeIngredients!)
            .ThenInclude(ri => ri.Ingredient)
        .ToListAsync();
}

public async Task<List<Recipe>> GetRecipesByCuisineTypeAsync(int cuisineTypeId)
{
    return await _context.Recipes
        .Where(r => r.CuisineTypeId == cuisineTypeId)
        .Include(r => r.Category)
        .Include(r => r.CuisineType)
        .Include(r => r.RecipeIngredients!)
            .ThenInclude(ri => ri.Ingredient)
        .ToListAsync();
}
}