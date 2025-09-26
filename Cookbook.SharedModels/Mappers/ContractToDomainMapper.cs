using Cookbook.SharedModels.Contracts.Requests;
using Cookbook.SharedModels.Entities;

namespace Cookbook.SharedModels.Mappers
{
    public static class ContractToDomainMapper
    {
        public static Recipe ToRecipe(this CreateRecipeRequest createRecipeRequest)
        {
            return new Recipe
            {
                RecipeId = 0, // Will be set in the service layer
                Title = createRecipeRequest.Title,
                PreparationDuration = createRecipeRequest.Steps
                    .Where(s => !s.IsCooking)
                    .Select(s => s.Duration)
                    .Average(),
                CookingDuration = createRecipeRequest.Steps
                    .Where(s => s.IsCooking)
                    .Select(s => s.Duration)
                    .Average(),
                Difficulty = createRecipeRequest.Difficulty,
                ImagePath = createRecipeRequest.ImagePath,
                CreatorId = 0, // Will be set in the service layer
                Categories = createRecipeRequest.Categories
                    .Select(c => new Category { CategoryId = c.CategoryId }).ToList(),
                RecipesIngredients = createRecipeRequest.Ingredients.Select(i => new RecipesIngredient
                {
                    IngredientId = i.IngredientId,
                    Quantity = i.Quantity,
                    Unit = i.Unit
                }).ToList(),
                Steps = createRecipeRequest.Steps.OrderBy(s => s.StepNumber).Select((s, index) => new Step
                {
                    StepNumber = (short)index,
                    Instruction = s.Instruction,
                    Duration = s.Duration,
                    IsCooking = s.IsCooking
                }).ToList()
            };
        }

        public static Recipe ToRecipe(this UpdateRecipeRequest updateRecipeRequest)
        {
            return new Recipe
            {
                RecipeId = 0, // Will be set in the service layer
                Title = updateRecipeRequest.Title,
                PreparationDuration = updateRecipeRequest.Steps
                    .Where(s => !s.IsCooking)
                    .Select(s => s.Duration)
                    .Average(),
                CookingDuration = updateRecipeRequest.Steps
                    .Where(s => s.IsCooking)
                    .Select(s => s.Duration)
                    .Average(),
                Difficulty = updateRecipeRequest.Difficulty,
                ImagePath = updateRecipeRequest.ImagePath ?? string.Empty,
                CreatorId = 0, // Will be set in the service layer
                Categories = updateRecipeRequest.Categories
                                        ?.Select(c => new Category { CategoryId = c.CategoryId }).ToList() ??
                                    [],
                RecipesIngredients = updateRecipeRequest.Ingredients?.Select(i => new RecipesIngredient
                {
                    IngredientId = i.IngredientId,
                    Quantity = i.Quantity,
                    Unit = i.Unit ?? string.Empty
                }).ToList() ?? [],
                Steps = updateRecipeRequest.Steps?.OrderBy(s => s.StepNumber).Select((s, index) => new Step
                {
                    StepNumber = (short)index,
                    Instruction = s.Instruction ?? string.Empty,
                    Duration = s.Duration,
                    IsCooking = s.IsCooking
                }).ToList() ?? []
            };
        }

        public static User ToUser(this SignInUserRequest signInUserRequest)
        {
            return new User()
            {
                Username = signInUserRequest.Username,
                PasswordHash = signInUserRequest.Password
            };
        }
        public static User ToUser(this SignUpUserRequest signUpUserRequest)
        {
            return new User()
            {
                Username = signUpUserRequest.Username,
                Email = signUpUserRequest.Email,
                BirthDate = signUpUserRequest.BirthDate,
                PasswordHash = signUpUserRequest.Password
            };
        }

        private static TimeSpan Average(this IEnumerable<TimeSpan> source)
        {
            var timeSpans = source.ToList();
            if (timeSpans.Count == 0)
                return TimeSpan.Zero;
            var totalTicks = timeSpans.Sum(ts => ts.Ticks);
            var averageTicks = totalTicks / timeSpans.Count;
            return new TimeSpan(averageTicks);
        }
    }
}