using Cookbook.Core;
using Cookbook.SharedModels.Domain.Contracts.Requests;
using Cookbook.SharedModels.Mappers;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Cookbook.API.Controllers;

public class AuthenticationController(IJwtService jwtService, IAccessService accessService) : Controller
{
    // TODO
    [HttpPost]
    public async Task<IActionResult> SignIn([FromServices] IValidator<SignInUserRequest> validator,
        [FromBody] SignInUserRequest request)
    {
        validator.ValidateAndThrow(request);

        var user = await accessService.SignInAsync(request.ToUser());
        var userResponse = user.ToSignInUserResponse();
        userResponse.Token = jwtService.GenerateJwt(user.Username);

        return Ok(userResponse);
    }
}