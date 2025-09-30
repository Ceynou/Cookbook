namespace Cookbook.SharedData.Entities;

public class Review
{
    public int RecipeId { get; set; }

    public int ReviewerId { get; set; }

    public short Rating { get; set; }

    public string? Impression { get; set; }

    public virtual Recipe Recipe { get; set; } = null!;

    public virtual User Reviewer { get; set; } = null!;
}