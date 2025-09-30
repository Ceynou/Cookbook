namespace Cookbook.SharedData.Entities;

public class RecipesCategory
{
    public int RecipeId { get; set; }

    public short CategoryId { get; set; }

    public virtual Category Category { get; set; } = null!;
    public virtual Recipe Recipe { get; set; } = null!;
}