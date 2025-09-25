using FluentValidation;

namespace Cookbook.SharedModels.Domain.Contracts.Requests;

public record SignUpUserRequest
{
    public string Email { get; init; }
    public string Username { get; init; }
    public string Password { get; init; }
}

public class SignUpRequestValidator : AbstractValidator<SignUpUserRequest>
{
    public SignUpRequestValidator()
    {
        RuleFor(x => x.Email).NotEmpty();
        RuleFor(x => x.Username).NotEmpty();
    }
}