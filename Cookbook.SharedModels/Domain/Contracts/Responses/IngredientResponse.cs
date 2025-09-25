namespace Cookbook.SharedModels.Domain.Contracts.Responses
{
	public record IngredientResponse
	{
		public required short IngredientId { get; init; }
		public required string Name { get; init; }
	}
}