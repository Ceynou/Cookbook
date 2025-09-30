using Cookbook.SharedData.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Cookbook.Data;

public class CookbookContext : DbContext
{
    private readonly IConfiguration _configuration;

    public CookbookContext()
    {
    }

    public CookbookContext(DbContextOptions<CookbookContext> options, IConfiguration config)
        : base(options)
    {
        _configuration = config;
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Ingredient> Ingredients { get; set; }

    public virtual DbSet<Recipe> Recipes { get; set; }

    public virtual DbSet<RecipesIngredient> RecipesIngredients { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Step> Steps { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
            optionsBuilder.UseNpgsql(_configuration.GetSection("DatabaseProviderString").Value);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("categories_pkey");

            entity.ToTable("categories");

            entity.HasIndex(e => e.Name, "categories_name_key").IsUnique();

            entity.Property(e => e.CategoryId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("category_id");
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Ingredient>(entity =>
        {
            entity.HasKey(e => e.IngredientId).HasName("ingredients_pkey");

            entity.ToTable("ingredients");

            entity.HasIndex(e => e.Name, "ingredients_name_key").IsUnique();

            entity.Property(e => e.IngredientId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("ingredient_id");
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Recipe>(entity =>
        {
            entity.HasKey(e => e.RecipeId).HasName("recipes_pkey");

            entity.ToTable("recipes");

            entity.Property(e => e.RecipeId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("recipe_id");
            entity.Property(e => e.CookingDuration).HasColumnName("cooking_duration");
            entity.Property(e => e.CreatorId).HasColumnName("creator_id");
            entity.Property(e => e.Difficulty).HasColumnName("difficulty");
            entity.Property(e => e.ImagePath)
                .HasMaxLength(255)
                .HasColumnName("image_path");
            entity.Property(e => e.PreparationDuration).HasColumnName("preparation_duration");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .HasColumnName("title");

            entity.HasOne(d => d.Creator).WithMany(p => p.Recipes)
                .HasForeignKey(d => d.CreatorId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("recipes_creator_id_fkey");
        });

        modelBuilder.Entity<RecipesCategory>(entity =>
        {
            entity.HasKey(e => new { e.RecipeId, e.CategoryId }).HasName("recipes_categories_pkey");

            entity.ToTable("recipes_categories");

            entity.Property(e => e.RecipeId).HasColumnName("recipe_id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");

            entity.HasOne(d => d.Category).WithMany(p => p.RecipesCategories)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("recipes_categories_category_id_fkey");

            entity.HasOne(d => d.Recipe).WithMany(p => p.RecipesCategories)
                .HasForeignKey(d => d.RecipeId)
                .HasConstraintName("recipes_categories_recipe_id_fkey");
        });

        modelBuilder.Entity<RecipesIngredient>(entity =>
        {
            entity.HasKey(e => new { e.RecipeId, e.IngredientId }).HasName("recipes_ingredients_pkey");

            entity.ToTable("recipes_ingredients");

            entity.Property(e => e.RecipeId).HasColumnName("recipe_id");
            entity.Property(e => e.IngredientId).HasColumnName("ingredient_id");
            entity.Property(e => e.Quantity)
                .HasPrecision(7, 2)
                .HasColumnName("quantity");
            entity.Property(e => e.Unit)
                .HasMaxLength(30)
                .HasColumnName("unit");

            entity.HasOne(d => d.Ingredient).WithMany(p => p.RecipesIngredients)
                .HasForeignKey(d => d.IngredientId)
                .HasConstraintName("recipes_ingredients_ingredient_id_fkey");

            entity.HasOne(d => d.Recipe).WithMany(p => p.RecipesIngredients)
                .HasForeignKey(d => d.RecipeId)
                .HasConstraintName("recipes_ingredients_recipe_id_fkey");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => new { e.RecipeId, e.ReviewerId }).HasName("reviews_pkey");

            entity.ToTable("reviews");

            entity.Property(e => e.RecipeId).HasColumnName("recipe_id");
            entity.Property(e => e.ReviewerId).HasColumnName("reviewer_id");
            entity.Property(e => e.Impression)
                .HasMaxLength(500)
                .HasColumnName("impression");
            entity.Property(e => e.Rating).HasColumnName("rating");

            entity.HasOne(d => d.Recipe).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.RecipeId)
                .HasConstraintName("reviews_recipe_id_fkey");

            entity.HasOne(d => d.Reviewer).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.ReviewerId)
                .HasConstraintName("reviews_reviewer_id_fkey");
        });

        modelBuilder.Entity<Step>(entity =>
        {
            entity.HasKey(e => new { e.StepNumber, e.RecipeId }).HasName("steps_pkey");

            entity.ToTable("steps");

            entity.Property(e => e.StepNumber).HasColumnName("step_number");
            entity.Property(e => e.RecipeId).HasColumnName("recipe_id");
            entity.Property(e => e.Duration).HasColumnName("duration");
            entity.Property(e => e.Instruction)
                .HasMaxLength(200)
                .HasColumnName("instruction");
            entity.Property(e => e.IsCooking).HasColumnName("is_cooking");

            entity.HasOne(d => d.Recipe).WithMany(p => p.Steps)
                .HasForeignKey(d => d.RecipeId)
                .HasConstraintName("steps_recipe_id_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "users_email_key").IsUnique();

            entity.HasIndex(e => e.Username, "users_username_key").IsUnique();

            entity.Property(e => e.UserId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("user_id");
            entity.Property(e => e.BirthDate).HasColumnName("birth_date");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.ImagePath)
                .HasMaxLength(255)
                .HasColumnName("image_path");
            entity.Property(e => e.IsAdmin).HasColumnName("is_admin");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(72)
                .HasColumnName("password_hash");
            entity.Property(e => e.Username)
                .HasMaxLength(20)
                .HasColumnName("username");
        });
    }
}