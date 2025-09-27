using Cookbook.SharedModels.Contracts.Responses;
using Cookbook.SharedModels.Entities;

namespace Cookbook.SharedModels.Mappers
{
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
				CreatorId = recipe.CreatorId ?? 0, // TODO Problem?
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
				CreatorId = recipe.CreatorId ?? 0, // TODO Problem?
				CreatorUsername = recipe.Creator?.Username ?? string.Empty,
				Ingredients = recipe.RecipesIngredients.Select(i => i.ToRecipeIngredientResponse()),
				Categories = recipe.Categories.Select(c => c.ToRecipeCategoryResponse()),
				Steps = recipe.Steps.Select(s => s.ToStepResponse()),
				Reviews = recipe.Reviews.Select(r => r.ToReviewResponse()),
				ReviewsCount = recipe.Reviews.Count,
				ReviewRating = recipe.Reviews.Count != 0 ? recipe.Reviews.Average(r => r.Rating) : 0
			};
		}

		public static StepResponse ToStepResponse(this Step step)
		{
			return new StepResponse
			{
				StepNumber = step.StepNumber,
				Instruction = step.Instruction,
				Duration = step.Duration,
				IsCooking = step.IsCooking
			};
		}

		public static ReviewResponse ToReviewResponse(this Review review)
		{
			return new ReviewResponse
			{
				ReviewerId = review.ReviewerId,
				Username = review.Reviewer?.Username ?? string.Empty,
				Rating = review.Rating,
				Impression = review.Impression
			};
		}

		public static RecipeIngredientResponse ToRecipeIngredientResponse(this RecipesIngredient recipeIngredient)
		{
			return new RecipeIngredientResponse
			{
				IngredientId = recipeIngredient.IngredientId,
				Quantity = recipeIngredient.Quantity,
				Unit = recipeIngredient.Unit
			};
		}

		public static RecipeCategoryResponse ToRecipeCategoryResponse(this Category recipeCategory)
		{
			return new RecipeCategoryResponse
			{
				CategoryId = recipeCategory.CategoryId,
			};
		}

		public static SignInUserResponse ToSignInUserResponse(this User user)
		{
			return new SignInUserResponse()
			{
				Username = user.Username,
				Email = user.Email,
				Token = string.Empty
			};
		}

		public static SignUpUserResponse ToSignUpUserResponse(this User user)
		{
			return new SignUpUserResponse()
			{
				Username = user.Username,
				Email = user.Email,
				Token = string.Empty
			};
		}

		public static CategoryResponse ToCategoryResponse(this Category category)
		{
			return new CategoryResponse()
			{
				CategoryId = category.CategoryId,
				Name = category.Name
			};
		}

		public static IngredientResponse ToIngredientResponse(this Ingredient ingredient)
		{
			return new IngredientResponse()
			{
				IngredientId = ingredient.IngredientId,
				Name = ingredient.Name
			};
		}
	}
}
