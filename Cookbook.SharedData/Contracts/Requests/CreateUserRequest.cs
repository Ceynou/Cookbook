using FluentValidation;

namespace Cookbook.SharedData.Contracts.Requests;

public class CreateUserRequest
{
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required DateOnly BirthDate { get; set; }
    public required bool IsAdmin { get; set; }
}

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidator()
    {
        RuleFor(x => x.Username).NotEmpty().WithMessage("Username is required");
        RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required");
        RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required");
        RuleFor(x => x.BirthDate).NotEmpty().WithMessage("BirthDate is required");
        RuleFor(x => x.IsAdmin).NotEmpty().WithMessage("IsAdmin is required");
    }
}