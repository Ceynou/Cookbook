namespace Cookbook.SharedData.Contracts.Responses;

public record StepResponse
{
    public required short StepNumber { get; init; }
    public required string Instruction { get; init; }
    public required TimeSpan Duration { get; init; }
    public required bool IsCooking { get; init; }
}