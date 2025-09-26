using FluentValidation;
namespace Cookbook.SharedModels.Contracts.Requests
{
	public record UpdateRecipeCategoryRequest
	{
		public required short CategoryId { get; set; }
	}
	
	public class UpdateRecipeCategoryRequestValidator : AbstractValidator<UpdateRecipeCategoryRequest>
	{
		public UpdateRecipeCategoryRequestValidator()
		{
			RuleFor(r => (int)r.CategoryId).NotEmpty().NotNull().GreaterThan(0);
		}
	}
}