using CustomizeIdentity.Data;
using CustomizeIdentity.Models;
using CustomizeIdentity.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CustomizeIdentity.Identity;

public class UserRepository : IUserInterface
{
    private readonly AppDbContext _dbContext;

    public UserRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result> CreateUserAsync(AddUserViewModel newUser)
    {
        User user = new()
        {
            FirstName = newUser.FirstName,
            LastName = newUser.LastName,
            Email = newUser.Email,
            PasswordHash = EncodePasswordToBase64(newUser.Password),
            UserName = newUser.Email
        };

        if (await IsNotExist(user))
        {
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            return new Result(user);
        }

        return new Result(user, false, "Email already exist!");
    }

    public async Task<Result> LoginAsync(LoginUserViewModel viewModel)
    {
        var user = await FindByEmailAsync(viewModel.Email);
        if (user == null)
        {
            return new Result(new User() { Email = viewModel.Email}, false,
                        "Email not found!");
        }

        var passwordHash = EncodePasswordToBase64(viewModel.Password);
        if (user.PasswordHash == passwordHash)
        {
            return new Result(new User());
        }

        return new Result(new User(), false, "Incorrect password!");
    }

    public async Task<User> FindByEmailAsync(string email)
        => await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Email == email);

    private async Task<bool> IsNotExist(User user)
    {
        var excitingUser = await FindByEmailAsync(user.Email);
        return excitingUser == null;
    }

    private static string EncodePasswordToBase64(string password)
    {
        try
        {
            byte[] encData_byte = new byte[password.Length];
            encData_byte = System.Text.Encoding.UTF8.GetBytes(password);
            string encodedData = Convert.ToBase64String(encData_byte);
            return encodedData;
        }
        catch (Exception ex)
        {
            throw new Exception("Error in base64Encode" + ex.Message);
        }
    }
}