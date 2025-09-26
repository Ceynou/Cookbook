namespace Cookbook.SharedModels.Contracts.Responses
{
	public record RecipeDetailResponse
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
		public required IEnumerable<RecipeIngredientResponse> Ingredients { get; init; }
		public required IEnumerable<RecipeCategoryResponse> Categories { get; init; }
		public required IEnumerable<StepResponse> Steps { get; init; }
		public required IEnumerable<ReviewResponse> Reviews { get; init; }
		public required int ReviewsCount { get; init; }
		public required double ReviewRating { get; init; }
	}
}
