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

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetBy(int id)
    {
        var category = await cookbookService.GetCategoryByAsync(id);

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
        await validator.ValidateAndThrowAsync(request);

        var createdCategory = await cookbookService.CreateCategoryAsync(request.ToCategory());

        var response = createdCategory.ToCategoryResponse();

        return CreatedAtAction(nameof(GetBy), new { id = createdCategory.CategoryId }, response);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update(IValidator<UpdateCategoryRequest> validator, int id,
        UpdateCategoryRequest request)
    {
        await validator.ValidateAndThrowAsync(request);

        var updatedCategory = await cookbookService.ModifyCategoryAsync(id, request.ToCategory());

        var response = updatedCategory.ToCategoryResponse();

        return Ok(response);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(int id)
    {
        await cookbookService.DeleteCategoryAsync(id);

        return NoContent();
    }
}