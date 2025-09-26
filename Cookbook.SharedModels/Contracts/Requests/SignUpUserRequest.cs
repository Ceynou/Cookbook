using FluentValidation;

namespace Cookbook.SharedModels.Contracts.Requests;

public record SignUpUserRequest
{
    public string Username { get; init; }
    public string Email { get; init; }
    public DateOnly BirthDate { get; init; }
    public string Password { get; init; }
}

public class SignUpRequestValidator : AbstractValidator<SignUpUserRequest>
{
    public SignUpRequestValidator()
    {
        RuleFor(s => s.Username).NotEmpty().WithMessage("Username is required");
        RuleFor(s => s.Email).NotEmpty().WithMessage("Email is required");
        RuleFor(s => s.BirthDate).NotEmpty().WithMessage("BirthDate is required")
            .LessThan(DateOnly.FromDateTime(DateTime.Now.AddYears(-13))).WithMessage("You must be at least 13 years old");
        RuleFor(s => s.Password).NotEmpty().WithMessage("Password is required");
    }
}