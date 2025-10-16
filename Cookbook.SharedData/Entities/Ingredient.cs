namespace Cookbook.SharedData.Entities;

public sealed class Ingredient
{
    public short IngredientId { get; set; }

    public string Name { get; init; } = null!;

    public ICollection<RecipesIngredient> RecipesIngredients { get; init; } = new List<RecipesIngredient>();
}