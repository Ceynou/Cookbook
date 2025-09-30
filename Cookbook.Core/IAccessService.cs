using Cookbook.SharedData.Entities;

namespace Cookbook.Core;

public interface IAccessService
{
    Task<User> SignUpAsync(User user);
    Task<User> SignInAsync(User user);
}