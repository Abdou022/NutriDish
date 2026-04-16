using System.ComponentModel.DataAnnotations;
namespace NutriDish.Models;


public class Unit
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Le nom de l'unité est obligatoire")]
    [StringLength(50)]
    public string Name { get; set; }

    [Required(ErrorMessage = "L'abréviation est obligatoire")]
    [StringLength(10)]
    public string Abbreviation { get; set; }

    public ICollection<Ingredient>? Ingredients { get; set; }
}