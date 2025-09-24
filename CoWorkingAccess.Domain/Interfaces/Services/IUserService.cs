using CoWorkingAccess.Domain.Entities;

namespace CoWorkingAccess.Domain.Interfaces.Services;

public interface IUserService
{
    Task<User?> GetUserByIdAsync(int id);

    Task<User?> GetUserByEmailAsync(string email);

    Task<IEnumerable<User>> GetAllUsersAsync();

    Task<User> CreateUserAsync(User user, string password);

    Task<User> UpdateUserAsync(User user);

    Task<bool> DeleteUserAsync(int id);

    Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
}
