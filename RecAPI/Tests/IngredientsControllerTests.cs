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
    public abstract class IngredientControllerTests
    {
        protected IngredientControllerTests(DbContextOptions<RecAPIContext> contextOptions)
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

            var fusilli = new Ingredient
            {
                Id = 1,
                Name = "Fusilli",
                RecipeIngredients = new List<RecipeIngredient>()
            };

            var garlic = new Ingredient
            {
                Id = 2,
                Name = "Garlic",
                RecipeIngredients = new List<RecipeIngredient>()
            };

            var recipe = new Recipe
            {
                Id = 1,
                Name = "Pure Garlic",
                Description = "For the true fans of garlic",
                Instructions = "Peel garlic. Consume garlic.",
                RecipeIngredients = new List<RecipeIngredient>()
            };

            var riGarlic = new RecipeIngredient
            {
                IngredientId = 1,
                Ingredient = garlic,
                RecipeId = 1,
                Recipe = recipe,
                Amount = 1000,
                Unit = (Unit)5
            };

            garlic.RecipeIngredients.Add(riGarlic);

            context.Ingredients.Add(garlic);
            context.Ingredients.Add(fusilli);
            context.SaveChanges();
        }

        [Fact]
        public async void Can_get_ingredient_without_recipe()
        {
            using var context = new RecAPIContext(ContextOptions);
            var controller = new IngredientsController(context);

            var ingredientActionResult = await controller.GetIngredient(1);

            var ingredient = ingredientActionResult.Value;

            Assert.Equal("Fusilli", ingredient.Name);
        }

        [Fact]
        public async void Can_post_ingredient()
        {
            using var context = new RecAPIContext(ContextOptions);
            var controller = new IngredientsController(context);


            var ingredientDTO = new IngredientDTO
            {
                Name = "Fusilli"
            };

            var ingredientActionResult = await controller.PostIngredient(ingredientDTO);

            var ingredient = (ingredientActionResult.Result as CreatedAtActionResult).Value as IngredientDTO;

            Assert.Equal("Fusilli", ingredient.Name);
        }

        public async void Can_get_ingredient_with_recipe()
        {
            using var context = new RecAPIContext(ContextOptions);
            var controller = new IngredientsController(context);

            var ingredientActionResult = await controller.GetIngredientWithRecipes(2);

            var ingredient = ingredientActionResult.Value;

            Assert.Equal("Pure Garlic", ingredient.RecipeIngredients.FirstOrDefault().RecipeName);

        }
    }
}
