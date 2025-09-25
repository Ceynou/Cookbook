using FluentValidation;
namespace Cookbook.SharedModels.Domain.Contracts.Requests
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