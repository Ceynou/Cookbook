using Microsoft.AspNetCore.Mvc;
using Cookbook.SharedModels.Entities;
using FluentValidation;
using Cookbook.Core;
using Cookbook.SharedModels.Domain.Contracts.Requests;
using Cookbook.SharedModels.Mappers;
using Microsoft.AspNetCore.Authorization;

namespace Cookbook.API.Controllers
{
	[Authorize(Roles = "admin,user")]
	[Route("api/cookbook/[controller]")]
	[ApiController]
	public class RecipesController(ICookbookService recipeService) : ControllerBase
	{
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> Get()
		{
			var recipes = (await recipeService.GetAllRecipesAsync()).ToList();
			if (recipes.Count == 0)
				return NotFound("No recipes found.");

			var response = recipes.Select(r => r.ToRecipeResponse());

			return Ok(response);
		}

		[HttpGet("{id:int}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetBy(int id)
		{
			var recipe = await recipeService.GetRecipeByAsync(id);
			if (recipe == null)
				return NotFound($"Recipe with ID {id} not found.");

			var response = recipe.ToRecipeResponse();

			return Ok(response);
		}		
		
		[HttpGet("{id:int}/full")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetFullBy(int id)
		{
			var recipe = await recipeService.GetRecipeByAsync(id);
			if (recipe == null)
				return NotFound($"Recipe with ID {id} not found.");
			var response = recipe.ToRecipeDetailResponse();
			
			return Ok(response);
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> Create([FromServices] IValidator<CreateRecipeRequest> validator, 
			[FromBody] CreateRecipeRequest request)
		{
			var res = await validator.ValidateAsync(request);
			if ( !res.IsValid)
				return BadRequest();
			
			var createdRecipe = await recipeService.CreateRecipeAsync(request.ToRecipe());
			if (createdRecipe is null)
				return Problem();
			var response = createdRecipe.ToRecipeResponse();
			
			return CreatedAtAction(nameof(GetBy), new { id = createdRecipe.RecipeId }, response);
		}

		[HttpPut("{id:int}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> Modify([FromServices] IValidator<UpdateRecipeRequest> validator, 
			[FromRoute] int id, [FromBody] UpdateRecipeRequest request)
		{
			var res = await validator.ValidateAsync(request);
			if ( !res.IsValid)
				return BadRequest();

			var dbRecipe = await recipeService.GetRecipeByAsync(id);
			if (dbRecipe == null)
				return NotFound($"Recipe with ID {id} not found.");

			var updatedRecipe = await recipeService.ModifyRecipeAsync(id, request.ToRecipe());

			if (updatedRecipe == null)
				return NotFound($"Recipe with ID {id} not found after update attempt.");

			return Ok(updatedRecipe.ToRecipeResponse());
		}

		[HttpDelete("{id:int}")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> Delete(int id)
		{
			var success = await recipeService.DeleteRecipeAsync(id);
			if (!success)
				return NotFound($"Recipe with ID {id} not found.");

			return NoContent();
		}
	}
}
