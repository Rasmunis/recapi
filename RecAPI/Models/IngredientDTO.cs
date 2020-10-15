using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecAPI.Models
{
    public class IngredientDTO
    {
		public int Id { get; set; }
		public string Name { get; set; }
		public List<RecipeIngredientDTO> RecipeIngredients { get; set; }
	}
}
