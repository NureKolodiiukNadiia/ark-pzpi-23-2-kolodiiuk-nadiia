using Microsoft.Extensions.DependencyInjection;
using SpotRent.Services.Auth;
using SpotRent.Services.Bookings;
using SpotRent.Services.Interfaces;
using SpotRent.Services.Payment;

namespace SpotRent.Services;

public static class ContainerConfigExtensions
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IBookingService, BookingService>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<IJwtService, JwtService>();
    }
}
