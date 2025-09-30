namespace Cookbook.SharedData.Entities;

public class RecipesIngredient
{
    public int RecipeId { get; set; }

    public short IngredientId { get; set; }

    public decimal Quantity { get; set; }

    public string? Unit { get; set; }

    public virtual Ingredient Ingredient { get; set; } = null!;

    public virtual Recipe Recipe { get; set; } = null!;
}