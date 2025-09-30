namespace Cookbook.SharedData.Contracts.Responses;

public record RecipeIngredientResponse
{
    public required short IngredientId { get; init; }
    public required decimal Quantity { get; init; }
    public string? Unit { get; init; }
}