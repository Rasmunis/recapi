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
}
