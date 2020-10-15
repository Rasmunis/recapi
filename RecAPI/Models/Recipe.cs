using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace RecAPI.Models
{
	public class Recipe
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string Instructions { get; set; }
		public List<RecipeIngredient> RecipeIngredients { get; set; }
	}

	public static class RecipeUtils
    {
		public static RecipeDTO ToDto(this Recipe recipe)
		{
			if (recipe == null)
				return null;

			RecipeDTO recipeDTO = new RecipeDTO
			{
				Id = recipe.Id,
				Name = recipe.Name,
				Description = recipe.Description,
				Instructions = recipe.Instructions,
				RecipeIngredients = new List<RecipeIngredientDTO>()
			};

			foreach (RecipeIngredient recipeIngredient in recipe.RecipeIngredients)
            {
				recipeDTO.RecipeIngredients.Add(recipeIngredient.ToDto());
            }

			return recipeDTO;
		}

		public static Recipe ToEntity(this RecipeDTO recipeDTO)
        {
			if (recipeDTO == null)
				return null;

			Recipe recipe = new Recipe
			{
				Name = recipeDTO.Name,
				Description = recipeDTO.Description,
				Instructions = recipeDTO.Instructions,
				RecipeIngredients = new List<RecipeIngredient>()
			};

			foreach (RecipeIngredientDTO recipeIngredientDTO in recipeDTO.RecipeIngredients)
            {
				recipe.RecipeIngredients.Add(recipeIngredientDTO.ToEntity());
            }

			return recipe;
        }
	}
}
