using FluentValidation;

namespace Cookbook.SharedModels.Contracts.Requests
{
	public record CreateIngredientRequest
	{
		public required string Name { get; set; }
	}

	public class CreateIngredientRequestValidator : AbstractValidator<CreateIngredientRequest>
	{
		public CreateIngredientRequestValidator()
		{
			RuleFor(i => i.Name)
				.NotEmpty().NotNull().Length(2, 50).WithMessage("Name must have between 2 and 50 characters")
				.Matches("[a-zA-Z]+").WithMessage("Incorrect category name, only letters accepted");
		}
	}
}