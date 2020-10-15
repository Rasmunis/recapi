using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecAPI.Models
{
    public class RecipeIngredientDTO
    {
        public int RecipeId { get; set; }
        public string RecipeName { get; set; }
        public int IngredientId { get; set; }
        public string IngredientName { get; set; }
        public decimal Amount { get; set; }
        public Unit Unit { get; set; }
    }
}
