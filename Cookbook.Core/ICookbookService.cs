using Cookbook.Data.Repositories;
using Cookbook.SharedData;
using Cookbook.SharedData.Entities;

namespace Cookbook.Core;

public interface ICookbookService
{
    #region CRUD for recipes

    /// <summary>
    /// Fetches all the recipes from the database with additional stats, such as the number of ingredients, of steps, of reviews and the average rating and returns a collection of <see cref="Recipe"/>.
    /// </summary>
    /// <remarks>
    /// <para>GlobalExceptionMiddleware handles the exception and returns a NotFoundResult response with error details.</para>
    /// </remarks>
    /// <exception cref="ResourceNotFoundException">thrown if the repository returns a null.</exception>
    /// <returns>A collection of <see cref="Recipe"/>.</returns>
    Task<IEnumerable<Recipe>> GetAllRecipesAsync();

    /// <summary>
    /// Fetches one <see cref="Recipe"/> by its primary key and returns it.
    /// </summary>
    /// <param name="id">the primary key</param>
    /// <remarks>
    /// <para>GlobalExceptionMiddleware handles the exception and returns a NotFoundResult response with error details.</para>
    /// </remarks>
    /// <exception cref="ResourceNotFoundException">thrown if the repository returns a null.</exception>
    /// <returns>One <see cref="Recipe"/>.</returns>
    Task<Recipe> GetRecipeByAsync(int id);

    /// <summary>
    /// Adds one <see cref="Recipe"/> into the database and returns the created <see cref="Recipe"/>.
    /// </summary>
    /// <param name="recipe">the recipe entity with the required properties.</param>
    /// <remarks>
    /// <para>GlobalExceptionMiddleware handles the exception and returns a NotFoundResult response with error details.</para>
    /// </remarks>
    /// <exception cref="ResourceNotFoundException">thrown if the repository returns a null.</exception>
    /// <returns>The <see cref="Recipe"/> from the database.</returns>
    Task<Recipe> CreateRecipeAsync(Recipe recipe);

    /// <summary>
    /// Modifies one <see cref="Recipe"/> from the database and returns the modified <see cref="Recipe"/>.
    /// </summary>
    /// <param name="id">the primary key.</param>
    /// <param name="recipe">the recipe entity with the required properties.</param>
    /// <remarks>
    /// <para>GlobalExceptionMiddleware handles the exception and returns a NotFoundResult response with error details.</para>
    /// </remarks>
    /// <exception cref="ResourceNotFoundException">thrown if the repository returns a null.</exception>
    /// <returns>The <see cref="Recipe"/> from the database.</returns>
    Task<Recipe> ModifyRecipeAsync(int id, Recipe recipe);

    /// <summary>
    /// Deletes one <see cref="Recipe"/> from the database and returns a boolean.
    /// </summary>
    /// <param name="id">the primary key.</param>
    /// <remarks>
    /// <para>GlobalExceptionMiddleware handles the exception and returns a NotFoundResult response with error details.</para>
    /// </remarks>
    /// <exception cref="ResourceNotFoundException">thrown if the repository returns a null.</exception>
    /// <returns>A Task representing the asynchronous operation.</returns>
    Task DeleteRecipeAsync(int id);

    #endregion

    #region CRUD for users

    /// <summary>
    /// Fetches all the <see cref="User"/> entities from the database.
    /// </summary>
    /// <remarks>
    /// <para>GlobalExceptionMiddleware handles the exception and returns a NotFoundResult response with error details.</para>
    /// </remarks>
    /// <exception cref="ResourceNotFoundException">thrown if the repository returns an empty collection.</exception>
    /// <returns>A collection of <see cref="User"/>.</returns>
    Task<IEnumerable<User>> GetAllUsersAsync();

    /// <summary>
    /// Fetches one <see cref="User"/> by its primary key.
    /// </summary>
    /// <param name="id">the primary key.</param>
    /// <remarks>
    /// <para>GlobalExceptionMiddleware handles the exception and returns a NotFoundResult response with error details.</para>
    /// </remarks>
    /// <exception cref="ResourceNotFoundException">thrown if the repository returns a null.</exception>
    /// <returns>One <see cref="User"/>.</returns>
    Task<User> GetUserByAsync(int id);

    /// <summary>
    /// Fetches one <see cref="User"/> by their username.
    /// </summary>
    /// <param name="username">the username.</param>
    /// <remarks>
    /// <para>GlobalExceptionMiddleware handles the exception and returns a NotFoundResult response with error details.</para>
    /// </remarks>
    /// <exception cref="ResourceNotFoundException">thrown if the repository returns a null.</exception>
    /// <returns>One <see cref="User"/>.</returns>
    Task<User> GetUserByUsernameAsync(string username);

    /// <summary>
    /// Adds one <see cref="User"/> into the database and returns the created <see cref="User"/>.
    /// </summary>
    /// <param name="user">the user entity with the required properties.</param>
    /// <remarks>
    /// <para>GlobalExceptionMiddleware handles the exception and returns a NotFoundResult response with error details.</para>
    /// </remarks>
    /// <exception cref="ResourceNotFoundException">thrown if the repository returns a null.</exception>
    /// <returns>The <see cref="User"/> from the database.</returns>
    Task<User> CreateUserAsync(User user);

    /// <summary>
    /// Modifies one <see cref="User"/> from the database and returns the modified <see cref="User"/>.
    /// </summary>
    /// <param name="id">the primary key.</param>
    /// <param name="user">the user entity with the required properties.</param>
    /// <remarks>
    /// <para>GlobalExceptionMiddleware handles the exception and returns a NotFoundResult response with error details.</para>
    /// </remarks>
    /// <exception cref="ResourceNotFoundException">thrown if the repository returns a null.</exception>
    /// <returns>The <see cref="User"/> from the database.</returns>
    Task<User> ModifyUserAsync(int id, User user);

    /// <summary>
    /// Deletes one <see cref="User"/> from the database.
    /// </summary>
    /// <param name="id">the primary key.</param>
    /// <remarks>
    /// <para>GlobalExceptionMiddleware handles the exception and returns a NotFoundResult response with error details.</para>
    /// </remarks>
    /// <exception cref="ResourceNotFoundException">thrown if the repository returns false.</exception>
    /// <returns>A Task representing the asynchronous operation.</returns>
    Task DeleteUserAsync(int id);

    #endregion

    #region CRUD for categories

    /// <summary>
    /// Fetches all the <see cref="Category"/> entities from the database.
    /// </summary>
    /// <remarks>
    /// <para>GlobalExceptionMiddleware handles the exception and returns a NotFoundResult response with error details.</para>
    /// </remarks>
    /// <exception cref="ResourceNotFoundException">thrown if the repository returns an empty collection.</exception>
    /// <returns>A collection of <see cref="Category"/>.</returns>
    Task<IEnumerable<Category>> GetAllCategoriesAsync();

    /// <summary>
    /// Fetches one <see cref="Category"/> by its primary key.
    /// </summary>
    /// <param name="id">the primary key.</param>
    /// <remarks>
    /// <para>GlobalExceptionMiddleware handles the exception and returns a NotFoundResult response with error details.</para>
    /// </remarks>
    /// <exception cref="ResourceNotFoundException">thrown if the repository returns a null.</exception>
    /// <returns>One <see cref="Category"/>.</returns>
    Task<Category> GetCategoryByAsync(int id);

    /// <summary>
    /// Adds one <see cref="Category"/> into the database and returns the created <see cref="Category"/>.
    /// </summary>
    /// <param name="category">the category entity with the required properties.</param>
    /// <remarks>
    /// <para>GlobalExceptionMiddleware handles the exception and returns a NotFoundResult response with error details.</para>
    /// </remarks>
    /// <exception cref="ResourceNotFoundException">thrown if the repository returns a null.</exception>
    /// <returns>The <see cref="Category"/> from the database.</returns>
    Task<Category> CreateCategoryAsync(Category category);

    /// <summary>
    /// Modifies one <see cref="Category"/> from the database and returns the modified <see cref="Category"/>.
    /// </summary>
    /// <param name="id">the primary key.</param>
    /// <param name="category">the category entity with the required properties.</param>
    /// <remarks>
    /// <para>GlobalExceptionMiddleware handles the exception and returns a NotFoundResult response with error details.</para>
    /// </remarks>
    /// <exception cref="ResourceNotFoundException">thrown if the repository returns a null.</exception>
    /// <returns>The <see cref="Category"/> from the database.</returns>
    Task<Category> ModifyCategoryAsync(int id, Category category);

    /// <summary>
    /// Deletes one <see cref="Category"/> from the database.
    /// </summary>
    /// <param name="id">the primary key.</param>
    /// <remarks>
    /// <para>GlobalExceptionMiddleware handles the exception and returns a NotFoundResult response with error details.</para>
    /// </remarks>
    /// <exception cref="ResourceNotFoundException">thrown if the repository returns false.</exception>
    /// <returns>A Task representing the asynchronous operation.</returns>
    Task DeleteCategoryAsync(int id);

    #endregion

    #region CRUD for ingredients

    /// <summary>
    /// Fetches all the <see cref="Ingredient"/> entities from the database.
    /// </summary>
    /// <remarks>
    /// <para>GlobalExceptionMiddleware handles the exception and returns a NotFoundResult response with error details.</para>
    /// </remarks>
    /// <exception cref="ResourceNotFoundException">thrown if the repository returns an empty collection.</exception>
    /// <returns>A collection of <see cref="Ingredient"/>.</returns>
    Task<IEnumerable<Ingredient>> GetAllIngredientsAsync();

    /// <summary>
    /// Fetches one <see cref="Ingredient"/> by its primary key.
    /// </summary>
    /// <param name="id">the primary key.</param>
    /// <remarks>
    /// <para>GlobalExceptionMiddleware handles the exception and returns a NotFoundResult response with error details.</para>
    /// </remarks>
    /// <exception cref="ResourceNotFoundException">thrown if the repository returns a null.</exception>
    /// <returns>One <see cref="Ingredient"/>.</returns>
    Task<Ingredient> GetIngredientByAsync(int id);

    /// <summary>
    /// Adds one <see cref="Ingredient"/> into the database and returns the created <see cref="Ingredient"/>.
    /// </summary>
    /// <param name="ingredient">the ingredient entity with the required properties.</param>
    /// <remarks>
    /// <para>GlobalExceptionMiddleware handles the exception and returns a NotFoundResult response with error details.</para>
    /// </remarks>
    /// <exception cref="ResourceNotFoundException">thrown if the repository returns a null.</exception>
    /// <returns>The <see cref="Ingredient"/> from the database.</returns>
    Task<Ingredient> CreateIngredientAsync(Ingredient ingredient);

    /// <summary>
    /// Modifies one <see cref="Ingredient"/> from the database and returns the modified <see cref="Ingredient"/>.
    /// </summary>
    /// <param name="id">the primary key.</param>
    /// <param name="ingredient">the ingredient entity with the required properties.</param>
    /// <remarks>
    /// <para>GlobalExceptionMiddleware handles the exception and returns a NotFoundResult response with error details.</para>
    /// </remarks>
    /// <exception cref="ResourceNotFoundException">thrown if the repository returns a null.</exception>
    /// <returns>The <see cref="Ingredient"/> from the database.</returns>
    Task<Ingredient> ModifyIngredientAsync(int id, Ingredient ingredient);

    /// <summary>
    /// Deletes one <see cref="Ingredient"/> from the database.
    /// </summary>
    /// <param name="id">the primary key.</param>
    /// <remarks>
    /// <para>GlobalExceptionMiddleware handles the exception and returns a NotFoundResult response with error details.</para>
    /// </remarks>
    /// <exception cref="ResourceNotFoundException">thrown if the repository returns false.</exception>
    /// <returns>A Task representing the asynchronous operation.</returns>
    Task DeleteIngredientAsync(int id);

    #endregion
}