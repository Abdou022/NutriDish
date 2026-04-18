using Microsoft.EntityFrameworkCore;
using NutriDish.Models;

namespace NutriDish.Services;

public class UnitService : IUnitService
{
    private readonly AppDbContext _context;

    public UnitService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Unit>> GetAllUnitsAsync()
    {
        return await _context.Units
            .OrderBy(u => u.Name)
            .ToListAsync();
    }

    public async Task<Unit?> GetUnitByIdAsync(int id)
    {
        return await _context.Units
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<Unit> AddUnitAsync(Unit unit)
    {
        var exists = await _context.Units
            .AnyAsync(u => u.Name.ToLower() == unit.Name.ToLower()
                        || u.Abbreviation.ToLower() == unit.Abbreviation.ToLower());

        if (exists)
            throw new Exception("Une unité avec ce nom ou cette abréviation existe déjà");

        _context.Units.Add(unit);
        await _context.SaveChangesAsync();

        return unit;
    }

    public async Task<Unit?> UpdateUnitAsync(Unit unit)
    {
        var existing = await _context.Units.FindAsync(unit.Id);

        if (existing == null)
            return null;

        var exists = await _context.Units
            .AnyAsync(u => (u.Name.ToLower() == unit.Name.ToLower()
                        || u.Abbreviation.ToLower() == unit.Abbreviation.ToLower())
                        && u.Id != unit.Id);

        if (exists)
            throw new Exception("Une unité avec ce nom ou cette abréviation existe déjà");

        existing.Name = unit.Name;
        existing.Abbreviation = unit.Abbreviation;
        await _context.SaveChangesAsync();

        return existing;
    }

    public async Task<bool> DeleteUnitAsync(int id)
    {
        var unit = await _context.Units.FindAsync(id);

        if (unit == null)
            return false;

        try
        {
            _context.Units.Remove(unit);
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            throw new Exception("Impossible de supprimer : unité utilisée");
        }
    }

    public async Task<List<Unit>> SearchUnitsAsync(string keyword)
    {
        return await _context.Units
            .Where(u => u.Name.ToLower().Contains(keyword.ToLower())
                     || u.Abbreviation.ToLower().Contains(keyword.ToLower()))
            .OrderBy(u => u.Name)
            .ToListAsync();
    }
}