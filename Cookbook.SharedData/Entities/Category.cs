namespace Cookbook.SharedData.Entities;

public sealed class Category
{
    public short CategoryId { get; set; }

    public string Name { get; init; } = null!;

    public ICollection<RecipesCategory> RecipesCategories { get; init; } = new List<RecipesCategory>();
}