using System.Security.Claims;
using SpotRent.Domain.Entities;

namespace SpotRent.Services.Interfaces;

public interface IJwtService
{
    string GenerateToken(User user);

    string GenerateRefreshToken();

    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}
