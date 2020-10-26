using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Frameworks;
using RecAPI;
using RecAPI.Controllers;
using RecAPI.Models;
using Xunit;

namespace RecAPI.Tests
{
    public abstract class RecipeControllerTests
    {
        protected RecipeControllerTests(DbContextOptions<RecAPIContext> contextOptions)
        {
            ContextOptions = contextOptions;

            Seed();
        }

        protected DbContextOptions<RecAPIContext> ContextOptions { get; }

        private void Seed()
        {
            using var context = new RecAPIContext(ContextOptions);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var garlic = new Ingredient
            {
                Id = 1,
                Name = "Garlic",
                RecipeIngredients = new List<RecipeIngredient>()
            };
            var fusilli = new Ingredient
            {
                Id = 2,
                Name = "Fusilli",
                RecipeIngredients = new List<RecipeIngredient>()
            };

            var ketchup = new Ingredient
            {
                Id = 3,
                Name = "Ketchup",
                RecipeIngredients = new List<RecipeIngredient>()
            };

            var recipe = new Recipe
            {
                Id = 1,
                Name = "Pasta with garlic",
                Description = "Garlic is tasty. Fusilli is screw-shaped.",
                Instructions = "Boil pasta. Add all of the cloves of garlic.",
                RecipeIngredients = new List<RecipeIngredient>()
            };

            var riGarlic = new RecipeIngredient
            {
                RecipeId = recipe.Id,
                Recipe = recipe,
                IngredientId = garlic.Id,
                Ingredient = garlic,
                Amount = 200,
                Unit = (Unit)1
            };

            var riFusilli = new RecipeIngredient
            {
                RecipeId = recipe.Id,
                Recipe = recipe,
                IngredientId = fusilli.Id,
                Ingredient = fusilli,
                Amount = 200,
                Unit = (Unit)1
            };

            recipe.RecipeIngredients.Add(riGarlic);
            recipe.RecipeIngredients.Add(riFusilli);
            garlic.RecipeIngredients.Add(riGarlic);
            fusilli.RecipeIngredients.Add(riFusilli);

            context.Ingredients.Add(ketchup);
            context.Recipes.Add(recipe);
            context.SaveChanges();
        }

        [Fact]
        public async void Can_get_recipe()
        {
            using var context = new RecAPIContext(ContextOptions);
            var controller = new RecipesController(context);

            var recipeActionResult = await controller.GetRecipe(1);
            var recipe = recipeActionResult.Value;

            Assert.Equal("Pasta with garlic", recipe.Name);
        }

        [Fact]
        public async void Get_recipe_includes_ingredients()
        {
            using var context = new RecAPIContext(ContextOptions);
            var controller = new RecipesController(context);

            var recipeActionResult = await controller.GetRecipe(1);
            var recipe = recipeActionResult.Value;

            Assert.Equal("Garlic", recipe.RecipeIngredients.FirstOrDefault().IngredientName);
        }

        [Fact]
        public async void Can_post_recipe()
        {
            using var context = new RecAPIContext(ContextOptions);
            var controller = new RecipesController(context);

            var riFusilliDTO = new RecipeIngredientDTO
            {
                IngredientId = 2,
                IngredientName = "Fusilli",
                Amount = 200,
                Unit = (Unit)1
            };

            var riKetchupDTO = new RecipeIngredientDTO
            {
                IngredientId = 3,
                IngredientName = "Ketchup",
                Amount = 200,
                Unit = (Unit)1
            };

            var recipeDTO = new RecipeDTO
            {
                Name = "Pasta with ketchup",
                Description = "Better than with garlic",
                Instructions = "Boil pasta. Add ketchup.",
                RecipeIngredients = new List<RecipeIngredientDTO> { riFusilliDTO, riKetchupDTO }
            };

            var recipeActionResult = await controller.PostRecipe(recipeDTO);
            var recipe = (recipeActionResult.Result as CreatedAtActionResult).Value as RecipeDTO;

            Assert.Equal("Pasta with ketchup", recipe.Name);
            Assert.Equal("Fusilli", recipe.RecipeIngredients.FirstOrDefault().IngredientName);
        }
    }
}
