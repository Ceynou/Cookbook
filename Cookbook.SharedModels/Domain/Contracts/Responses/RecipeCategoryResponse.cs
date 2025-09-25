namespace Cookbook.SharedModels.Domain.Contracts.Responses
{
	public record RecipeCategoryResponse
	{
		public required short CategoryId { get; init; }
	}
}