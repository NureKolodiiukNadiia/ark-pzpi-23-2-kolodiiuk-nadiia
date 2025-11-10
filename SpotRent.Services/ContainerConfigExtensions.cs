using Microsoft.Extensions.DependencyInjection;
using SpotRent.Services.Auth;
using SpotRent.Services.Interfaces;
using SpotRent.Services.Payment;
using SpotRent.Services.Spaces;

namespace SpotRent.Services;

public static class ContainerConfigExtensions
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<ISpaceService, SpaceService>();
    }
}
