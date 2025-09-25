namespace Cookbook.SharedModels.Domain.Contracts.Responses;

public record SignInUserResponse
{
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string Token { get; set; }
}