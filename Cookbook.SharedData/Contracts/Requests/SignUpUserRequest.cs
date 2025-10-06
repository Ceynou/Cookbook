using FluentValidation;

namespace Cookbook.SharedData.Contracts.Requests;

public record SignUpUserRequest
{
    public required string Username { get; init; }
    public required string Email { get; init; }
    public required string Password { get; init; }
}

public class SignUpRequestValidator : AbstractValidator<SignUpUserRequest>
{
    public SignUpRequestValidator()
    {
        RuleFor(s => s.Username).NotEmpty().WithMessage("Username is required");
        RuleFor(s => s.Email).NotEmpty().WithMessage("Email is required");
        RuleFor(s => s.Password).NotEmpty().WithMessage("Password is required");
    }
}