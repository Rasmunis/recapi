using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecAPI.Models;

namespace RecAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngredientsController : ControllerBase
    {
        private readonly RecAPIContext _context;

        public IngredientsController(RecAPIContext context)
        {
            _context = context;
        }

        // GET: api/Ingredients
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IngredientDTO>>> GetIngredients()
        {
            var ingredients = await _context.Ingredients
                .ToListAsync();
            return Ok(ingredients.Select(i => i.ToDto()));
        }

        // GET: api/Ingredients/search?query=helloworld
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<IngredientDTO>>> GetIngredientsByNameSubstring([FromQuery(Name = "query")] string query)
        {
            var ingredients = await _context.Ingredients
                .Where(i => EF.Functions.Like(i.Name, $"%{query}%"))
                .ToListAsync();
            return Ok(ingredients.Select(i => i.ToDto()));
        }

        // GET: api/Ingredients/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IngredientDTO>> GetIngredient(int id)
        {
            var ingredient = await _context.Ingredients.FindAsync(id);

            if (ingredient == null)
            {
                return NotFound();
            }

            return ingredient.ToDto();
        }

        // GET: api/Ingredients/5
        [HttpGet("{id}/recipes")]
        public async Task<ActionResult<IEnumerable<RecipeIngredientDTO>>> GetRecipeIngredientsOfIngredient(int id)
        {
            var recipeIngredients = await _context.RecipeIngredients
                .Where(ri => ri.IngredientId == id)
                .Include(ri => ri.Recipe)
                .Include(ri => ri.Ingredient)
                .ToListAsync();

            if (recipeIngredients == null)
            {
                return NotFound();
            }

            return Ok(recipeIngredients.Select(ri => ri.ToDto()));
        }

        // PUT: api/Ingredients/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIngredient(int id, Ingredient ingredient)
        {
            if (id != ingredient.Id)
            {
                return BadRequest();
            }

            _context.Entry(ingredient).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IngredientExists(id))
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

        // POST: api/Ingredients
        [HttpPost]
        public async Task<ActionResult<IngredientDTO>> PostIngredient(IngredientDTO ingredientDTO)
        {
            Ingredient ingredient = ingredientDTO.ToEntity();
            _context.Ingredients.Add(ingredient);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetIngredient", new { id = ingredient.Id }, ingredient.ToDto());
        }

        // DELETE: api/Ingredients/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<IngredientDTO>> DeleteIngredient(int id)
        {
            var ingredient = await _context.Ingredients.FindAsync(id);
            if (ingredient == null)
            {
                return NotFound();
            }

            _context.Ingredients.Remove(ingredient);
            await _context.SaveChangesAsync();

            return ingredient.ToDto();
        }

        private bool IngredientExists(int id)
        {
            return _context.Ingredients.Any(e => e.Id == id);
        }
    }
}
