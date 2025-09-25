using FluentValidation;

namespace Cookbook.SharedModels.Domain.Contracts.Requests
{
	public record UpdateStepRequest
	{
		public required int StepNumber { get; set; }
		public required string Instruction { get; set; }
		public required TimeSpan Duration { get; set; }
		public required bool IsCooking { get; set; }
	}

	public class UpdateStepRequestValidator : AbstractValidator<UpdateStepRequest>
	{
		public UpdateStepRequestValidator()
		{
			RuleFor(s => s.StepNumber).InclusiveBetween(1, 20)
				.WithMessage("Step number must be between 1 and 20");
			
			RuleFor(s => s.Instruction).NotEmpty().NotNull().Length(5, 500)
				.Matches("[a-zA-Z]+").WithMessage("Incorrect step description");
			
			RuleFor(s => s.Duration).GreaterThan(TimeSpan.Zero);
			
			RuleFor(s => s.IsCooking).NotNull();
		}
	}
}