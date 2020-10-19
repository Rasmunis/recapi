using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecAPI.Models;

namespace RecAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipesController : ControllerBase
    {
        private readonly RecAPIContext _context;

        public RecipesController(RecAPIContext context)
        {
            _context = context;
        }

        // GET: api/Recipes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RecipeDTO>>> GetRecipes()
        {
            var recipes = await _context.Recipes
                .Include(r => r.RecipeIngredients).ThenInclude(ri => ri.Ingredient)
                .ToListAsync();
            IEnumerable<RecipeDTO> recipeDTOs = recipes.Select(r => r.ToDto());
            return Ok(recipeDTOs);
        }

        // GET: api/Recipes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RecipeDTO>> GetRecipe(int id)
        {
            var recipe = await _context.Recipes
                .Where(r => r.Id == id)
                .Include(r => r.RecipeIngredients).ThenInclude(ri => ri.Ingredient)
                .FirstOrDefaultAsync();

            if (recipe == null)
            {
                return NotFound();
            }

            return recipe.ToDto();
        }

        // PUT: api/Recipes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRecipe(int id, Recipe recipe)
        {
            if (id != recipe.Id)
            {
                return BadRequest();
            }

            _context.Entry(recipe).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecipeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Recipes
        [HttpPost]
        public async Task<ActionResult<RecipeDTO>> PostRecipe(RecipeDTO recipeDTO)
        {
            Recipe recipe = recipeDTO.ToEntity();
            foreach (RecipeIngredientDTO recipeIngredientDTO in recipeDTO.RecipeIngredients)
            {
                Ingredient ingredient = await _context.Ingredients
                    .Where(i => i.Id == recipeIngredientDTO.IngredientId)
                    .Include(i => i.RecipeIngredients)
                    .FirstOrDefaultAsync();

                RecipeIngredient recipeIngredient = new RecipeIngredient
                {
                    RecipeId = recipe.Id,
                    Recipe = recipe,
                    IngredientId = recipeIngredientDTO.IngredientId,
                    Ingredient = ingredient,
                    Amount = recipeIngredientDTO.Amount,
                    Unit = recipeIngredientDTO.Unit
                };

                recipe.RecipeIngredients.Add(recipeIngredient);
                ingredient.RecipeIngredients.Add(recipeIngredient);
            }
            _context.Recipes.Add(recipe);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRecipe", new { id = recipe.Id }, recipe.ToDto());
        }

        // DELETE: api/Recipes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<RecipeDTO>> DeleteRecipe(int id)
        {
            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe == null)
            {
                return NotFound();
            }

            _context.Recipes.Remove(recipe);
            await _context.SaveChangesAsync();

            return recipe.ToDto();
        }

        private bool RecipeExists(int id)
        {
            return _context.Recipes.Any(e => e.Id == id);
        }
    }
}
