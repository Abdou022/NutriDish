using NutriDish.Models;

namespace NutriDish.Services;

public interface ICuisineTypeService
{
    Task<List<CuisineType>> GetAllCuisineTypesAsync();
    Task<CuisineType?> GetCuisineTypeByIdAsync(int id);
    Task<CuisineType> AddCuisineTypeAsync(CuisineType cuisineType);
    Task<CuisineType?> UpdateCuisineTypeAsync(CuisineType cuisineType);
    Task<bool> DeleteCuisineTypeAsync(int id);
    Task<List<CuisineType>> SearchCuisineTypesAsync(string keyword);
}