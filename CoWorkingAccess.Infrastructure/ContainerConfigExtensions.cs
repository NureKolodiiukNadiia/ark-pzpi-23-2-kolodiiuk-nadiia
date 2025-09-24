using CoWorkingAccess.Domain.Interfaces.Repositories;
using CoWorkingAccess.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace CoWorkingAccess.Infrastructure;

public static class ContainerConfigExtensions
{
    public static void RegisterRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
    }
}
