using Cookbook.Core;
using Cookbook.SharedData.Contracts.Requests;
using Cookbook.SharedData.Mappers;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cookbook.API.Controllers;

[AllowAnonymous]
[Route("api/[controller]")]
[ApiController]
public class AuthenticationController(IJwtService jwtService, IAccessService accessService) : ControllerBase
{
    [HttpPost("signup")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SignUp(IValidator<SignUpUserRequest> validator,
        [FromBody] SignUpUserRequest request)
    {
        await validator.ValidateAndThrowAsync(request);

        var user = await accessService.SignUpAsync(request.ToUser());
        var userResponse = user.ToSignUpUserResponse();
        string[] roles = user.IsAdmin ? ["admin", "user"] : ["user"];
        userResponse.Token = jwtService.GenerateJwt(user.Username, roles);

        return Ok(userResponse);
    }

    [HttpPost("signin")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> SignIn(IValidator<SignInUserRequest> validator,
        [FromBody] SignInUserRequest request)
    {
        await validator.ValidateAndThrowAsync(request);

        var user = await accessService.SignInAsync(request.ToUser());
        var userResponse = user.ToSignInUserResponse();
        string[] roles = user.IsAdmin ? ["admin", "user"] : ["user"];
        userResponse.Token = jwtService.GenerateJwt(user.Username, roles);

        return Ok(userResponse);
    }
}