using SpotRent.Domain.Common;
using SpotRent.Domain.Entities;
using SpotRent.Services.Bookings;

namespace SpotRent.Services.Interfaces;

public interface IBookingService
{
    Task<Result<BookingCreationResponse>> CreateBookingAsync(int userId, CreateBookingRequest req);
    
    Task<Result<IEnumerable<Booking>>> GetUserBookingsHistoryAsync(int userId);

    Task<Result<IEnumerable<Booking>>> GetUserActiveBookingsAsync(int userId);

    Task<Result<IEnumerable<Booking>>> GetOwnerBookingsAsync(int ownerId);

    Task<Result<IEnumerable<Booking>>> GetOwnerActiveBookingsAsync(int ownerId);

    Task<Result<IEnumerable<Booking>>> GetBookingsAsync(int requesterId, BookingFilterRequest req);

    Task<Result<Booking>> GetBookingByIdAsync(int id, int requesterId);

    Task<Result> CancelBookingAsync(int bookingId);
}
