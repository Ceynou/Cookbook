using Cookbook.SharedModels.Domain.Contracts.Requests;
using Cookbook.SharedModels.Entities;

namespace Cookbook.Core;

public interface IAccessService
{
    Task<User> SignInAsync(User user);

}