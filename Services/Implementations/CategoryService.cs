using Microsoft.EntityFrameworkCore;
using NutriDish.Services;
using NutriDish.Models;

public class CategoryService : ICategoryService
{
    private readonly AppDbContext _context;

    public CategoryService(AppDbContext context)
    {
        _context = context;
    }

    // 🔹 Get All
    public async Task<List<Category>> GetAllCategoriesAsync()
    {
        return await _context.Categories
            .OrderBy(c => c.Name)
            .Include(c => c.Recipes)
            .ToListAsync();
    }

    // 🔹 Pagination
    public async Task<(List<Category> Categories, int TotalCount)> GetPagedCategoriesAsync(int pageNumber, int pageSize)
    {
        var query = _context.Categories.AsQueryable();

        var totalCount = await query.CountAsync();

        var categories = await query
            .OrderBy(c => c.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (categories, totalCount);
    }

    // 🔹 Get By Id
    public async Task<Category?> GetCategoryByIdAsync(int id)
    {
        return await _context.Categories
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    // 🔹 Add
    public async Task<Category> AddCategoryAsync(Category category)
    {
        var exists = await _context.Categories
            .AnyAsync(c => c.Name.ToLower() == category.Name.ToLower());

        if (exists)
            throw new Exception("Cette catégorie existe déjà");

        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        return category;
    }

    // 🔹 Update
    public async Task<Category?> UpdateCategoryAsync(Category category)
    {
        var existing = await _context.Categories.FindAsync(category.Id);

        if (existing == null)
            return null;

        var exists = await _context.Categories
            .AnyAsync(c => c.Name.ToLower() == category.Name.ToLower()
                        && c.Id != category.Id);

        if (exists)
            throw new Exception("Une catégorie avec ce nom existe déjà");

        existing.Name = category.Name;

        await _context.SaveChangesAsync();

        return existing;
    }

    // 🔹 Delete
    public async Task<bool> DeleteCategoryAsync(int id)
    {
        var category = await _context.Categories.FindAsync(id);

        if (category == null)
            return false;

        try
        {
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            throw new Exception("Impossible de supprimer : catégorie utilisée");
        }
    }

    // 🔹 Search
    public async Task<List<Category>> SearchCategoriesAsync(string keyword)
    {
        return await _context.Categories
            .Where(c => c.Name.ToLower().Contains(keyword.ToLower()))
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    
}