using System.ComponentModel.DataAnnotations;

public class CuisineType
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Le type de cuisine est obligatoire")]
    [StringLength(50)]
    public string Name { get; set; }

    public ICollection<Recipe>? Recipes { get; set; }
}