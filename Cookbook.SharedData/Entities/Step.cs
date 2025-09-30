namespace Cookbook.SharedData.Entities;

public class Step
{
    public int RecipeId { get; set; }

    public short StepNumber { get; set; }

    public string Instruction { get; set; } = null!;

    public TimeSpan Duration { get; set; }

    public bool IsCooking { get; set; }

    public virtual Recipe Recipe { get; set; } = null!;
}