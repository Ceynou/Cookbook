using Cookbook.SharedData.Contracts.Responses;
using Cookbook.SharedData.Entities;

namespace Cookbook.SharedData.Mappers;

public static class DomainToContractMapper
{
		public static RecipeResponse ToRecipeResponse(this Recipe recipe)
		{
				return new RecipeResponse
				{
						RecipeId = recipe.RecipeId,
						Title = recipe.Title,
						PreparationDuration = recipe.PreparationDuration,
						CookingDuration = recipe.CookingDuration,
						Difficulty = recipe.Difficulty,
						ImagePath = recipe.ImagePath,
						CreatorId = recipe.CreatorId ?? 0,
						CreatorUsername = recipe.Creator?.Username ?? string.Empty,
						CreatorProfilePicture = recipe.Creator?.ImagePath ?? string.Empty,
						IngredientsCount = recipe.RecipesIngredients.Count,
						StepsCount = recipe.Steps.Count,
						ReviewsCount = recipe.Reviews.Count,
						ReviewRating = recipe.Reviews.Count > 0 ? recipe.Reviews.Average(r => r.Rating) : 0
				};
		}

		public static RecipeDetailResponse ToRecipeDetailResponse(this Recipe recipe)
		{
				return new RecipeDetailResponse
				{
						RecipeId = recipe.RecipeId,
						Title = recipe.Title,
						PreparationDuration = recipe.PreparationDuration,
						CookingDuration = recipe.CookingDuration,
						Difficulty = recipe.Difficulty,
						ImagePath = recipe.ImagePath,
						CreatorId = recipe.CreatorId ?? 0,
						CreatorUsername = recipe.Creator?.Username ?? string.Empty,
						Ingredients = recipe.RecipesIngredients.Select(i => i.ToRecipeIngredientResponse()),
						Categories = recipe.RecipesCategories.Select(c => c.ToRecipeCategoryResponse()),
						Steps = recipe.Steps.Select(s => s.ToStepResponse()),
						Reviews = recipe.Reviews.Select(r => r.ToReviewResponse()),
						ReviewsCount = recipe.Reviews.Count,
						ReviewRating = recipe.Reviews.Count != 0 ? recipe.Reviews.Average(r => r.Rating) : 0
				};
		}

		private static StepResponse ToStepResponse(this Step step)
		{
				return new StepResponse
				{
						StepNumber = step.StepNumber,
						Instruction = step.Instruction,
						Duration = step.Duration,
						IsCooking = step.IsCooking
				};
		}

		private static ReviewResponse ToReviewResponse(this Review review)
		{
				return new ReviewResponse
				{
						ReviewerId = review.ReviewerId,
						Username = review.Reviewer.Username,
						Rating = review.Rating,
						Impression = review.Impression
				};
		}

		public static RecipeIngredientResponse ToRecipeIngredientResponse(this RecipeIngredient recipeIngredient)
		{
				return new RecipeIngredientResponse
				{
						Name = recipeIngredient.Ingredient.Name,
						IngredientId = recipeIngredient.IngredientId,
						Quantity = recipeIngredient.Quantity,
						Unit = recipeIngredient.Unit
				};
		}

		public static RecipeCategoryResponse ToRecipeCategoryResponse(this RecipeCategory recipeCategory)
		{
				return new RecipeCategoryResponse
				{
						CategoryId = recipeCategory.CategoryId,
						Name = recipeCategory.Category.Name
				};
		}

		public static RecipeCategoryResponse ToCategoryResponse(this RecipeCategory recipeCategory)
		{
				return new RecipeCategoryResponse
				{
						CategoryId = recipeCategory.CategoryId,
						Name = recipeCategory.Category.Name
				};
		}

		public static CategoryResponse ToCategoryResponse(this Category category)
		{
				return new CategoryResponse
				{
						CategoryId = category.CategoryId,
						Name = category.Name
				};
		}

		public static IngredientResponse ToIngredientResponse(this Ingredient ingredient)
		{
				return new IngredientResponse
				{
						IngredientId = ingredient.IngredientId,
						Name = ingredient.Name
				};
		}
}