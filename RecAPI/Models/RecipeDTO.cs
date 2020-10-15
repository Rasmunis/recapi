using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace RecAPI.Models
{
	public class RecipeDTO
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string Instructions { get; set; }
		public List<RecipeIngredientDTO> RecipeIngredients { get; set; }
	}
}
