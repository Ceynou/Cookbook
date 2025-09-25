using FluentValidation;

namespace Cookbook.SharedModels.Domain.Contracts.Requests;

public record SignInUserRequest
{
    public string Email { get; init; }
    public string Username { get; init; }
    public string Password { get; init; }
}

public class SignInRequestValidator : AbstractValidator<SignInUserRequest>
{
    public SignInRequestValidator()
    {
        RuleFor(s => s.Email).NotEmpty().WithMessage("Email is required");
        RuleFor(s => s.Password).NotEmpty().WithMessage("Password is required");
    }
}