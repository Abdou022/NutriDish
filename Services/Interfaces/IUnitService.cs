using NutriDish.Models;

namespace NutriDish.Services;

public interface IUnitService
{
    Task<List<Unit>> GetAllUnitsAsync();
    Task<Unit?> GetUnitByIdAsync(int id);
    Task<Unit> AddUnitAsync(Unit unit);
    Task<Unit?> UpdateUnitAsync(Unit unit);
    Task<bool> DeleteUnitAsync(int id);
    Task<List<Unit>> SearchUnitsAsync(string keyword);
}