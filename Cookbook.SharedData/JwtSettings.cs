using FluentValidation;

namespace Cookbook.SharedData;

public class JwtSettings : IJwtSettings
{
    public string Secret { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public int ExpirationMinutes { get; set; }
}

public class JwtSettingsValidator : AbstractValidator<JwtSettings>
{
    public JwtSettingsValidator()
    {
        const int minSecretLength = 32;
        RuleFor(x => x.Secret)
            .NotNull().NotEmpty().WithMessage("JWT secret is required")
            .MinimumLength(minSecretLength)
            .WithMessage($"JWT secret length must be at least {minSecretLength} symbols");

        RuleFor(x => x.Issuer)
            .NotNull().NotEmpty().WithMessage("Issuer is required");

        RuleFor(x => x.Audience)
            .NotNull().NotEmpty().WithMessage("Audience is required");

        RuleFor(x => x.ExpirationMinutes)
            .GreaterThan(0).WithMessage("Expiration time must be greater than 0");
    }
}