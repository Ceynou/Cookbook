using FluentValidation;

namespace Cookbook.SharedData.Contracts.Requests;

public record SignInUserRequest
{
    public required string Username { get; init; }
    public required string Password { get; init; }
}

public class SignInRequestValidator : AbstractValidator<SignInUserRequest>
{
    public SignInRequestValidator()
    {
        RuleFor(s => s.Username).NotEmpty().WithMessage("Username is required");
        RuleFor(s => s.Password).NotEmpty().WithMessage("Password is required");
    }
}