namespace Cookbook.SharedModels.Domain.Contracts.Responses
{
	public record StepResponse
	{
		// TODO no need for the recipe id?
		public required short StepNumber { get; init; }
		public required string Instruction { get; init; }
		public required TimeSpan Duration { get; init; }
		public required bool IsCooking { get; init; }
	}

}