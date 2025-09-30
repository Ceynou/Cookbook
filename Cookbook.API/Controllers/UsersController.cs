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
public class UsersController(ICookbookService cookbookService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll()
    {
        var users = (await cookbookService.GetAllUsersAsync()).ToList();

        var response = users.Select(i => i.ToUserResponse());

        return Ok(response);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetBy(int id)
    {
        var user = await cookbookService.GetUserByAsync(id);

        var response = user.ToUserResponse();

        return Ok(response);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create(IValidator<CreateUserRequest> validator,
        [FromBody] CreateUserRequest request)
    {
        await validator.ValidateAndThrowAsync(request);

        var createdUser = await cookbookService.CreateUserAsync(request.ToUser());

        var response = createdUser.ToUserResponse();

        return CreatedAtAction(nameof(GetBy), new { id = createdUser.UserId }, response);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update(IValidator<UpdateUserRequest> validator, int id, UpdateUserRequest request)
    {
        await validator.ValidateAndThrowAsync(request);

        var updatedUser = await cookbookService.ModifyUserAsync(id, request.ToUser());

        var response = updatedUser.ToUserResponse();

        return Ok(response);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(int id)
    {
        await cookbookService.DeleteUserAsync(id);
        return NoContent();
    }
}