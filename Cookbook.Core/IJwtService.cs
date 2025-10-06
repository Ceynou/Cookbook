namespace Cookbook.Core;

public interface IJwtService
{
    /// <summary>
    ///     Adds the id and the roles to a list of claims, creates a JSON Web Token and returns it.
    /// </summary>
    /// <returns>The string of a token.</returns>
    string GenerateJwt(string id, params string[] roles);
}