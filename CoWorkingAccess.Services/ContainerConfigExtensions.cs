using CoWorkingAccess.Domain.Interfaces.Services;
using CoWorkingAccess.Services.Auth;
using CoWorkingAccess.Services.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace CoWorkingAccess.Services;

public static class ContainerConfigExtensions
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ISubscriptionService, SubscriptionService>();
        services.AddTransient<TokenHelper, TokenHelper>();
    }
}
