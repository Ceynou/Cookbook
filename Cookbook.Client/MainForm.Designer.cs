namespace Cookbook.Client
{
	partial class MainForm
	{
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

				#region Windows Form Designer generated code

				/// <summary>
				/// Required method for Designer support - do not modify
				/// the contents of this method with the code editor.
				/// </summary>
				private void InitializeComponent()
				{
						components = new System.ComponentModel.Container();
						tabHeader1 = new AntdUI.TabHeader();
						tabControl = new TabControl();
						tabPageAccount = new TabPage();
						tableLayoutPanelAccount = new TableLayoutPanel();
						buttonLogin = new Button();
						buttonLogout = new Button();
						labelApiUrl = new Label();
						labelUsername = new Label();
						labelPassword = new Label();
						textBoxApiUrl = new TextBox();
						textBoxUsername = new TextBox();
						textBoxPassword = new TextBox();
						tabPageRecipes = new TabPage();
						tableLayoutPanelRecipes = new TableLayoutPanel();
						tableLayoutPanel4 = new TableLayoutPanel();
						buttonRecipesRefresh = new Button();
						dataGridViewRecipes = new DataGridView();
						dataGridViewIngredientsInRecipe = new DataGridView();
						tabPageCategories = new TabPage();
						tableLayoutPanel3 = new TableLayoutPanel();
						dataGridViewCategories = new DataGridView();
						tableLayoutPanel5 = new TableLayoutPanel();
						buttonCategoryDelete = new Button();
						buttonCategoryModify = new Button();
						buttonCategoryCreate = new Button();
						buttonCategoriesRefresh = new Button();
						tableLayoutPanel1 = new TableLayoutPanel();
						categoryName = new Label();
						textBoxCategoryName = new TextBox();
						tabPageAssociationManager = new TabPage();
						tableLayoutPanel6 = new TableLayoutPanel();
						buttonAddCategory = new Button();
						buttonRemoveCategory = new Button();
						dataGridViewAssociationRecipes = new DataGridView();
						dataGridViewCategoriesInRecipe = new DataGridView();
						dataGridViewCategoriesNotInRecipe = new DataGridView();
						buttonAssociationRecipesRefresh = new Button();
						bindingSourceRecipes = new BindingSource(components);
						bindingSourceIngredientsInRecipe = new BindingSource(components);
						bindingSourceCategories = new BindingSource(components);
						bindingSourceCategoriesToRemove = new BindingSource(components);
						bindingSourceCategoriesToAdd = new BindingSource(components);
						tabControl.SuspendLayout();
						tabPageAccount.SuspendLayout();
						tableLayoutPanelAccount.SuspendLayout();
						tabPageRecipes.SuspendLayout();
						tableLayoutPanelRecipes.SuspendLayout();
						tableLayoutPanel4.SuspendLayout();
						((System.ComponentModel.ISupportInitialize)dataGridViewRecipes).BeginInit();
						((System.ComponentModel.ISupportInitialize)dataGridViewIngredientsInRecipe).BeginInit();
						tabPageCategories.SuspendLayout();
						tableLayoutPanel3.SuspendLayout();
						((System.ComponentModel.ISupportInitialize)dataGridViewCategories).BeginInit();
						tableLayoutPanel5.SuspendLayout();
						tableLayoutPanel1.SuspendLayout();
						tabPageAssociationManager.SuspendLayout();
						tableLayoutPanel6.SuspendLayout();
						((System.ComponentModel.ISupportInitialize)dataGridViewAssociationRecipes).BeginInit();
						((System.ComponentModel.ISupportInitialize)dataGridViewCategoriesInRecipe).BeginInit();
						((System.ComponentModel.ISupportInitialize)dataGridViewCategoriesNotInRecipe).BeginInit();
						((System.ComponentModel.ISupportInitialize)bindingSourceRecipes).BeginInit();
						((System.ComponentModel.ISupportInitialize)bindingSourceIngredientsInRecipe).BeginInit();
						((System.ComponentModel.ISupportInitialize)bindingSourceCategories).BeginInit();
						((System.ComponentModel.ISupportInitialize)bindingSourceCategoriesToRemove).BeginInit();
						((System.ComponentModel.ISupportInitialize)bindingSourceCategoriesToAdd).BeginInit();
						SuspendLayout();
						// 
						// tabHeader1
						// 
						tabHeader1.Dock = DockStyle.Top;
						tabHeader1.Location = new Point(0, 0);
						tabHeader1.Name = "tabHeader1";
						tabHeader1.ShowButton = true;
						tabHeader1.ShowIcon = true;
						tabHeader1.Size = new Size(800, 23);
						tabHeader1.TabIndex = 1;
						tabHeader1.Text = "Cookbook";
						// 
						// tabControl
						// 
						tabControl.Controls.Add(tabPageAccount);
						tabControl.Controls.Add(tabPageRecipes);
						tabControl.Controls.Add(tabPageCategories);
						tabControl.Controls.Add(tabPageAssociationManager);
						tabControl.Dock = DockStyle.Fill;
						tabControl.Location = new Point(0, 23);
						tabControl.Name = "tabControl";
						tabControl.SelectedIndex = 0;
						tabControl.Size = new Size(800, 577);
						tabControl.TabIndex = 3;
						tabControl.Selecting += tabControl_Selecting;
						// 
						// tabPageAccount
						// 
						tabPageAccount.Controls.Add(tableLayoutPanelAccount);
						tabPageAccount.Location = new Point(4, 24);
						tabPageAccount.Name = "tabPageAccount";
						tabPageAccount.Padding = new Padding(3);
						tabPageAccount.Size = new Size(792, 549);
						tabPageAccount.TabIndex = 0;
						tabPageAccount.Text = "Account";
						tabPageAccount.UseVisualStyleBackColor = true;
						// 
						// tableLayoutPanelAccount
						// 
						tableLayoutPanelAccount.Anchor = AnchorStyles.None;
						tableLayoutPanelAccount.AutoSizeMode = AutoSizeMode.GrowAndShrink;
						tableLayoutPanelAccount.ColumnCount = 3;
						tableLayoutPanelAccount.ColumnStyles.Add(new ColumnStyle());
						tableLayoutPanelAccount.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
						tableLayoutPanelAccount.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
						tableLayoutPanelAccount.Controls.Add(buttonLogin, 1, 3);
						tableLayoutPanelAccount.Controls.Add(buttonLogout, 2, 3);
						tableLayoutPanelAccount.Controls.Add(labelApiUrl, 0, 0);
						tableLayoutPanelAccount.Controls.Add(labelUsername, 0, 1);
						tableLayoutPanelAccount.Controls.Add(labelPassword, 0, 2);
						tableLayoutPanelAccount.Controls.Add(textBoxApiUrl, 1, 0);
						tableLayoutPanelAccount.Controls.Add(textBoxUsername, 1, 1);
						tableLayoutPanelAccount.Controls.Add(textBoxPassword, 1, 2);
						tableLayoutPanelAccount.Location = new Point(248, 206);
						tableLayoutPanelAccount.Name = "tableLayoutPanelAccount";
						tableLayoutPanelAccount.RowCount = 4;
						tableLayoutPanelAccount.RowStyles.Add(new RowStyle());
						tableLayoutPanelAccount.RowStyles.Add(new RowStyle());
						tableLayoutPanelAccount.RowStyles.Add(new RowStyle());
						tableLayoutPanelAccount.RowStyles.Add(new RowStyle());
						tableLayoutPanelAccount.Size = new Size(297, 137);
						tableLayoutPanelAccount.TabIndex = 0;
						// 
						// buttonLogin
						// 
						buttonLogin.Dock = DockStyle.Fill;
						buttonLogin.Location = new Point(68, 90);
						buttonLogin.Name = "buttonLogin";
						buttonLogin.Size = new Size(110, 44);
						buttonLogin.TabIndex = 0;
						buttonLogin.Text = "log in";
						buttonLogin.UseVisualStyleBackColor = true;
						buttonLogin.Click += buttonLogin_Click;
						// 
						// buttonLogout
						// 
						buttonLogout.Dock = DockStyle.Fill;
						buttonLogout.Location = new Point(184, 90);
						buttonLogout.Name = "buttonLogout";
						buttonLogout.Size = new Size(110, 44);
						buttonLogout.TabIndex = 1;
						buttonLogout.Text = "log out";
						buttonLogout.UseVisualStyleBackColor = true;
						buttonLogout.Click += buttonLogout_Click;
						// 
						// labelApiUrl
						// 
						labelApiUrl.AutoSize = true;
						labelApiUrl.Location = new Point(3, 0);
						labelApiUrl.Name = "labelApiUrl";
						labelApiUrl.Size = new Size(49, 15);
						labelApiUrl.TabIndex = 2;
						labelApiUrl.Text = "API URL";
						// 
						// labelUsername
						// 
						labelUsername.AutoSize = true;
						labelUsername.Location = new Point(3, 29);
						labelUsername.Name = "labelUsername";
						labelUsername.Size = new Size(59, 15);
						labelUsername.TabIndex = 3;
						labelUsername.Text = "username";
						// 
						// labelPassword
						// 
						labelPassword.AutoSize = true;
						labelPassword.Location = new Point(3, 58);
						labelPassword.Name = "labelPassword";
						labelPassword.Size = new Size(57, 15);
						labelPassword.TabIndex = 4;
						labelPassword.Text = "password";
						// 
						// textBoxApiUrl
						// 
						tableLayoutPanelAccount.SetColumnSpan(textBoxApiUrl, 2);
						textBoxApiUrl.Dock = DockStyle.Fill;
						textBoxApiUrl.Location = new Point(68, 3);
						textBoxApiUrl.Name = "textBoxApiUrl";
						textBoxApiUrl.Size = new Size(226, 23);
						textBoxApiUrl.TabIndex = 7;
						textBoxApiUrl.KeyPress += textBoxApiUrl_KeyPress;
						// 
						// textBoxUsername
						// 
						tableLayoutPanelAccount.SetColumnSpan(textBoxUsername, 2);
						textBoxUsername.Dock = DockStyle.Fill;
						textBoxUsername.Location = new Point(68, 32);
						textBoxUsername.Name = "textBoxUsername";
						textBoxUsername.Size = new Size(226, 23);
						textBoxUsername.TabIndex = 5;
						textBoxUsername.KeyPress += textBoxUsername_KeyPress;
						// 
						// textBoxPassword
						// 
						tableLayoutPanelAccount.SetColumnSpan(textBoxPassword, 2);
						textBoxPassword.Dock = DockStyle.Fill;
						textBoxPassword.Location = new Point(68, 61);
						textBoxPassword.Name = "textBoxPassword";
						textBoxPassword.PasswordChar = '*';
						textBoxPassword.Size = new Size(226, 23);
						textBoxPassword.TabIndex = 6;
						textBoxPassword.KeyPress += textBoxPassword_KeyPress;
						// 
						// tabPageRecipes
						// 
						tabPageRecipes.Controls.Add(tableLayoutPanelRecipes);
						tabPageRecipes.Location = new Point(4, 24);
						tabPageRecipes.Name = "tabPageRecipes";
						tabPageRecipes.Padding = new Padding(3);
						tabPageRecipes.Size = new Size(792, 549);
						tabPageRecipes.TabIndex = 1;
						tabPageRecipes.Text = "Recipes";
						tabPageRecipes.UseVisualStyleBackColor = true;
						// 
						// tableLayoutPanelRecipes
						// 
						tableLayoutPanelRecipes.ColumnCount = 2;
						tableLayoutPanelRecipes.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
						tableLayoutPanelRecipes.ColumnStyles.Add(new ColumnStyle());
						tableLayoutPanelRecipes.Controls.Add(tableLayoutPanel4, 0, 1);
						tableLayoutPanelRecipes.Controls.Add(dataGridViewRecipes, 0, 0);
						tableLayoutPanelRecipes.Controls.Add(dataGridViewIngredientsInRecipe, 1, 0);
						tableLayoutPanelRecipes.Dock = DockStyle.Fill;
						tableLayoutPanelRecipes.Location = new Point(3, 3);
						tableLayoutPanelRecipes.Name = "tableLayoutPanelRecipes";
						tableLayoutPanelRecipes.RowCount = 2;
						tableLayoutPanelRecipes.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
						tableLayoutPanelRecipes.RowStyles.Add(new RowStyle());
						tableLayoutPanelRecipes.Size = new Size(786, 543);
						tableLayoutPanelRecipes.TabIndex = 0;
						// 
						// tableLayoutPanel4
						// 
						tableLayoutPanel4.AutoSize = true;
						tableLayoutPanel4.ColumnCount = 1;
						tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
						tableLayoutPanel4.Controls.Add(buttonRecipesRefresh, 0, 0);
						tableLayoutPanel4.Dock = DockStyle.Fill;
						tableLayoutPanel4.Location = new Point(3, 489);
						tableLayoutPanel4.Name = "tableLayoutPanel4";
						tableLayoutPanel4.RowCount = 1;
						tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
						tableLayoutPanel4.Size = new Size(240, 51);
						tableLayoutPanel4.TabIndex = 4;
						// 
						// buttonRecipesRefresh
						// 
						buttonRecipesRefresh.Anchor = AnchorStyles.None;
						buttonRecipesRefresh.Location = new Point(45, 3);
						buttonRecipesRefresh.Name = "buttonRecipesRefresh";
						buttonRecipesRefresh.Size = new Size(150, 45);
						buttonRecipesRefresh.TabIndex = 0;
						buttonRecipesRefresh.Text = "refresh";
						buttonRecipesRefresh.UseVisualStyleBackColor = true;
						buttonRecipesRefresh.Click += buttonRecipesRefresh_Click;
						// 
						// dataGridViewRecipes
						// 
						dataGridViewRecipes.AllowUserToAddRows = false;
						dataGridViewRecipes.AllowUserToDeleteRows = false;
						dataGridViewRecipes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
						dataGridViewRecipes.Dock = DockStyle.Fill;
						dataGridViewRecipes.Location = new Point(3, 3);
						dataGridViewRecipes.Name = "dataGridViewRecipes";
						dataGridViewRecipes.ReadOnly = true;
						dataGridViewRecipes.Size = new Size(240, 480);
						dataGridViewRecipes.TabIndex = 0;
						// 
						// dataGridViewIngredientsInRecipe
						// 
						dataGridViewIngredientsInRecipe.AllowUserToAddRows = false;
						dataGridViewIngredientsInRecipe.AllowUserToDeleteRows = false;
						dataGridViewIngredientsInRecipe.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
						dataGridViewIngredientsInRecipe.Dock = DockStyle.Fill;
						dataGridViewIngredientsInRecipe.Location = new Point(249, 3);
						dataGridViewIngredientsInRecipe.Name = "dataGridViewIngredientsInRecipe";
						dataGridViewIngredientsInRecipe.ReadOnly = true;
						dataGridViewIngredientsInRecipe.Size = new Size(534, 480);
						dataGridViewIngredientsInRecipe.TabIndex = 1;
						// 
						// tabPageCategories
						// 
						tabPageCategories.Controls.Add(tableLayoutPanel3);
						tabPageCategories.Location = new Point(4, 24);
						tabPageCategories.Name = "tabPageCategories";
						tabPageCategories.Padding = new Padding(3);
						tabPageCategories.Size = new Size(792, 549);
						tabPageCategories.TabIndex = 2;
						tabPageCategories.Text = "Categories";
						tabPageCategories.UseVisualStyleBackColor = true;
						// 
						// tableLayoutPanel3
						// 
						tableLayoutPanel3.ColumnCount = 1;
						tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
						tableLayoutPanel3.Controls.Add(dataGridViewCategories, 0, 0);
						tableLayoutPanel3.Controls.Add(tableLayoutPanel5, 0, 2);
						tableLayoutPanel3.Controls.Add(tableLayoutPanel1, 0, 1);
						tableLayoutPanel3.Dock = DockStyle.Fill;
						tableLayoutPanel3.Location = new Point(3, 3);
						tableLayoutPanel3.Name = "tableLayoutPanel3";
						tableLayoutPanel3.RowCount = 3;
						tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
						tableLayoutPanel3.RowStyles.Add(new RowStyle());
						tableLayoutPanel3.RowStyles.Add(new RowStyle());
						tableLayoutPanel3.Size = new Size(786, 543);
						tableLayoutPanel3.TabIndex = 0;
						// 
						// dataGridViewCategories
						// 
						dataGridViewCategories.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
						dataGridViewCategories.Dock = DockStyle.Fill;
						dataGridViewCategories.Location = new Point(3, 3);
						dataGridViewCategories.Name = "dataGridViewCategories";
						dataGridViewCategories.Size = new Size(780, 445);
						dataGridViewCategories.TabIndex = 0;
						// 
						// tableLayoutPanel5
						// 
						tableLayoutPanel5.AutoSize = true;
						tableLayoutPanel5.ColumnCount = 5;
						tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle());
						tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle());
						tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle());
						tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle());
						tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
						tableLayoutPanel5.Controls.Add(buttonCategoryDelete, 3, 0);
						tableLayoutPanel5.Controls.Add(buttonCategoryModify, 2, 0);
						tableLayoutPanel5.Controls.Add(buttonCategoryCreate, 1, 0);
						tableLayoutPanel5.Controls.Add(buttonCategoriesRefresh, 0, 0);
						tableLayoutPanel5.Dock = DockStyle.Fill;
						tableLayoutPanel5.Location = new Point(3, 489);
						tableLayoutPanel5.Name = "tableLayoutPanel5";
						tableLayoutPanel5.RowCount = 1;
						tableLayoutPanel5.RowStyles.Add(new RowStyle());
						tableLayoutPanel5.Size = new Size(780, 51);
						tableLayoutPanel5.TabIndex = 1;
						// 
						// buttonCategoryDelete
						// 
						buttonCategoryDelete.Anchor = AnchorStyles.None;
						buttonCategoryDelete.Location = new Point(471, 3);
						buttonCategoryDelete.Name = "buttonCategoryDelete";
						buttonCategoryDelete.Size = new Size(150, 45);
						buttonCategoryDelete.TabIndex = 3;
						buttonCategoryDelete.Text = "delete";
						buttonCategoryDelete.UseVisualStyleBackColor = true;
						buttonCategoryDelete.Click += buttonCategoryDelete_Click;
						// 
						// buttonCategoryModify
						// 
						buttonCategoryModify.Anchor = AnchorStyles.None;
						buttonCategoryModify.Location = new Point(315, 3);
						buttonCategoryModify.Name = "buttonCategoryModify";
						buttonCategoryModify.Size = new Size(150, 45);
						buttonCategoryModify.TabIndex = 2;
						buttonCategoryModify.Text = "modify";
						buttonCategoryModify.UseVisualStyleBackColor = true;
						buttonCategoryModify.Click += buttonCategoryModify_Click;
						// 
						// buttonCategoryCreate
						// 
						buttonCategoryCreate.Anchor = AnchorStyles.None;
						buttonCategoryCreate.Location = new Point(159, 3);
						buttonCategoryCreate.Name = "buttonCategoryCreate";
						buttonCategoryCreate.Size = new Size(150, 45);
						buttonCategoryCreate.TabIndex = 1;
						buttonCategoryCreate.Text = "create";
						buttonCategoryCreate.UseVisualStyleBackColor = true;
						buttonCategoryCreate.Click += buttonCategoryCreate_Click;
						// 
						// buttonCategoriesRefresh
						// 
						buttonCategoriesRefresh.Anchor = AnchorStyles.None;
						buttonCategoriesRefresh.Location = new Point(3, 3);
						buttonCategoriesRefresh.Name = "buttonCategoriesRefresh";
						buttonCategoriesRefresh.Size = new Size(150, 45);
						buttonCategoriesRefresh.TabIndex = 0;
						buttonCategoriesRefresh.Text = "refresh";
						buttonCategoriesRefresh.UseVisualStyleBackColor = true;
						buttonCategoriesRefresh.Click += buttonCategoriesRefresh_Click;
						// 
						// tableLayoutPanel1
						// 
						tableLayoutPanel1.AutoSize = true;
						tableLayoutPanel1.ColumnCount = 2;
						tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
						tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
						tableLayoutPanel1.Controls.Add(categoryName, 0, 0);
						tableLayoutPanel1.Controls.Add(textBoxCategoryName, 1, 0);
						tableLayoutPanel1.Dock = DockStyle.Fill;
						tableLayoutPanel1.Location = new Point(3, 454);
						tableLayoutPanel1.Name = "tableLayoutPanel1";
						tableLayoutPanel1.RowCount = 1;
						tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
						tableLayoutPanel1.Size = new Size(780, 29);
						tableLayoutPanel1.TabIndex = 2;
						// 
						// categoryName
						// 
						categoryName.Location = new Point(3, 0);
						categoryName.Name = "categoryName";
						categoryName.Size = new Size(100, 23);
						categoryName.TabIndex = 0;
						categoryName.Text = "name";
						categoryName.TextAlign = ContentAlignment.MiddleCenter;
						// 
						// textBoxCategoryName
						// 
						textBoxCategoryName.Dock = DockStyle.Fill;
						textBoxCategoryName.Location = new Point(109, 3);
						textBoxCategoryName.Name = "textBoxCategoryName";
						textBoxCategoryName.Size = new Size(668, 23);
						textBoxCategoryName.TabIndex = 1;
						// 
						// tabPageAssociationManager
						// 
						tabPageAssociationManager.Controls.Add(tableLayoutPanel6);
						tabPageAssociationManager.Location = new Point(4, 24);
						tabPageAssociationManager.Name = "tabPageAssociationManager";
						tabPageAssociationManager.Padding = new Padding(3);
						tabPageAssociationManager.Size = new Size(792, 549);
						tabPageAssociationManager.TabIndex = 3;
						tabPageAssociationManager.Text = "Association manager";
						tabPageAssociationManager.UseVisualStyleBackColor = true;
						// 
						// tableLayoutPanel6
						// 
						tableLayoutPanel6.AutoSize = true;
						tableLayoutPanel6.ColumnCount = 3;
						tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
						tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
						tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle());
						tableLayoutPanel6.Controls.Add(buttonAddCategory, 2, 1);
						tableLayoutPanel6.Controls.Add(buttonRemoveCategory, 2, 0);
						tableLayoutPanel6.Controls.Add(dataGridViewAssociationRecipes, 0, 0);
						tableLayoutPanel6.Controls.Add(dataGridViewCategoriesInRecipe, 1, 0);
						tableLayoutPanel6.Controls.Add(dataGridViewCategoriesNotInRecipe, 1, 1);
						tableLayoutPanel6.Controls.Add(buttonAssociationRecipesRefresh, 0, 2);
						tableLayoutPanel6.Dock = DockStyle.Fill;
						tableLayoutPanel6.Location = new Point(3, 3);
						tableLayoutPanel6.Name = "tableLayoutPanel6";
						tableLayoutPanel6.RowCount = 3;
						tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
						tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
						tableLayoutPanel6.RowStyles.Add(new RowStyle());
						tableLayoutPanel6.Size = new Size(786, 543);
						tableLayoutPanel6.TabIndex = 0;
						// 
						// buttonAddCategory
						// 
						buttonAddCategory.Anchor = AnchorStyles.None;
						buttonAddCategory.Location = new Point(633, 346);
						buttonAddCategory.Name = "buttonAddCategory";
						buttonAddCategory.Size = new Size(150, 45);
						buttonAddCategory.TabIndex = 5;
						buttonAddCategory.Text = "add";
						buttonAddCategory.UseVisualStyleBackColor = true;
						buttonAddCategory.Click += buttonAddCategory_Click;
						// 
						// buttonRemoveCategory
						// 
						buttonRemoveCategory.Anchor = AnchorStyles.None;
						buttonRemoveCategory.Location = new Point(633, 100);
						buttonRemoveCategory.Name = "buttonRemoveCategory";
						buttonRemoveCategory.Size = new Size(150, 45);
						buttonRemoveCategory.TabIndex = 4;
						buttonRemoveCategory.Text = "remove";
						buttonRemoveCategory.UseVisualStyleBackColor = true;
						buttonRemoveCategory.Click += buttonRemoveCategory_Click;
						// 
						// dataGridViewAssociationRecipes
						// 
						dataGridViewAssociationRecipes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
						dataGridViewAssociationRecipes.Dock = DockStyle.Fill;
						dataGridViewAssociationRecipes.Location = new Point(3, 3);
						dataGridViewAssociationRecipes.Name = "dataGridViewAssociationRecipes";
						tableLayoutPanel6.SetRowSpan(dataGridViewAssociationRecipes, 2);
						dataGridViewAssociationRecipes.Size = new Size(309, 486);
						dataGridViewAssociationRecipes.TabIndex = 0;
						// 
						// dataGridViewCategoriesInRecipe
						// 
						dataGridViewCategoriesInRecipe.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
						dataGridViewCategoriesInRecipe.Dock = DockStyle.Fill;
						dataGridViewCategoriesInRecipe.Location = new Point(318, 3);
						dataGridViewCategoriesInRecipe.Name = "dataGridViewCategoriesInRecipe";
						dataGridViewCategoriesInRecipe.Size = new Size(309, 240);
						dataGridViewCategoriesInRecipe.TabIndex = 1;
						// 
						// dataGridViewCategoriesNotInRecipe
						// 
						dataGridViewCategoriesNotInRecipe.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
						dataGridViewCategoriesNotInRecipe.Dock = DockStyle.Fill;
						dataGridViewCategoriesNotInRecipe.Location = new Point(318, 249);
						dataGridViewCategoriesNotInRecipe.Name = "dataGridViewCategoriesNotInRecipe";
						dataGridViewCategoriesNotInRecipe.Size = new Size(309, 240);
						dataGridViewCategoriesNotInRecipe.TabIndex = 2;
						// 
						// buttonAssociationRecipesRefresh
						// 
						buttonAssociationRecipesRefresh.Anchor = AnchorStyles.None;
						buttonAssociationRecipesRefresh.Location = new Point(82, 495);
						buttonAssociationRecipesRefresh.Name = "buttonAssociationRecipesRefresh";
						buttonAssociationRecipesRefresh.Size = new Size(150, 45);
						buttonAssociationRecipesRefresh.TabIndex = 3;
						buttonAssociationRecipesRefresh.Text = "refresh";
						buttonAssociationRecipesRefresh.UseVisualStyleBackColor = true;
						buttonAssociationRecipesRefresh.Click += buttonAssociationRecipesRefresh_Click;
						// 
						// bindingSourceRecipes
						// 
						bindingSourceRecipes.CurrentChanged += bindingSourceRecipes_CurrentChanged;
						// 
						// MainForm
						// 
						AutoScaleDimensions = new SizeF(7F, 15F);
						AutoScaleMode = AutoScaleMode.Font;
						ClientSize = new Size(800, 600);
						Controls.Add(tabControl);
						Controls.Add(tabHeader1);
						MinimumSize = new Size(800, 600);
						Name = "MainForm";
						Text = "Form1";
						FormClosing += MainForm_FormClosing;
						Load += MainForm_Load;
						tabControl.ResumeLayout(false);
						tabPageAccount.ResumeLayout(false);
						tableLayoutPanelAccount.ResumeLayout(false);
						tableLayoutPanelAccount.PerformLayout();
						tabPageRecipes.ResumeLayout(false);
						tableLayoutPanelRecipes.ResumeLayout(false);
						tableLayoutPanelRecipes.PerformLayout();
						tableLayoutPanel4.ResumeLayout(false);
						((System.ComponentModel.ISupportInitialize)dataGridViewRecipes).EndInit();
						((System.ComponentModel.ISupportInitialize)dataGridViewIngredientsInRecipe).EndInit();
						tabPageCategories.ResumeLayout(false);
						tableLayoutPanel3.ResumeLayout(false);
						tableLayoutPanel3.PerformLayout();
						((System.ComponentModel.ISupportInitialize)dataGridViewCategories).EndInit();
						tableLayoutPanel5.ResumeLayout(false);
						tableLayoutPanel1.ResumeLayout(false);
						tableLayoutPanel1.PerformLayout();
						tabPageAssociationManager.ResumeLayout(false);
						tabPageAssociationManager.PerformLayout();
						tableLayoutPanel6.ResumeLayout(false);
						((System.ComponentModel.ISupportInitialize)dataGridViewAssociationRecipes).EndInit();
						((System.ComponentModel.ISupportInitialize)dataGridViewCategoriesInRecipe).EndInit();
						((System.ComponentModel.ISupportInitialize)dataGridViewCategoriesNotInRecipe).EndInit();
						((System.ComponentModel.ISupportInitialize)bindingSourceRecipes).EndInit();
						((System.ComponentModel.ISupportInitialize)bindingSourceIngredientsInRecipe).EndInit();
						((System.ComponentModel.ISupportInitialize)bindingSourceCategories).EndInit();
						((System.ComponentModel.ISupportInitialize)bindingSourceCategoriesToRemove).EndInit();
						((System.ComponentModel.ISupportInitialize)bindingSourceCategoriesToAdd).EndInit();
						ResumeLayout(false);
				}

				private System.Windows.Forms.Label categoryName;
		private System.Windows.Forms.TextBox textBoxCategoryName;

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;

		#endregion

		private AntdUI.TabHeader tabHeader1;
		private TabControl tabControl;
		private TabPage tabPageAccount;
		private TableLayoutPanel tableLayoutPanelAccount;
		private TabPage tabPageRecipes;
		private Button buttonLogin;
		private Button buttonLogout;
		private Label labelApiUrl;
		private Label labelUsername;
		private Label labelPassword;
		private TextBox textBoxApiUrl;
		private TextBox textBoxUsername;
		private TextBox textBoxPassword;
		private TabPage tabPageCategories;
		private TabPage tabPageAssociationManager;
		private TableLayoutPanel tableLayoutPanelRecipes;
		private DataGridView dataGridViewRecipes;
		private DataGridView dataGridViewIngredientsInRecipe;
		private System.Windows.Forms.BindingSource bindingSourceRecipes;
		private BindingSource bindingSourceCategories;
		private BindingSource bindingSourceIngredientsInRecipe;
		private BindingSource bindingSourceCategoriesToRemove;
		private TableLayoutPanel tableLayoutPanel4;
		private Button buttonRecipesRefresh;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
		private System.Windows.Forms.DataGridView dataGridViewCategories;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
		private System.Windows.Forms.Button buttonCategoriesRefresh;
		private System.Windows.Forms.Button buttonCategoryDelete;
		private System.Windows.Forms.Button buttonCategoryModify;
		private System.Windows.Forms.Button buttonCategoryCreate;
		private TableLayoutPanel tableLayoutPanel6;
		private DataGridView dataGridViewAssociationRecipes;
		private DataGridView dataGridViewCategoriesInRecipe;
		private DataGridView dataGridViewCategoriesNotInRecipe;
		private Button buttonAssociationRecipesRefresh;
		private BindingSource bindingSourceCategoriesToAdd;
		private Button buttonAddCategory;
		private Button buttonRemoveCategory;
		}
}
