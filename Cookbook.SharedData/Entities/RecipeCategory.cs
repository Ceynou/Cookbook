namespace Cookbook.SharedData.Entities;

public sealed class RecipeCategory
{
    public int RecipeId { get; init; }

    public short CategoryId { get; init; }

    public Category Category { get; init; } = null!;
    public Recipe Recipe { get; init; } = null!;
}