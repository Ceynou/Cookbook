using FluentValidation;

namespace Cookbook.SharedModels.Contracts.Requests;

public record SignInUserRequest
{
    public string Username { get; init; }
    public string Password { get; init; }
}

public class SignInRequestValidator : AbstractValidator<SignInUserRequest>
{
    public SignInRequestValidator()
    {
        RuleFor(s => s.Username).NotEmpty().WithMessage("Username is required");
        RuleFor(s => s.Password).NotEmpty().WithMessage("Password is required");
    }
}