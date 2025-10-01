using System.Security.Claims;
using System.Text;
using Cookbook.Core;
using Cookbook.Data;
using Cookbook.SharedData;
using Cookbook.SharedData.Contracts.Requests;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Cookbook.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddValidatorsFromAssemblyContaining<SignInUserRequest>();

        if (TryBuildSettings<IJwtSettings, JwtSettings, JwtSettingsValidator>(builder, "JwtSettings",
                out var jwtSettings))
            builder.Services.AddSingleton<IJwtSettings>(jwtSettings);
        else
            return;

        builder.Services.AddDbContext<CookbookContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("RecipeManagement")));

        builder.Services.AddDal();
        builder.Services.AddBll();
        builder.Services.AddControllers(options =>
        {
            var policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
            options.Filters.Add(new AuthorizeFilter(policy));
        });

        builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),

                    RoleClaimType = ClaimTypes.Role
                };
            });

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddEndpointsApiExplorer();

        var app = builder.Build();
        app.UseMiddleware<GlobalExceptionMiddleware>();


        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            var context = services.GetRequiredService<CookbookContext>();
            context.Database.EnsureCreated();
        }

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }

    private static bool TryBuildSettings<TService, TImplementation, TValidator>(
        WebApplicationBuilder builder,
        string sectionName,
        out TImplementation settings)
        where TService : class
        where TImplementation : class, TService, new()
        where TValidator : AbstractValidator<TImplementation>, new()
    {
        settings = new TImplementation();
        builder.Configuration.GetSection(sectionName).Bind(settings);


        using var loggerFactory = LoggerFactory.Create(logging =>
        {
            logging.AddConsole();
            logging.AddDebug();
            logging.SetMinimumLevel(LogLevel.Information);
        });
        var logger = loggerFactory.CreateLogger("Startup");


        var validator = new TValidator();
        var result = validator.Validate(settings);

        if (result.IsValid)
        {
            logger.LogInformation("Configuration {SectionName} loaded and valid", sectionName);
        }
        else
        {
            logger.LogError("Invalid configuration in '{SectionName}'", sectionName);
            foreach (var error in result.Errors)
                logger.LogError(" - {Property}: {ErrorMessage}", error.PropertyName, error.ErrorMessage);

            settings = null;
        }

        return result.IsValid;
    }
}