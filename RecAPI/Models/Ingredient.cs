using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RecAPI.Models
{
    public class Ingredient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<RecipeIngredient> RecipeIngredients { get; set; }
    }

    public static class IngredientUtils
    {
        public static IngredientDTO ToDto(this Ingredient ingredient)
        {
            if (ingredient == null)
                return null;
            IngredientDTO ingredientDTO = new IngredientDTO
            {
                Id = ingredient.Id,
                Name = ingredient.Name,
                RecipeIngredients = new List<RecipeIngredientDTO>()
            };

            ingredient.RecipeIngredients?.ForEach(recipeIngredient =>
            {
                ingredientDTO.RecipeIngredients.Add(recipeIngredient.ToDto());
            });

            return ingredientDTO;
        }

        public static Ingredient ToEntity(this IngredientDTO ingredientDTO)
        {
            if (ingredientDTO == null)
                return null;

            Ingredient ingredient = new Ingredient
            {
                Name = ingredientDTO.Name,
                RecipeIngredients = new List<RecipeIngredient>()
            };

            return ingredient;
        }
    }
}
