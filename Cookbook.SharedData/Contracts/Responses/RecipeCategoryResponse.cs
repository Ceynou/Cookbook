﻿namespace Cookbook.SharedData.Contracts.Responses;

public record RecipeCategoryResponse
{
    public required short CategoryId { get; init; }
}