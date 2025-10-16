namespace Cookbook.SharedData.Entities;

public sealed class Review
{
    public int RecipeId { get; init; }

    public int ReviewerId { get; init; }

    public short Rating { get; init; }

    public string? Impression { get; init; }

    public Recipe Recipe { get; init; } = null!;

    public User Reviewer { get; init; } = null!;
}