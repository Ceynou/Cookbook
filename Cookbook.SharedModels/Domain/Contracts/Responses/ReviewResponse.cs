namespace Cookbook.SharedModels.Domain.Contracts.Responses
{
	public record ReviewResponse
	{
		public required int ReviewerId { get; init; }
		public required short Rating { get; init; }
		public string? Impression { get; init; }
		public required string Username { get; init; }
	}
}