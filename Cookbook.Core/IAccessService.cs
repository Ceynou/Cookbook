using Cookbook.SharedData.Entities;

namespace Cookbook.Core;

public interface IAccessService
{
    /// <summary>
    ///     Hashes the input password, creates the <see cref="User" /> and returns it.
    /// </summary>
    /// <returns>A <see cref="User" />.</returns>
    Task<User> SignUpAsync(User user);

    /// <summary>
    ///     Fetches a user with the input username, verify the password and returns the <see cref="User" />.
    /// </summary>
    /// <remarks>
    ///     <para>GlobalExceptionMiddleware handles the exception and returns a NotFoundResult response with error details.</para>
    /// </remarks>
    /// <exception cref="ResourceNotFoundException">thrown if the repository returns a null.</exception>
    /// <returns>A <see cref="User" />.</returns>
    Task<User> SignInAsync(User user);
}