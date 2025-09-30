namespace Cookbook.SharedData.Contracts.Responses;

public record RecipeResponse
{
    public required int RecipeId { get; init; }
    public required string Title { get; init; }
    public required TimeSpan PreparationDuration { get; init; }
    public required TimeSpan CookingDuration { get; init; }
    public required short Difficulty { get; init; }
    public string? ImagePath { get; init; }
    public required int CreatorId { get; init; }
    public required string CreatorUsername { get; init; }
    public string? CreatorProfilePicture { get; init; }
    public required int IngredientsCount { get; init; }
    public required int StepsCount { get; init; }
    public required int ReviewsCount { get; init; }
    public required double ReviewRating { get; init; }
}