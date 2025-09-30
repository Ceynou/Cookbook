using FluentValidation;

namespace Cookbook.SharedData.Contracts.Requests;

public record CreateRecipeRequest
{
    public required string Title { get; set; }
    public required short Difficulty { get; set; }
    public required string ImagePath { get; set; }
    public required List<CreateRecipeIngredientRequest> Ingredients { get; set; }
    public required List<CreateRecipeCategoryRequest> Categories { get; set; }
    public required List<CreateStepRequest> Steps { get; set; }
}

public class CreateRecipeRequestValidator : AbstractValidator<CreateRecipeRequest>
{
    public CreateRecipeRequestValidator()
    {
        RuleFor(r => r.Title).NotEmpty().NotNull().Length(3, 100)
            .Matches("[a-zA-Z]+").WithMessage("Incorrect title");

        RuleFor(r => (int)r.Difficulty).InclusiveBetween(1, 10)
            .WithMessage("Difficulty must be between 1 and 10");

        RuleFor(r => r.ImagePath).NotEmpty().NotNull().Length(1, 255);

        RuleFor(r => r.Categories).NotEmpty().NotNull();
        RuleFor(r => r.Ingredients).NotEmpty().NotNull();
        RuleFor(r => r.Steps).NotEmpty().NotNull();
    }
}