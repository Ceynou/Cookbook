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
                    Instruction = s.Instruction,
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

        public static Category ToCategory(this CreateCategoryRequest createCategoryRequest)
        {
            return new Category()
            {
                Name = createCategoryRequest.Name,
            };
        }

        public static Category ToCategory(this UpdateCategoryRequest updateCategoryRequest)
        {
            return new Category()
            {
                Name = updateCategoryRequest.Name
            };
        }

        public static Ingredient ToIngredient(this CreateIngredientRequest createIngredientRequest)
        {
            return new Ingredient()
            {
                Name = createIngredientRequest.Name
            };
        }

        public static Ingredient ToIngredient(this UpdateIngredientRequest updateIngredientRequest)
        {
            return new Ingredient()
            {
                Name = updateIngredientRequest.Name
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