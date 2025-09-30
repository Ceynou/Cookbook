using FluentValidation;

namespace Cookbook.SharedData.Contracts.Requests;

public record SignUpUserRequest
{
    public required string Username { get; init; }
    public required string Email { get; init; }
    public required DateOnly BirthDate { get; init; }
    public required string Password { get; init; }
}

public class SignUpRequestValidator : AbstractValidator<SignUpUserRequest>
{
    public SignUpRequestValidator()
    {
        RuleFor(s => s.Username).NotEmpty().WithMessage("Username is required");
        RuleFor(s => s.Email).NotEmpty().WithMessage("Email is required");
        RuleFor(s => s.BirthDate).NotEmpty().WithMessage("BirthDate is required")
            .LessThan(DateOnly.FromDateTime(DateTime.Now.AddYears(-13)))
            .WithMessage("You must be at least 13 years old"); // BLL?
        RuleFor(s => s.Password).NotEmpty().WithMessage("Password is required");
    }
}