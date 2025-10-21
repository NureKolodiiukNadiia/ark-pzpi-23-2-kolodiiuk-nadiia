using Microsoft.Extensions.DependencyInjection;
using SpotRent.Services.Auth;
using SpotRent.Services.Interfaces;
using SpotRent.Services.Payment;

namespace SpotRent.Services;

public static class ContainerConfigExtensions
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IAccessLogService, AccessLogService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<ISmartLockService, SmartLockService>();
    }
}
