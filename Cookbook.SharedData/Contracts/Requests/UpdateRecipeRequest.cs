using FluentValidation;

namespace Cookbook.SharedData.Contracts.Requests;

public record UpdateRecipeRequest
{
    public required string Title { get; set; }
    public required short Difficulty { get; set; }
    public string? ImagePath { get; set; }
    public required ICollection<UpdateRecipeIngredientRequest> Ingredients { get; set; }
    public required ICollection<UpdateRecipeCategoryRequest> Categories { get; set; }
    public required ICollection<UpdateStepRequest> Steps { get; set; }
}

public class UpdateRecipeRequestValidator : AbstractValidator<UpdateRecipeRequest>
{
    public UpdateRecipeRequestValidator()
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