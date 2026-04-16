using System.ComponentModel.DataAnnotations;
namespace NutriDish.Models;


public class Category
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Le nom de la catégorie est obligatoire")]
    [StringLength(50)]
    public string Name { get; set; }

    public ICollection<Recipe>? Recipes { get; set; }
}