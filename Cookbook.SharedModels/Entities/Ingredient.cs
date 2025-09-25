using System;
using System.Collections.Generic;

namespace Cookbook.SharedModels.Entities;

public partial class Ingredient
{
    public short IngredientId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<RecipesIngredient> RecipesIngredients { get; set; } = new List<RecipesIngredient>();
}
