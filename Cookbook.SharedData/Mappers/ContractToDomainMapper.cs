using Cookbook.SharedData.Contracts.Requests;
using Cookbook.SharedData.Contracts.Responses;
using Cookbook.SharedData.Entities;

namespace Cookbook.SharedData.Mappers;

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
						RecipesCategories = createRecipeRequest.Categories
								.Select(c => c.ToRecipesCategory())
								.ToList(),
						RecipesIngredients = createRecipeRequest.Ingredients.Select(i => i.ToRecipesIngredient()).ToList(),
						Steps = createRecipeRequest.Steps.OrderBy(s => s.StepNumber).Select((s, index) => new Step
						{
								StepNumber = (short)(index + 1),
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
						RecipesCategories = updateRecipeRequest.Categories
								.Select(c => c.ToRecipesCategory())
								.ToList(),
						RecipesIngredients = updateRecipeRequest.Ingredients
								.Select(i => i.ToRecipesIngredient()).ToList(),
						Steps = updateRecipeRequest.Steps.OrderBy(s => s.StepNumber)
								.Select((s, index) => new Step
								{
										StepNumber = (short)(index + 1),
										Instruction = s.Instruction,
										Duration = s.Duration,
										IsCooking = s.IsCooking
								}).ToList()
				};
		}

		public static User ToUser(this SignInUserRequest signInUserRequest)
		{
				return new User
				{
						Username = signInUserRequest.Username,
						PasswordHash = signInUserRequest.Password
				};
		}

		public static User ToUser(this SignUpUserRequest signUpUserRequest)
		{
				return new User
				{
						Username = signUpUserRequest.Username,
						Email = signUpUserRequest.Email,
						PasswordHash = signUpUserRequest.Password
				};
		}

		public static Category ToCategory(this CreateCategoryRequest createCategoryRequest)
		{
				return new Category
				{
						Name = createCategoryRequest.Name
				};
		}

		public static Category ToCategory(this UpdateCategoryRequest updateCategoryRequest)
		{
				return new Category
				{
						Name = updateCategoryRequest.Name
				};
		}

		public static Category ToCategory(this CategoryResponse categoryResponse)
		{
				return new Category
				{
						CategoryId = categoryResponse.CategoryId,
						Name = categoryResponse.Name
				};
		}

		public static RecipeCategory ToRecipesCategory(this CreateRecipeCategoryRequest createRecipeCategoryRequest)
		{
				return new RecipeCategory
				{
						CategoryId = createRecipeCategoryRequest.CategoryId
				};
		}

		public static RecipeCategory ToRecipesCategory(this UpdateRecipeCategoryRequest updateRecipeCategoryRequest)
		{
				return new RecipeCategory
				{
						CategoryId = updateRecipeCategoryRequest.CategoryId
				};
		}

		public static Ingredient ToIngredient(this CreateIngredientRequest createIngredientRequest)
		{
				return new Ingredient
				{
						Name = createIngredientRequest.Name
				};
		}

		public static Ingredient ToIngredient(this UpdateIngredientRequest updateIngredientRequest)
		{
				return new Ingredient
				{
						Name = updateIngredientRequest.Name
				};
		}

		public static RecipeIngredient ToRecipesIngredient(
				this CreateRecipeIngredientRequest createRecipeIngredientRequest)
		{
				return new RecipeIngredient
				{
						IngredientId = createRecipeIngredientRequest.IngredientId,
						Quantity = createRecipeIngredientRequest.Quantity,
						Unit = createRecipeIngredientRequest.Unit
				};
		}

		public static RecipeIngredient ToRecipesIngredient(
				this UpdateRecipeIngredientRequest updateRecipeIngredientRequest)
		{
				return new RecipeIngredient
				{
						IngredientId = updateRecipeIngredientRequest.IngredientId,
						Quantity = updateRecipeIngredientRequest.Quantity,
						Unit = updateRecipeIngredientRequest.Unit
				};
		}

		public static Step ToStep(this CreateStepRequest createStepRequest)
		{
				return new Step
				{
						Instruction = createStepRequest.Instruction,
						Duration = createStepRequest.Duration,
						IsCooking = createStepRequest.IsCooking
				};
		}

		public static Step ToStep(this UpdateStepRequest updateStepRequest)
		{
				return new Step
				{
						Instruction = updateStepRequest.Instruction,
						Duration = updateStepRequest.Duration,
						IsCooking = updateStepRequest.IsCooking
				};
		}

		public static Recipe ToRecipe(this Contracts.Responses.RecipeResponse recipeResponse)
		{
				return new Recipe()
				{
						RecipeId = recipeResponse.RecipeId,
						CookingDuration = recipeResponse.CookingDuration,
						CreatorId = recipeResponse.CreatorId,
						Difficulty = recipeResponse.Difficulty,
						ImagePath = recipeResponse.ImagePath ?? string.Empty,
						PreparationDuration = recipeResponse.PreparationDuration,
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