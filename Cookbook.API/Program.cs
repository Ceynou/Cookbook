using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using Cookbook.API;
using Cookbook.Core;
using Cookbook.Infrastructure;
using Cookbook.SharedData.Contracts.Requests;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

[assembly: InternalsVisibleTo("Cookbook.API.UnitTests")]
[assembly: InternalsVisibleTo("Cookbook.API.IntegrationTests")]


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddValidatorsFromAssemblyContaining<SignInUserRequest>();


builder.Services.AddDbContext<CookbookContext>(options => options
    .UseNpgsql(builder.Configuration.GetConnectionString("CookbookDB"))
    .UseAsyncSeeding(async (dbContext, _, cancellationToken) =>
    {
        var context = (CookbookContext)dbContext;
        await context.SeedAsync(context, cancellationToken);
    }));


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

            ValidIssuer = builder.Configuration.GetValue<string>("JwtIssuer"),
            ValidAudience = builder.Configuration.GetValue<string>("JwtAudience"),
            IssuerSigningKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("JwtSecret") ??
                                                                throw new InvalidOperationException())),
            RoleClaimType = ClaimTypes.Role
        };
    });

builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();
app.UseMiddleware<GlobalExceptionMiddleware>();


using (var serviceScope = app.Services.CreateScope())
{
    await using (var context = serviceScope.ServiceProvider.GetRequiredService<CookbookContext>())
    {
        if (!app.Environment.IsDevelopment())
        {
            await context.Database.MigrateAsync();
        }
        else
        {
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();
        }
    }
}

app.UseAuthorization();


app.MapControllers();

app.Run();