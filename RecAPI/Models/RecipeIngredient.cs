using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecAPI.Models
{
    public class RecipeIngredient
    {
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }
        public int IngredientId { get; set; }
        public Ingredient Ingredient { get; set; }
        public decimal Amount { get; set; }
        public Unit Unit { get; set; }
    }

    public enum Unit
    {
        Pcs,
        Tablespoons,
        Teaspoons,
        Grams,
        Deciliters,
        Liters,
        Cloves
    }

    public static class RecipeIngredientUtils
    {
        public static RecipeIngredientDTO ToDto(this RecipeIngredient recipeIngredient)
        {
            if (recipeIngredient == null)
                return null;

            RecipeIngredientDTO recipeIngredientDTO = new RecipeIngredientDTO
            {
                RecipeId = recipeIngredient.RecipeId,
                RecipeName = recipeIngredient.Recipe.Name,
                IngredientId = recipeIngredient.IngredientId,
                IngredientName = recipeIngredient.Ingredient.Name,
                Amount = recipeIngredient.Amount,
                Unit = recipeIngredient.Unit
            };

            return recipeIngredientDTO;
        }

        public static RecipeIngredient ToEntity(this RecipeIngredientDTO recipeIngredientDTO)
        {
            if (recipeIngredientDTO == null)
                return null;

            RecipeIngredient recipeIngredient = new RecipeIngredient
            {
                RecipeId = recipeIngredientDTO.RecipeId,
                IngredientId = recipeIngredientDTO.IngredientId,
                Amount = recipeIngredientDTO.Amount,
                Unit = recipeIngredientDTO.Unit
            };

            return recipeIngredient;
        }
    }
}
