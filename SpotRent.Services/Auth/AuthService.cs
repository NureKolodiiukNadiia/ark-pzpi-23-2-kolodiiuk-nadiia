using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SpotRent.Domain.Common;
using SpotRent.Domain.Entities;
using SpotRent.Infrastructure;
using SpotRent.Services.Interfaces;

namespace SpotRent.Services.Auth;

public class AuthService : IAuthService
{
    private readonly SpotRentDbContext _context;

    private readonly IJwtService _jwtService;

    private readonly UserManager<User> _userManager;

    private readonly IConfiguration _configuration;

    private IHttpContextAccessor _httpContextAccessor;

    public AuthService(SpotRentDbContext context,
        UserManager<User> userManager,
        IJwtService jwtService,
        IHttpContextAccessor httpContextAccessor,
        IConfiguration configuration)
    {
        _context = context;
        _userManager = userManager;
        _jwtService = jwtService;
        _httpContextAccessor = httpContextAccessor;
        _configuration = configuration;
    }

    public async Task<Result> RegisterAsync(User user, string password, 
        string phoneNumber, string firstName, string lastName)
    {
        user.PhoneNumber = phoneNumber ?? "";
        user.UserName = user.Email;
        user.FirstName = firstName;
        user.LastName = lastName;

        var result = await _userManager.CreateAsync(user, password);
        if (!result.Succeeded)
        {
            return Result.Fail($"Failed to create a user: {string.Join(", ", 
                    result.Errors.Select(e => e.Description))}");
        }

        var roleResult = await _userManager.AddToRoleAsync(user, user.Role.ToString());
        if (!roleResult.Succeeded)
        {
            return Result.Fail(
                $"Failed to assign role: {string.Join(", ", 
                    roleResult.Errors.Select(e => e.Description))}");
        }

        return Result.Success();
    }

    public async Task<Result<User>> ValidateUserCredentials(string email, string password)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);

        if (user == null || !(await VerifyPasswordAsync(user, user.PasswordHash)))
        {
            return Result.Fail<User>("Invalid email or password");
        }

        return Result.Success(user);
    }

    public async Task<(string, string)> GenerateTokens(User user)
    {
        var token = _jwtService.GenerateToken(user);
        var refreshToken = _jwtService.GenerateRefreshToken();

        var userRefreshToken = new UserRefreshToken
        {
            UserId = user.Id,
            Token = refreshToken,
            Expires = DateTime.UtcNow.AddDays(Convert.ToDouble(_configuration["Jwt:RefreshTokenExpirationDays"])),
            CreatedAt = DateTime.UtcNow,
            CreatedByIp = GetIpAddress()
        };

        _context.UserRefreshTokens.Add(userRefreshToken);

        var oldTokens = _context.UserRefreshTokens
            .Where(rt => rt.UserId == user.Id && rt.Expires < DateTime.UtcNow);
        _context.UserRefreshTokens.RemoveRange(oldTokens);

        await _context.SaveChangesAsync();

        return new ValueTuple<string, string>(token, refreshToken);
    }

    public async Task<Result<RefreshTokenResponse>> RefreshTokenAsync(string token)
    {
        var storedRefreshToken = await _context.UserRefreshTokens
            .Include(rt => rt.User)
            .FirstOrDefaultAsync(rt => rt.Token == token && rt.Expires > DateTime.UtcNow);

        if (storedRefreshToken == null)
        {
            return Result.Fail<RefreshTokenResponse>("Invalid refresh token");
        }

        if (storedRefreshToken.Revoked != null)
        {
            return Result.Fail<RefreshTokenResponse>("Token revoked");
        }

        var newToken = _jwtService.GenerateToken(storedRefreshToken.User);
        var newRefreshToken = _jwtService.GenerateRefreshToken();

        storedRefreshToken.Revoked = DateTime.UtcNow;
        storedRefreshToken.RevokedByIp = GetIpAddress();
        storedRefreshToken.ReplacedByToken = newRefreshToken;
        var userRefreshToken = new UserRefreshToken
        {
            UserId = storedRefreshToken.UserId,
            Token = newRefreshToken,
            Expires = DateTime.UtcNow.AddDays(Convert.ToDouble(_configuration["Jwt:RefreshTokenExpirationDays"])),
            CreatedAt = DateTime.UtcNow,
            CreatedByIp = GetIpAddress()
        };
        _context.UserRefreshTokens.Add(userRefreshToken);
        await _context.SaveChangesAsync();

        return Result.Success(new RefreshTokenResponse
        {
            Token = newToken,
            RefreshToken = newRefreshToken,
            Id = storedRefreshToken.User.Id,
            Email = storedRefreshToken.User.Email,
            FirstName = storedRefreshToken.User.FirstName,
            LastName = storedRefreshToken.User.LastName
        });
    }

    public async Task<Result> LogoutAsync(string token)
    {
        var refreshToken = await _context.UserRefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == token);

        if (refreshToken != null)
        {
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = GetIpAddress();
            await _context.SaveChangesAsync();
        }
        else
        {
            return Result.Fail("Refresh token wasn't found");
        }

        return Result.Success();
    }

    public async Task<Result<User>> GetUserAsync(int userId)
    {
        var user = await _context.Users
            .Where(u => u.Id == userId)
            .FirstOrDefaultAsync();
        if (user == null)
        {
            return Result.Fail<User>("User is not found");
        }

        return Result.Success(user);
    }

    private async Task<bool> VerifyPasswordAsync(User user, string password)
    {
        return await _userManager.CheckPasswordAsync(user, password);
    }

    private string GetIpAddress()
    {
        var context = _httpContextAccessor.HttpContext;
        if (context == null)
        {
            return null;
        }

        if (context.Request.Headers.ContainsKey("X-Forwarded-For"))
        {
            return context.Request.Headers["X-Forwarded-For"].ToString();
        }

        return context.Connection.RemoteIpAddress?.MapToIPv4().ToString() ?? "0";
    }
}