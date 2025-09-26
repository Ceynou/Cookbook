namespace Cookbook.SharedModels.Contracts.Responses;

public class SignUpUserResponse
{
    
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string Token { get; set; }
}