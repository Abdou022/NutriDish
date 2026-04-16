using Microsoft.EntityFrameworkCore;
namespace NutriDish.Models;


public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    // DbSets
    public DbSet<Ingredient> Ingredients { get; set; }
    public DbSet<Unit> Units { get; set; }
    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<CuisineType> CuisineTypes { get; set; }
    public DbSet<RecipeIngredient> RecipeIngredients { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 🔗 Clé composite pour RecipeIngredient
        modelBuilder.Entity<RecipeIngredient>()
            .HasKey(ri => new { ri.RecipeId, ri.IngredientId });

        // 🔗 Relation RecipeIngredient → Recipe
        modelBuilder.Entity<RecipeIngredient>()
            .HasOne(ri => ri.Recipe)
            .WithMany(r => r.RecipeIngredients)
            .HasForeignKey(ri => ri.RecipeId)
            .OnDelete(DeleteBehavior.Cascade);

        // 🔗 Relation RecipeIngredient → Ingredient
        modelBuilder.Entity<RecipeIngredient>()
            .HasOne(ri => ri.Ingredient)
            .WithMany(i => i.RecipeIngredients)
            .HasForeignKey(ri => ri.IngredientId)
            .OnDelete(DeleteBehavior.Restrict);

        // 🔗 Ingredient → Unit
        modelBuilder.Entity<Ingredient>()
            .HasOne(i => i.Unit)
            .WithMany(u => u.Ingredients)
            .HasForeignKey(i => i.UnitId)
            .OnDelete(DeleteBehavior.Restrict);

        // 🔗 Recipe → Category
        modelBuilder.Entity<Recipe>()
            .HasOne(r => r.Category)
            .WithMany(c => c.Recipes)
            .HasForeignKey(r => r.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        // 🔗 Recipe → CuisineType
        modelBuilder.Entity<Recipe>()
            .HasOne(r => r.CuisineType)
            .WithMany(ct => ct.Recipes)
            .HasForeignKey(r => r.CuisineTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        // 🔒 Contraintes supplémentaires

        modelBuilder.Entity<Unit>()
            .HasIndex(u => u.Name)
            .IsUnique();

        modelBuilder.Entity<Category>()
            .HasIndex(c => c.Name)
            .IsUnique();

        modelBuilder.Entity<CuisineType>()
            .HasIndex(ct => ct.Name)
            .IsUnique();

        modelBuilder.Entity<Ingredient>()
            .HasIndex(i => i.Name)
            .IsUnique();
    }
}