namespace Cookbook.SharedData.Entities;

public sealed class Recipe
{
    public int RecipeId { get; set; }

    public string Title { get; init; } = null!;

    public TimeSpan PreparationDuration { get; init; }

    public TimeSpan CookingDuration { get; init; }

    public short Difficulty { get; init; }

    public string ImagePath { get; init; } = null!;

    public int? CreatorId { get; set; }

    public User? Creator { get; init; }

    public ICollection<RecipesIngredient> RecipesIngredients { get; set; } = new List<RecipesIngredient>();

    public ICollection<Review> Reviews { get; init; } = new List<Review>();

    public ICollection<Step> Steps { get; set; } = new List<Step>();

    public ICollection<RecipesCategory> RecipesCategories { get; set; } = new List<RecipesCategory>();
}