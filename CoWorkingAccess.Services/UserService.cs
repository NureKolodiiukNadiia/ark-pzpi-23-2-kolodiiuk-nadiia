using CoWorkingAccess.Domain.Entities;
using CoWorkingAccess.Domain.Extensions;
using CoWorkingAccess.Domain.Interfaces.Repositories;
using CoWorkingAccess.Domain.Interfaces.Services;

namespace CoWorkingAccess.Services;

public class UserService : IUserService
{
    private IUserRepository _userRepository;

    public Task<User?> GetUserByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<User?> GetUserByEmailAsync(string email)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        var result = await _userRepository.GetAllAsync();
        result.OnFailure(() => throw new Exception(result.Error));

        return result.Value;
    }

    public Task<User> CreateUserAsync(User user, string password)
    {
        throw new NotImplementedException();
    }

    public Task<User> UpdateUserAsync(User user)
    {
        throw new NotImplementedException();
    }

    Task<bool> IUserService.DeleteUserAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
    {
        throw new NotImplementedException();
    }

    public async Task<User> GetUserAsync(int userId)
    {
        var result = await _userRepository.GetByIdAsync(userId);
        result.OnFailure(() => throw new Exception(result.Error));

        return result.Value;
    }

    public async Task DeleteUserAsync(int userId)
    {
        throw new NotImplementedException();
    }
}