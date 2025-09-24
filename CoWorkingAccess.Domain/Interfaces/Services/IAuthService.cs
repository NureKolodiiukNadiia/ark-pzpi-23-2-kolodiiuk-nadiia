using CoWorkingAccess.Domain.Entities;

namespace CoWorkingAccess.Domain.Interfaces.Services;

public interface IAuthService
{
    Task<User> LoginAsync(string email, string password);

    Task<int> RegisterAsync(User user);

    // Task<bool> ValidatePasswordAsync(User user, string password);
}