namespace Cookbook.SharedModels.Contracts.Responses
{
	public record CategoryResponse
	{
		public required short CategoryId { get; init; }
		public required string Name { get; init; }
	}
}