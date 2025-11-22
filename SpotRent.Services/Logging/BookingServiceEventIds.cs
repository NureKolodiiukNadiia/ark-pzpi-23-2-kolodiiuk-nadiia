using Microsoft.Extensions.Logging;
using SpotRent.Services.Bookings;

namespace SpotRent.Services.Logging;

internal static class BookingServiceEventIds
{
    internal static readonly EventId GetBookingById = new EventId(5201, nameof(BookingService.GetBookingByIdAsync));
}
