using Google.Apis.Auth;
using SpotRent.Domain.Common;
using SpotRent.Domain.Entities;
using SpotRent.Services.Auth;

namespace SpotRent.Services.Interfaces;

public interface IAuthService
{
    Task<Result<User>> ValidateUserCredentials(string email, string password);

    Task<(string token, string refreshToken)> GenerateTokens(User user);

    Task<Result<GoogleJsonWebSignature.Payload>> ValidateGoogleSignInRequestAsync(string idToken);

    Task<Result> RegisterAsync(User user, string password, string phoneNumber, string firstName, string lastName);

    Task<Result<RefreshTokenResponse>> RefreshTokenAsync(string token);

    Task<Result> LogoutAsync(string token);

    Task<Result<User>> GetUserAsync(int userId);

    Task<Result<User>> GetOrCreateUser(string payloadEmail, string payloadName, string payloadSubject);
}
