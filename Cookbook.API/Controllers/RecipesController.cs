using Cookbook.Core;
using Cookbook.SharedData.Contracts.Requests;
using Cookbook.SharedData.Mappers;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cookbook.API.Controllers;

[Authorize(Roles = "admin,user")]
[Route("api/cookbook/[controller]")]
[ApiController]
public class RecipesController(ICookbookService cookbookService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll()
    {
        var recipes = (await cookbookService.GetAllRecipesAsync()).ToList();

        var response = recipes.Select(r => r.ToRecipeResponse());

        return Ok(response);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetBy(int id)
    {
        var recipe = await cookbookService.GetRecipeByAsync(id);

        var response = recipe.ToRecipeResponse();

        return Ok(response);
    }

    [HttpGet("{id:int}/full")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetFullBy(int id)
    {
        var recipe = await cookbookService.GetRecipeByAsync(id);

        var response = recipe.ToRecipeDetailResponse();

        return Ok(response);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create(IValidator<CreateRecipeRequest> validator,
        [FromBody] CreateRecipeRequest request)
    {
        await validator.ValidateAndThrowAsync(request);

        var createdRecipe = await cookbookService.CreateRecipeAsync(request.ToRecipe());

        var response = createdRecipe.ToRecipeResponse();

        return CreatedAtAction(nameof(GetBy), new { id = response.RecipeId }, response);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Modify(IValidator<UpdateRecipeRequest> validator,
        [FromRoute] int id, [FromBody] UpdateRecipeRequest request)
    {
        await validator.ValidateAndThrowAsync(request);

        var updatedRecipe = await cookbookService.ModifyRecipeAsync(id, request.ToRecipe());

        return Ok(updatedRecipe.ToRecipeResponse());
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(int id)
    {
        await cookbookService.DeleteRecipeAsync(id);

        return NoContent();
    }
}