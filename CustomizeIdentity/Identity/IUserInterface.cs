using CustomizeIdentity.Models;
using CustomizeIdentity.ViewModels;

namespace CustomizeIdentity.Identity;

public interface IUserInterface
{
    Task<Result> CreateUserAsync(AddUserViewModel newUser);
    Task<Result> LoginAsync(LoginUserViewModel viewModel);
    Task<User> FindByEmailAsync(string email);
}