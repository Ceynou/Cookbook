using FluentValidation;

namespace Cookbook.SharedModels.Contracts.Requests;

public record UpdateCategoryRequest
{
    public required string Name { get; init; }
}

public class UpdateCategoryRequestValidator : AbstractValidator<UpdateCategoryRequest>
{
    public UpdateCategoryRequestValidator()
    {
        RuleFor(x => x.Name).NotNull().NotEmpty().WithMessage("Name is required");
    }
}