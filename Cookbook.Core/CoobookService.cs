using System.Security.Claims;
using Cookbook.Data.Repositories;
using Cookbook.SharedModels.Entities;

namespace Cookbook.Core
{
    public class CookbookService(
        IRecipeRepository recipeRepository,
        ICategoryRepository categoryRepository,
        IIngredientRepository ingredientRepository,
        IUserRepository userRepository) : ICookbookService
    {
        #region CRUD for Recipes

        public async Task<IEnumerable<Recipe>> GetAllRecipesAsync()
        {
            var recipes = await recipeRepository.GetAllAsync();

            return recipes;
        }

        public async Task<Recipe?> GetRecipeByAsync(int id)
        {
            var recipe = await recipeRepository.GetByAsync(id);

            return recipe;
        }

        public async Task<Recipe> CreateRecipeAsync(Recipe recipe)
        {
            recipe.CreatorId = 1; // TODO replace with current user id

            return await recipeRepository.CreateAsync(recipe);
        }

        public async Task<Recipe?> ModifyRecipeAsync(int id, Recipe recipe)
        {
            return await recipeRepository.ModifyAsync(recipe);
        }

        public async Task<bool> DeleteRecipeAsync(int id)
        {
            return await recipeRepository.DeleteAsync(id);
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

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await userRepository.GetByUsernameAsync(username);
        }

        public async Task<User> CreateUserAsync(User user)
        {
            return await userRepository.CreateAsync(user);
        }

        public async Task<User?> ModifyUserAsync(int id, User user)
        {
            return await userRepository.ModifyAsync(user);
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            return await userRepository.DeleteAsync(id);
        }

        #endregion

        #region CRUD for Ingredients

        public async Task<IEnumerable<Ingredient>> GetAllIngredientsAsync()
        {
            return await ingredientRepository.GetAllAsync();
        }

        public async Task<Ingredient?> GetIngredientByAsync(int id)
        {
            return await ingredientRepository.GetByAsync(id);
        }

        public async Task<Ingredient> CreateIngredientAsync(Ingredient ingredient)
        {
            return await ingredientRepository.CreateAsync(ingredient);
        }

        public async Task<Ingredient?> ModifyIngredientAsync(int id, Ingredient ingredient)
        {
            return await ingredientRepository.ModifyAsync(ingredient);
        }

        public async Task<bool> DeleteIngredientAsync(int id)
        {
            return await ingredientRepository.DeleteAsync(id);
        }

        #endregion

        #region CRUD for Categories

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await categoryRepository.GetAllAsync();
        }

        public async Task<Category?> GetCategoryByAsync(int id)
        {
            return await categoryRepository.GetByAsync(id);
        }

        public async Task<Category> CreateCategoryAsync(Category category)
        {
            return await categoryRepository.CreateAsync(category);
        }

        public async Task<Category?> ModifyCategoryAsync(int id, Category category)
        {
            return await categoryRepository.ModifyAsync(category);
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            return await categoryRepository.DeleteAsync(id);
        }

        #endregion
    }
}