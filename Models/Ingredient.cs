using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Ingredient
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Le nom est obligatoire")]
    [StringLength(100)]
    public string Name { get; set; }

    [Range(0, 1000, ErrorMessage = "Calories invalides")]
    public double CaloriesPerUnit { get; set; }

    [Range(0, 100, ErrorMessage = "Protéines invalides")]
    public double Proteins { get; set; }

    [Range(0, 100, ErrorMessage = "Glucides invalides")]
    public double Carbs { get; set; }

    [Range(0, 100, ErrorMessage = "Lipides invalides")]
    public double Fats { get; set; }

    // Foreign Key
    [Required]
    public int UnitId { get; set; }

    [ForeignKey("UnitId")]
    public Unit Unit { get; set; }

    public ICollection<RecipeIngredient>? RecipeIngredients { get; set; }
}