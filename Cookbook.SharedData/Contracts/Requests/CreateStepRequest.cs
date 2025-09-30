using FluentValidation;

namespace Cookbook.SharedData.Contracts.Requests;

public record CreateStepRequest
{
    public required short StepNumber { get; set; }
    public required string Instruction { get; set; }
    public required TimeSpan Duration { get; set; }
    public required bool IsCooking { get; set; }
}

public class CreateStepRequestValidator : AbstractValidator<CreateStepRequest>
{
    public CreateStepRequestValidator()
    {
        RuleFor(s => (int)s.StepNumber).InclusiveBetween(1, 20)
            .WithMessage("Step number must be between 1 and 20");

        RuleFor(s => s.Instruction).NotEmpty().NotNull().Length(5, 500)
            .Matches("[a-zA-Z]+").WithMessage("Incorrect step description");

        RuleFor(s => s.Duration).GreaterThan(TimeSpan.Zero);

        RuleFor(s => s.IsCooking).NotNull();
    }
}