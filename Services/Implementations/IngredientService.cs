using Microsoft.EntityFrameworkCore;
using NutriDish.Models;

namespace NutriDish.Services;

public class IngredientService : IIngredientService
{
    private readonly AppDbContext _context;

    public IngredientService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Ingredient>> GetAllIngredientsAsync()
    {
        return await _context.Ingredients
            .Include(i => i.Unit)
            .OrderBy(i => i.Name)
            .ToListAsync();
    }

    public async Task<Ingredient?> GetIngredientByIdAsync(int id)
    {
        return await _context.Ingredients
            .Include(i => i.Unit)
            .FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<Ingredient> AddIngredientAsync(Ingredient ingredient)
    {
        var exists = await _context.Ingredients
            .AnyAsync(i => i.Name.ToLower() == ingredient.Name.ToLower());

        if (exists)
            throw new Exception("Un ingrédient avec ce nom existe déjà");

        _context.Ingredients.Add(ingredient);
        await _context.SaveChangesAsync();

        return ingredient;
    }

    public async Task<Ingredient?> UpdateIngredientAsync(Ingredient ingredient)
    {
        var existing = await _context.Ingredients.FindAsync(ingredient.Id);

        if (existing == null)
            return null;

        var exists = await _context.Ingredients
            .AnyAsync(i => i.Name.ToLower() == ingredient.Name.ToLower()
                        && i.Id != ingredient.Id);

        if (exists)
            throw new Exception("Un ingrédient avec ce nom existe déjà");

        existing.Name = ingredient.Name;
        existing.CaloriesPerUnit = ingredient.CaloriesPerUnit;
        existing.Proteins = ingredient.Proteins;
        existing.Carbs = ingredient.Carbs;
        existing.Fats = ingredient.Fats;
        existing.UnitId = ingredient.UnitId;

        await _context.SaveChangesAsync();

        return existing;
    }

    public async Task<bool> DeleteIngredientAsync(int id)
    {
        var ingredient = await _context.Ingredients.FindAsync(id);

        if (ingredient == null)
            return false;

        try
        {
            _context.Ingredients.Remove(ingredient);
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            throw new Exception("Impossible de supprimer : ingrédient utilisé dans une recette");
        }
    }

    public async Task<List<Ingredient>> SearchIngredientsAsync(string keyword)
    {
        return await _context.Ingredients
            .Include(i => i.Unit)
            .Where(i => i.Name.ToLower().Contains(keyword.ToLower()))
            .OrderBy(i => i.Name)
            .ToListAsync();
    }
}