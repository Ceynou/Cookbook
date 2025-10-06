namespace Cookbook.SharedData.Contracts.Responses;

public class UserResponse
{
    public required int UserId { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required bool IsAdmin { get; set; }
    public string? ProfilePicturePath { get; set; }
}