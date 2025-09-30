namespace Cookbook.Core;

public interface IJwtService
{
    string GenerateJwt(string username, params string[] roles);
}