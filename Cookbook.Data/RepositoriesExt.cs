using Cookbook.Data.Interfaces;
using Cookbook.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Cookbook.Data
{
    public static class RepositoriesExt
    {
        public static void AddDal(this IServiceCollection services)
        {
            services.AddTransient<IRecipeRepository, RecipeRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
        }
    }
}
