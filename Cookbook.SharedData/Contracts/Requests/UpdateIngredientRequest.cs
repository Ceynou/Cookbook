using FluentValidation;

namespace Cookbook.SharedData.Contracts.Requests;

public class UpdateIngredientRequest
{
    public string Name { get; set; }
}

public class UpdateIngredientRequestValidator : AbstractValidator<UpdateIngredientRequest>
{
    public UpdateIngredientRequestValidator()
    {
        RuleFor(x => x.Name).NotNull().NotEmpty();
    }
}