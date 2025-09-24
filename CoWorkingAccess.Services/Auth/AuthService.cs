using CoWorkingAccess.Domain.Entities;
using CoWorkingAccess.Domain.Extensions;
using CoWorkingAccess.Domain.Interfaces.Repositories;
using CoWorkingAccess.Domain.Interfaces.Services;
using CoWorkingAccess.Services.Helpers;
using Microsoft.AspNetCore.Identity;

namespace CoWorkingAccess.Services.Auth;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    
    private readonly TokenHelper _tokenHelper;
    
    private readonly UserManager<User> _userManager;

    public AuthService(IUserRepository userRepository,
        UserManager<User> userManager,
        TokenHelper tokenHelper)
    {
        _userRepository = userRepository;
        _userManager = userManager;
        _tokenHelper = tokenHelper;
    }

    public async Task<int> RegisterAsync(User user)
    {
        var result = await _userRepository.CreateAsync(user);
        // var result = await _userRepository.SaveChangesAsync();
        result.OnFailure(() => throw new Exception(result.Error));

        return result.Value.Id;
    }

    public async Task<User> LoginAsync(string email, string password)
    {
        var result = await _userRepository.GetUserByEmailPassword(email, password);
        result.OnFailure(() => throw new ArgumentException(result.Error));
        
        return result.Value;
    }
}
