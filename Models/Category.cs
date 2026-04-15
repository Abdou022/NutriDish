using System.ComponentModel.DataAnnotations;

public class Category
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Le nom de la catégorie est obligatoire")]
    [StringLength(50)]
    public string Name { get; set; }

    public ICollection<Recipe>? Recipes { get; set; }
}