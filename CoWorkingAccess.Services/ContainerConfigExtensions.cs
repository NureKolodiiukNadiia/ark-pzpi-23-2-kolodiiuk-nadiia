using CoWorkingAccess.Domain.Interfaces;
using CoWorkingAccess.Services.Auth;
using CoWorkingAccess.Services.Payment;
using Microsoft.Extensions.DependencyInjection;

namespace CoWorkingAccess.Services;

public static class ContainerConfigExtensions
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ISubscriptionService, SubscriptionService>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddTransient<TokenHelper, TokenHelper>();
    }
}
