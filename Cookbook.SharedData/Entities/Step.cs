namespace Cookbook.SharedData.Entities;

public sealed class Step
{
    public int RecipeId { get; init; }

    public short StepNumber { get; init; }

    public string Instruction { get; init; } = null!;

    public TimeSpan Duration { get; init; }

    public bool IsCooking { get; init; }

    public Recipe Recipe { get; init; } = null!;
}