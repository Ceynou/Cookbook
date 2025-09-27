using Cookbook.Core;
using Cookbook.SharedModels.Contracts.Requests;
using Cookbook.SharedModels.Mappers;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cookbook.API.Controllers;

[Authorize(Roles = "admin,user")]
[Route("api/cookbook/[controller]")]
[ApiController]
public class CategoriesController(ICookbookService cookbookService) : Controller
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll()
    {
        var categories = (await cookbookService.GetAllCategoriesAsync()).ToList();
        
        var response = categories.Select(i => i.ToCategoryResponse());
        
        return Ok(response);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetBy(int id)
    {
        var category = await cookbookService.GetCategoryByAsync(id);
        if (category == null) 
            return NotFound($"Category with id {id} not found");
        
        var response = category.ToCategoryResponse();
        
        return Ok(response);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create(IValidator<CreateCategoryRequest> validator,
        [FromBody] CreateCategoryRequest request)
    {
        var res = await validator.ValidateAsync(request);
        if ( !res.IsValid)
            return BadRequest(res.Errors);
			
        var createdCategory = await cookbookService.CreateCategoryAsync(request.ToCategory());
        
        var response = createdCategory.ToCategoryResponse();
			
        return CreatedAtAction(nameof(GetBy), new { id = createdCategory.CategoryId }, response);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update(IValidator<UpdateCategoryRequest> validator, int id, UpdateCategoryRequest request)
    {
        var res = await validator.ValidateAsync(request);
        if ( !res.IsValid)
            return BadRequest(res.Errors);
        
        var updatedCategory = await cookbookService.ModifyCategoryAsync(id, request.ToCategory());

        if (updatedCategory is null)
            return BadRequest("Category was not updated");
        var response = updatedCategory.ToCategoryResponse();
        
        return Ok(response);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await cookbookService.DeleteCategoryAsync(id);
        return success ? NoContent() : NotFound($"Category with id {id} not found");
    }
}