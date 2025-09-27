using Cookbook.SharedModels.Entities;

namespace Cookbook.Core
{
	public interface ICookbookService
	{
		#region CRUD for recipes
		Task<IEnumerable<Recipe>> GetAllRecipesAsync();
		Task<Recipe?> GetRecipeByAsync(int id);
		Task<Recipe> CreateRecipeAsync(Recipe recipe);
		Task<Recipe?> ModifyRecipeAsync(int id, Recipe recipe);
		Task<bool> DeleteRecipeAsync(int id);

		#endregion
		
		#region CRUD for users
		Task<IEnumerable<User>> GetAllUsersAsync();
		Task<User?> GetUserByAsync(int id);
		Task<User?> GetUserByUsernameAsync(string username);
		Task<User> CreateUserAsync(User user);
		Task<User?> ModifyUserAsync(int id, User user);
		Task<bool> DeleteUserAsync(int id);
		
		#endregion
		
		#region CRUD for categories

		Task<IEnumerable<Category>> GetAllCategoriesAsync();
		Task<Category?> GetCategoryByAsync(int id);
		Task<Category> CreateCategoryAsync(Category category);
		Task<Category?> ModifyCategoryAsync(int id, Category category);
		Task<bool> DeleteCategoryAsync(int id);

		#endregion
		
		
		#region CRUD for ingredients
		
		Task<IEnumerable<Ingredient>> GetAllIngredientsAsync();
		Task<Ingredient?> GetIngredientByAsync(int id);
		Task<Ingredient> CreateIngredientAsync(Ingredient ingredient);
		Task<Ingredient?> ModifyIngredientAsync(int id, Ingredient ingredient);
		Task<bool> DeleteIngredientAsync(int id);

		#endregion
	}
}
