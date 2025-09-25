using Cookbook.Data.Interfaces;
using Cookbook.Data.Repositories;
using Cookbook.SharedModels.Entities;

namespace Cookbook.Core
{
	public class CookbookService(IRecipeRepository recipeRepository, IUserRepository userRepository) : ICookbookService
	{
		
		#region CRUD for Recipes
		public async Task<IEnumerable<Recipe>> GetAllRecipesAsync()
		{
			IEnumerable<Recipe> recipes = await recipeRepository.GetAllAsync();

			return recipes;
		}

		public async Task<Recipe?> GetRecipeByAsync(int id)
		{
			var recipe = await recipeRepository.GetByAsync(id);

			return recipe;
		}

		public async Task<Recipe?> CreateRecipeAsync(Recipe recipe)
		{
			recipe.CreatorId = 1;

			await recipeRepository.CreateAsync(recipe);
			return await recipeRepository.GetByAsync(recipe.RecipeId);
		}

		public async Task<Recipe?> ModifyRecipeAsync(int id, Recipe recipe)
		{
			await recipeRepository.ModifyAsync(recipe);
			return await recipeRepository.GetByAsync(id);
		}

		public async Task<bool> DeleteRecipeAsync(int id)
		{
			return await recipeRepository.DeleteAsync(id);;
		}

		#endregion

		#region CRUD for Users
		public async Task<IEnumerable<User>> GetAllUsersAsync()
		{
			return await userRepository.GetAllAsync();
		}

		public async Task<User?> GetUserByAsync(int id)
		{
			return await userRepository.GetByAsync(id);
		}

		public async Task<User?> GetUserByAsync(User user)
		{
			return await  userRepository.GetByAsync(user);
		}

		public Task<User> CreateUserAsync(User user)
		{
			throw new NotImplementedException();
		}

		public Task<User?> ModifyUserAsync(int id, User user)
		{
			throw new NotImplementedException();
		}

		public Task<bool> DeleteUserAsync(int id)
		{
			throw new NotImplementedException();
		}
		
		#endregion
	}
}
