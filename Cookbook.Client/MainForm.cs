using System.ComponentModel;
using System.IdentityModel.Tokens.Jwt;
using AntdUI;
using Cookbook.SharedData.Contracts.Requests;
using Cookbook.SharedData.Contracts.Responses;
using Cookbook.SharedData.Mappers;
using Cookbook.SharedData.Entities;
using WTabPage = System.Windows.Forms.TabPage;
using Cookbook.Client.Properties;
using System.Linq;
using Cookbook.SharedData;

namespace Cookbook.Client;

public partial class MainForm : Window
{
		private readonly RestClient _rest;
		private List<WTabPage> _tabPages = [];
		private IEnumerable<string> _roles = [];
		private BindingList<RecipeResponse> _recipes;
		private BindingList<RecipeIngredientResponse> _ingredientsInRecipe;
		private BindingList<CategoryResponse> _categories;
		private BindingList<CategoryResponse> _categoriesInRecipe;
		private BindingList<CategoryResponse> _categoriesNotInRecipe;

		public MainForm()
		{
				Localization.Provider = new Localizer();
				"en-US".SetLanguage();
				_rest = new();
				InitializeComponent();
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
				InitializeBinding();

				_tabPages = tabControl.TabPages.Cast<WTabPage>().ToList();
				textBoxApiUrl.Text = ClientSettings.Default.ApiUrl;
				textBoxUsername.Text = ClientSettings.Default.Username;
				textBoxPassword.Text = ClientSettings.Default.Password;
				TabPagesAuthorizations();
		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
				ClientSettings.Default.ApiUrl = textBoxApiUrl.Text;
				ClientSettings.Default.Username = textBoxUsername.Text;
				ClientSettings.Default.Password = textBoxPassword.Text;
				ClientSettings.Default.Save();
		}

		private async void tabControl_Selecting(object sender, TabControlCancelEventArgs e)
		{
				if (e.TabPage == tabPageAccount)
				{
						AcceptButton = buttonLogin;
				}
				else if (e.TabPage == tabPageRecipes)
				{
						await RefreshRecipes();
						AcceptButton = buttonRecipesRefresh;
				}
				else if (e.TabPage == tabPageCategories)
				{
						await RefreshCategories();
						AcceptButton = buttonCategoriesRefresh;
				}
				else if (e.TabPage == tabPageAssociationManager)
				{
						await RefreshCategories();
						await RefreshRecipes();
						AcceptButton = buttonAssociationRecipesRefresh;
				}
		}

		#region Account

		private async void buttonLogin_Click(object sender, EventArgs e)
		{
				try
				{
						Cursor = Cursors.WaitCursor;

						SignInUserRequest request = new()
						{
								Username = textBoxUsername.Text,
								Password = textBoxPassword.Text
						};

						_rest.BaseUrl = textBoxApiUrl.Text;
						var res = await _rest.PostAsync<JwtResponse, SignInUserRequest>(ClientSettings.Default.UrlSignIn, request);
						if (res is null) return; // Login failed
						_rest.JsonWebToken = res.Token;
						//textBoxPassword.Text = string.Empty;

						_roles = GetRolesFromJwt(res.Token);

						TabPagesAuthorizations();
				}
				finally
				{
						Cursor = Cursors.Default;
				}
		}



		private void buttonLogout_Click(object sender, EventArgs e)
		{
				_rest.JsonWebToken = string.Empty;

				_roles = [];
				_recipes.Clear();
				_ingredientsInRecipe.Clear();
				_categories.Clear();
				_categoriesInRecipe.Clear();
				_categoriesNotInRecipe.Clear();

				textBoxPassword.Text = string.Empty;

				TabPagesAuthorizations();
		}

		private void TabPagesAuthorizations()
		{
				if (string.IsNullOrEmpty(_rest.JsonWebToken))
				{
						foreach (var tab in _tabPages.Where(tab => tab != tabPageAccount))
						{
								tabControl.TabPages.Remove(tab);
						}
				}
				else
				{
						foreach (var tab in _tabPages.Where(tab => !tabControl.TabPages.Contains(tab)))
						{
								if ((tab == tabPageCategories || tab == tabPageAssociationManager) && !_roles.Contains("admin"))
										continue;
								tabControl.TabPages.Add(tab);
						}
				}
		}

		private static IEnumerable<string> GetRolesFromJwt(string token, string[]? possibleClaimTypes = null)
		{
				possibleClaimTypes ??=
				[
						"http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
						"role",
						"roles"
				];

				try
				{
						var handler = new JwtSecurityTokenHandler();
						var jwtToken = handler.ReadJwtToken(token);

						var roles = jwtToken.Claims
								.Where(c => possibleClaimTypes.Contains(c.Type))
								.Select(c => c.Value);

						return roles;
				}
				catch (Exception ex)
				{
						throw new InvalidOperationException($"Error while decoding the token : {ex.Message}");
				}
		}

		private void textBoxApiUrl_KeyPress(object sender, KeyPressEventArgs e)
		{
				if (e.KeyChar == (char)Keys.Return)
						buttonLogin.PerformClick();
		}

		private void textBoxUsername_KeyPress(object sender, KeyPressEventArgs e)
		{
				if (e.KeyChar == (char)Keys.Return)
						buttonLogin.PerformClick();
		}

		private void textBoxPassword_KeyPress(object sender, KeyPressEventArgs e)
		{
				if (e.KeyChar == (char)Keys.Return)
						buttonLogin.PerformClick();
		}

		#endregion Account

		#region Recipes

		private async void buttonRecipesRefresh_Click(object sender, EventArgs e)
		{
				try
				{
						Cursor = Cursors.WaitCursor;
						await RefreshRecipes();
				}
				finally
				{
						Cursor = Cursors.Default;
				}
		}

		private async Task RefreshRecipes()
		{
				var current = bindingSourceRecipes.Current as Recipe;

				var res = await _rest.GetAsync<ICollection<RecipeResponse>>(ClientSettings.Default.UrlGetRecipes);
				if (res == null) return;

				_recipes.Clear();
				foreach (var recipe in res)
						_recipes.Add(recipe);

				if (current is not null)
				{
						var item = _recipes.FirstOrDefault(r => r.RecipeId == current.RecipeId);
						if (item is not null)
								bindingSourceRecipes.Position = _recipes.IndexOf(item);
				}
		}

		#endregion Recipes


		#region Categories


		private async void buttonCategoryCreate_Click(object sender, EventArgs e)
		{
				try
				{
						Cursor = Cursors.WaitCursor;
						CreateCategoryRequest request = new()
						{
								Name = textBoxCategoryName.Text
						};

						var res = await _rest.PostAsync<CategoryResponse, CreateCategoryRequest>(ClientSettings.Default.UrlPostCategory, request);
						if (res is null) return;

						await RefreshCategories();

						CategoryResponse? item = _categories.FirstOrDefault(c => c.CategoryId == res.CategoryId);
						if (item is not null)
								bindingSourceCategories.Position = _categories.IndexOf(item);
				}
				finally
				{
						Cursor = Cursors.Default;
				}
		}

		private async void buttonCategoryModify_Click(object sender, EventArgs e)
		{
				try
				{
						Cursor = Cursors.WaitCursor;

						if (bindingSourceCategories.Current is CategoryResponse currentCategory)
						{
								UpdateCategoryRequest request = new()
								{
										Name = textBoxCategoryName.Text
								};

								await _rest.PutAsync<CategoryResponse, UpdateCategoryRequest>($"{ClientSettings.Default.UrlPutCategory}/{currentCategory.CategoryId}",
										request);

								await RefreshCategories();


								if (currentCategory is not null)
								{
										CategoryResponse? item = _categories.FirstOrDefault(c => c.CategoryId == currentCategory.CategoryId);
										if (item is not null)
												bindingSourceCategories.Position = _categories.IndexOf(item);
								}
						}
				}
				finally
				{
						Cursor = Cursors.Default;
				}
		}

		private async void buttonCategoryDelete_Click(object sender, EventArgs e)
		{
				try
				{
						Cursor = Cursors.WaitCursor;

						if (bindingSourceCategories.Current is CategoryResponse current)
						{
								await _rest.DeleteAsync($"{ClientSettings.Default.UrlDeleteCategory}/{current.CategoryId}");
								await RefreshCategories();
						}
				}
				finally
				{
						Cursor = Cursors.Default;
				}
		}
		private async void buttonCategoriesRefresh_Click(object sender, EventArgs e)
		{
				try
				{
						Cursor = Cursors.WaitCursor;
						await RefreshCategories();
				}
				finally
				{
						Cursor = Cursors.Default;
				}
		}

		private async Task RefreshCategories()
		{
				var currentCategory = bindingSourceCategories.Current as Category;

				var res = await _rest.GetAsync<IEnumerable<CategoryResponse>>(ClientSettings.Default.UrlGetCategories);
				if (res == null) return;

				_categories.Clear();
				foreach (var c in res)
						_categories.Add(c);


				if (currentCategory is not null)
				{
						CategoryResponse? item = _categories.FirstOrDefault(c => c.CategoryId == currentCategory.CategoryId);
						if (item is not null)
								bindingSourceCategories.Position = _categories.IndexOf(item);
				}
		}

		#endregion Categories

		#region Association

		private async void bindingSourceRecipes_CurrentChanged(object sender, EventArgs e)
		{

				try
				{
						if (bindingSourceRecipes.Current is not RecipeResponse currentRecipe) return;
						Cursor = Cursors.WaitCursor;
						if (tabControl.SelectedTab == tabPageAssociationManager)
						{



								var categories = await _rest.GetAsync<IEnumerable<CategoryResponse>>(
										$"{ClientSettings.Default.UrlGetCategoriesByRecipeId}/{currentRecipe.RecipeId}");
								if (categories == null) return;

								_categoriesInRecipe.Clear();
								foreach (var category in categories)
										_categoriesInRecipe.Add(category);

								_categoriesNotInRecipe.Clear();
								foreach (var category in _categories.Where(category => _categoriesInRecipe.All(a => a.CategoryId != category.CategoryId)))
								{
										_categoriesNotInRecipe.Add(category);
								}

						}
						else if (tabControl.SelectedTab == tabPageRecipes)
						{

								var recipeIngredients = await _rest.GetAsync<IEnumerable<RecipeIngredientResponse>>(
										$"{ClientSettings.Default.UrlGetIngredientsByRecipeId}/{currentRecipe.RecipeId}");

								_ingredientsInRecipe.Clear();
								if (recipeIngredients is null) return;
								foreach (var ingredient in recipeIngredients)
								{
										_ingredientsInRecipe.Add(ingredient);
								}

						}

				}
				finally
				{
						Cursor = Cursors.Default;
				}
		}
		private void InitializeBinding()
		{
				// Recipes
				_recipes = [];
				bindingSourceRecipes.DataSource = _recipes;
				dataGridViewRecipes.DataSource = bindingSourceRecipes;

				// Ingredients in recipe
				_ingredientsInRecipe = [];
				bindingSourceIngredientsInRecipe.DataSource = _ingredientsInRecipe;
				dataGridViewIngredientsInRecipe.DataSource = bindingSourceIngredientsInRecipe;

				// Categories
				_categories = [];
				bindingSourceCategories.DataSource = _categories;
				dataGridViewCategories.DataSource = bindingSourceCategories;
				textBoxCategoryName.DataBindings.Add(nameof(textBoxCategoryName.Text), bindingSourceCategories, nameof(Category.Name), false, DataSourceUpdateMode.Never);

				// Associations
				dataGridViewAssociationRecipes.DataSource = bindingSourceRecipes;

				_categoriesInRecipe = [];
				bindingSourceCategoriesToRemove.DataSource = _categoriesInRecipe;
				dataGridViewCategoriesInRecipe.DataSource = bindingSourceCategoriesToRemove;

				_categoriesNotInRecipe = [];
				bindingSourceCategoriesToAdd.DataSource = _categoriesNotInRecipe;
				dataGridViewCategoriesNotInRecipe.DataSource = bindingSourceCategoriesToAdd;


				// Visibility

				dataGridViewRecipes.Columns[nameof(RecipeResponse.RecipeId)].Visible = false;
				dataGridViewRecipes.Columns[nameof(RecipeResponse.ImagePath)].Visible = false;
				dataGridViewRecipes.Columns[nameof(RecipeResponse.CreatorId)].Visible = false;
				dataGridViewRecipes.Columns[nameof(RecipeResponse.CreatorProfilePicture)].Visible = false;

				dataGridViewIngredientsInRecipe.Columns[nameof(RecipeIngredientResponse.IngredientId)].Visible = false;

				dataGridViewCategories.Columns[nameof(CategoryResponse.CategoryId)].Visible = false;

				foreach (var column in dataGridViewAssociationRecipes.Columns.Cast<DataGridViewColumn>().ToList())
				{
						if (column.Name == nameof(RecipeResponse.Title))
								continue;
						column.Visible = false;
				}

				dataGridViewCategoriesInRecipe.Columns[nameof(CategoryResponse.CategoryId)].Visible = false;

				dataGridViewCategoriesInRecipe.Columns[nameof(CategoryResponse.CategoryId)].Visible = false;
		}

		#endregion Association

		private async void buttonRemoveCategory_Click(object sender, EventArgs e)
		{
				try
				{
						Cursor = Cursors.WaitCursor;

						if (bindingSourceRecipes.Current is not RecipeResponse currentRecipe) return;

						if (bindingSourceCategoriesToRemove.Current is not CategoryResponse currentCategory) return;
						await _rest.DeleteAsync(
								$"{ClientSettings.Default.UrlRemoveCategoryByRecipeId}/{currentCategory.CategoryId}/{currentRecipe.RecipeId}");
				}
				finally
				{
						Cursor = Cursors.Default;
						buttonAssociationRecipesRefresh.PerformClick();
				}
		}

		private async void buttonAddCategory_Click(object sender, EventArgs e)
		{
				try
				{
						Cursor = Cursors.WaitCursor;

						if (bindingSourceRecipes.Current is not RecipeResponse currentRecipe) return;

						if (bindingSourceCategoriesToAdd.Current is not CategoryResponse currentCategory) return;
						await _rest.PostAsync(
								$"{ClientSettings.Default.UrlAddCategoryByRecipeId}/{currentCategory.CategoryId}/{currentRecipe.RecipeId}");
				}
				finally
				{
						Cursor = Cursors.Default;
						buttonAssociationRecipesRefresh.PerformClick();
				}
		}

		private async void buttonAssociationRecipesRefresh_Click(object sender, EventArgs e)
		{
				await RefreshRecipes();
		}
}