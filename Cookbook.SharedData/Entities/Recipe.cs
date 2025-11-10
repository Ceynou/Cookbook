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

    public ICollection<RecipeIngredient> RecipesIngredients { get; set; } = new List<RecipeIngredient>();

    public ICollection<Review> Reviews { get; init; } = new List<Review>();

    public ICollection<Step> Steps { get; set; } = new List<Step>();

    public ICollection<RecipeCategory> RecipesCategories { get; set; } = new List<RecipeCategory>();
}