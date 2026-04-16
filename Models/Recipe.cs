using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace NutriDish.Models;


public class Recipe
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Le nom de la recette est obligatoire")]
    [StringLength(150)]
    public string Name { get; set; }

    [Range(1, 20, ErrorMessage = "Nombre de personnes invalide")]
    public int NumberOfPersons { get; set; }

    [Required(ErrorMessage = "La méthode de préparation est obligatoire")]
    [StringLength(2000)]
    public string CookingMethod { get; set; }

    public string? ImageUrl { get; set; } // optionnel

    // Relations
    [Required]
    public int CuisineTypeId { get; set; }

    [ForeignKey("CuisineTypeId")]
    public CuisineType CuisineType { get; set; }

    [Required]
    public int CategoryId { get; set; }

    [ForeignKey("CategoryId")]
    public Category Category { get; set; }

    public ICollection<RecipeIngredient>? RecipeIngredients { get; set; }
}