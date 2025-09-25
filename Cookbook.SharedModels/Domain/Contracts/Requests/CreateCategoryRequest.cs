using FluentValidation;

namespace Cookbook.SharedModels.Domain.Contracts.Requests
{
	public record CreateCategoryRequest
	{
		public required string Name { get; set; }
	}
	public class CreateCategoryRequestValidator : AbstractValidator<CreateCategoryRequest>
	{
		public CreateCategoryRequestValidator()
		{
			RuleFor(c => c.Name)
				.NotEmpty().NotNull().Length(2, 50).WithMessage("Name must have between 2 and 50 characters")
				.Matches("[a-zA-Z]+").WithMessage("Incorrect category name, only letters accepted");
		}
	}
}
