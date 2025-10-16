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
        RuleFor(s => s.Username).NotNull().NotEmpty().WithMessage("Username is required");
        RuleFor(s => s.Email).NotNull().NotEmpty().WithMessage("Email is required");
        RuleFor(s => s.Password).NotNull().NotEmpty().WithMessage("Password is required")
            .MinimumLength(4).WithMessage("Password must have at least 4 characters")
            .MaximumLength(20).WithMessage("Password must have at most 20 characters")
            .Matches("[a-zA-Z0-9]+").WithMessage("Incorrect password, only letters and numbers accepted");
    }
}