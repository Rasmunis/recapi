using Microsoft.EntityFrameworkCore;

namespace RecAPI.Models
{
	public class RecAPIContext : DbContext
	{
		public RecAPIContext(DbContextOptions<RecAPIContext> options)
			: base(options)
		{
		}

		public DbSet<Recipe> Recipes { get; set; }
		public DbSet<Ingredient> Ingredients { get; set; }
		public DbSet<RecipeIngredient> RecipeIngredients { get; set; }

		// https://docs.microsoft.com/en-us/ef/core/modeling/relationships#many-to-many
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<RecipeIngredient>()
				.HasKey(ri => new { ri.RecipeId, ri.IngredientId });

			modelBuilder.Entity<RecipeIngredient>()
				.HasOne(ri => ri.Recipe)
				.WithMany(r => r.RecipeIngredients)
				.HasForeignKey(ri => ri.RecipeId);

			modelBuilder.Entity<RecipeIngredient>()
				.HasOne(ri => ri.Ingredient)
				.WithMany(i => i.RecipeIngredients)
				.HasForeignKey(ri => ri.IngredientId);
		}
	}
}
