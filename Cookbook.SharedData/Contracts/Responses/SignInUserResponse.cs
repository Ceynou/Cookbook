namespace Cookbook.SharedData.Contracts.Responses;

public record SignInUserResponse
{
    public required int UserId { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string Token { get; set; }
}