namespace Cookbook.SharedData.Entities;

public class Ingredient
{
    public short IngredientId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<RecipesIngredient> RecipesIngredients { get; set; } = new List<RecipesIngredient>();
}