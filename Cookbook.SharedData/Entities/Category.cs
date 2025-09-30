namespace Cookbook.SharedData.Entities;

public class Category
{
    public short CategoryId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<RecipesCategory> RecipesCategories { get; set; } = new List<RecipesCategory>();
}