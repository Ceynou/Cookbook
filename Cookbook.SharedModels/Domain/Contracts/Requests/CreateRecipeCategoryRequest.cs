using FluentValidation;

namespace Cookbook.SharedModels.Domain.Contracts.Requests
{
	public record CreateRecipeCategoryRequest
	{
		public required short CategoryId { get; set; }
	}

	public class CreateRecipeCategoryRequestValidator : AbstractValidator<CreateRecipeCategoryRequest>
	{
		public CreateRecipeCategoryRequestValidator()
		{
			RuleFor(r => (int)r.CategoryId)
				.NotEmpty().NotNull().GreaterThan(0).WithMessage("CategoryId must have a value");
		}
	}
}