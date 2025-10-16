namespace Cookbook.SharedData.Entities;

public sealed class User
{
    public int UserId { get; init; }

    public string Username { get; init; } = null!;

    public string Email { get; init; } = null!;

    public string PasswordHash { get; set; } = null!;

    public bool IsAdmin { get; init; }

    public string? ImagePath { get; init; }

    public ICollection<Recipe> Recipes { get; init; } = new List<Recipe>();

    public ICollection<Review> Reviews { get; init; } = new List<Review>();
}