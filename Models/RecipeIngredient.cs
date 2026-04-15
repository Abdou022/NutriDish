using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class RecipeIngredient
{
    [Required]
    public int RecipeId { get; set; }

    [ForeignKey("RecipeId")]
    public Recipe Recipe { get; set; }

    [Required]
    public int IngredientId { get; set; }

    [ForeignKey("IngredientId")]
    public Ingredient Ingredient { get; set; }

    [Range(0.1, 10000, ErrorMessage = "Quantité invalide")]
    public double Quantity { get; set; }
}