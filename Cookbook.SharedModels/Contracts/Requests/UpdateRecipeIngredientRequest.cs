using FluentValidation;
namespace Cookbook.SharedModels.Contracts.Requests
{
	public record UpdateRecipeIngredientRequest
	{
		public required short IngredientId { get; set; }
		public required decimal Quantity { get; set; }
		public string? Unit { get; set; }
	}
	
	public class UpdateRecipeIngredientRequestValidator : AbstractValidator<UpdateRecipeIngredientRequest>
	{
		public UpdateRecipeIngredientRequestValidator()
		{
			RuleFor(ri => (int)ri.IngredientId).GreaterThan(0);
			RuleFor(ri => ri.Quantity).GreaterThan(0);
			RuleFor(ri => ri.Unit).MaximumLength(20);
		}
	}
}