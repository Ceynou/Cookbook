namespace Cookbook.SharedData.Entities;

public sealed class RecipeIngredient
{
    public int RecipeId { get; init; }

    public short IngredientId { get; init; }

    public decimal Quantity { get; init; }

    public string? Unit { get; init; }

    public Ingredient Ingredient { get; init; } = null!;

    public Recipe Recipe { get; init; } = null!;
}