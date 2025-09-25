using FluentValidation;

namespace Cookbook.SharedModels.Domain.Contracts.Requests
{
	public record CreateRecipeIngredientRequest
	{
		public required short IngredientId { get; set; }
		public required decimal Quantity { get; set; }
		public string? Unit { get; set; }
	}

	public class CreateRecipeIngredientRequestValidator : AbstractValidator<CreateRecipeIngredientRequest>
	{
		public CreateRecipeIngredientRequestValidator()
		{
			RuleFor(ri => (int)ri.IngredientId).NotEmpty().NotNull().GreaterThan(0);
			RuleFor(ri => ri.Quantity).NotEmpty().NotNull().GreaterThan(0);
			RuleFor(ri => ri.Unit).MaximumLength(20);
		}
	}
}
