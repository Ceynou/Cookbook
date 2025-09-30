namespace Cookbook.SharedData.Entities;

public class Recipe
{
    public int RecipeId { get; set; }

    public string Title { get; set; } = null!;

    public TimeSpan PreparationDuration { get; set; }

    public TimeSpan CookingDuration { get; set; }

    public short Difficulty { get; set; }

    public string ImagePath { get; set; } = null!;

    public int? CreatorId { get; set; }

    public virtual User? Creator { get; set; }

    public virtual ICollection<RecipesIngredient> RecipesIngredients { get; set; } = new List<RecipesIngredient>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual ICollection<Step> Steps { get; set; } = new List<Step>();

    public virtual ICollection<RecipesCategory> RecipesCategories { get; set; } = new List<RecipesCategory>();
}