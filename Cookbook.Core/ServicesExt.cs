using Microsoft.Extensions.DependencyInjection;

namespace Cookbook.Core;

public static class ServicesExt
{
    public static void AddBll(this IServiceCollection services)
    {
        // Services are typically Scoped
        services.AddScoped<ICookbookService, CookbookService>();
        services.AddScoped<IAccessService, AccessService>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
    }
}