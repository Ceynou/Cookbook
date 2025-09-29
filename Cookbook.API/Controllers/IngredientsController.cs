using Cookbook.Core;
using Cookbook.Data.Repositories;
using Cookbook.SharedModels.Contracts.Requests;
using Cookbook.SharedModels.Entities;
using Cookbook.SharedModels.Mappers;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cookbook.API.Controllers;

[Authorize(Roles = "admin,user")]
[Route("api/cookbook/[controller]")]
[ApiController]
public class IngredientsController(ICookbookService cookbookService) : Controller
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll()
    {
        var ingredients = (await cookbookService.GetAllIngredientsAsync()).ToList();
        
        var response = ingredients.Select(i => i.ToIngredientResponse());
        
        return Ok(response);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetBy(int id)
    {
        var ingredient = await cookbookService.GetIngredientByAsync(id);
        if (ingredient == null) 
            return NotFound();
        
        var response = ingredient.ToIngredientResponse();
        
        return Ok(response);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create(IValidator<CreateIngredientRequest> validator,
        [FromBody] CreateIngredientRequest request)
    {
        await validator.ValidateAndThrowAsync(request);
			
        var createdIngredient = await cookbookService.CreateIngredientAsync(request.ToIngredient());
        
        var response = createdIngredient.ToIngredientResponse();
			
        return CreatedAtAction(nameof(GetBy), new { id = createdIngredient.IngredientId }, response);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update(IValidator<UpdateIngredientRequest> validator, int id, UpdateIngredientRequest request)
    {
        await validator.ValidateAndThrowAsync(request);
        
        var updatedIngredient = await cookbookService.ModifyIngredientAsync(id, request.ToIngredient());

        if (updatedIngredient is null)
            return BadRequest();
        var response = updatedIngredient.ToIngredientResponse();
        
        return Ok(response);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await cookbookService.DeleteIngredientAsync(id);
        return success ? NoContent() : NotFound();
    }
}