using Microsoft.EntityFrameworkCore;
using NutriDish.Models;

namespace NutriDish.Services;

public class CuisineTypeService : ICuisineTypeService
{
    private readonly AppDbContext _context;

    public CuisineTypeService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<CuisineType>> GetAllCuisineTypesAsync()
    {
        return await _context.CuisineTypes
            .OrderBy(c => c.Name)
            .Include(c => c.Recipes) // populate pour le datagrid
            .ToListAsync();
    }

    public async Task<CuisineType?> GetCuisineTypeByIdAsync(int id)
    {
        return await _context.CuisineTypes
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<CuisineType> AddCuisineTypeAsync(CuisineType cuisineType)
    {
        var exists = await _context.CuisineTypes
            .AnyAsync(c => c.Name.ToLower() == cuisineType.Name.ToLower());

        if (exists)
            throw new Exception("Ce type de cuisine existe déjà");

        _context.CuisineTypes.Add(cuisineType);
        await _context.SaveChangesAsync();

        return cuisineType;
    }

    public async Task<CuisineType?> UpdateCuisineTypeAsync(CuisineType cuisineType)
    {
        var existing = await _context.CuisineTypes.FindAsync(cuisineType.Id);

        if (existing == null)
            return null;

        var exists = await _context.CuisineTypes
            .AnyAsync(c => c.Name.ToLower() == cuisineType.Name.ToLower()
                        && c.Id != cuisineType.Id);

        if (exists)
            throw new Exception("Un type de cuisine avec ce nom existe déjà");

        existing.Name = cuisineType.Name;
        await _context.SaveChangesAsync();

        return existing;
    }

    public async Task<bool> DeleteCuisineTypeAsync(int id)
    {
        var cuisineType = await _context.CuisineTypes.FindAsync(id);

        if (cuisineType == null)
            return false;

        try
        {
            _context.CuisineTypes.Remove(cuisineType);
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            throw new Exception("Impossible de supprimer : type de cuisine utilisé");
        }
    }

    public async Task<List<CuisineType>> SearchCuisineTypesAsync(string keyword)
    {
        return await _context.CuisineTypes
            .Where(c => c.Name.ToLower().Contains(keyword.ToLower()))
            .OrderBy(c => c.Name)
            .ToListAsync();
    }
}