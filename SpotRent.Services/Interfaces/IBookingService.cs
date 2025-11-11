using SpotRent.Domain.Common;
using SpotRent.Domain.Entities;

namespace SpotRent.Services.Interfaces;

public interface IBookingService
{
    Task<Result<Booking>> CreateBookingAsync(Booking booking);
    
    Task<Result<IEnumerable<Booking>>> GetUserBookingsHistoryAsync(int userId);

    Task<Result<IEnumerable<Booking>>> GetUserActiveBookingsAsync(int userId);

    Task<Result<IEnumerable<Booking>>> GetOwnerBookingsAsync(int ownerId);

    Task<Result<IEnumerable<Booking>>> GetBookingsAsync(BookingFilterRequest filterRequest);

    Task<Result<Booking>> GetBookingByIdAsync(int id);

    Task<Result<Booking>> UpdateBookingAsync(Booking booking);

    Task<Result> CancelBookingAsync(int id);
}

public class BookingFilterRequest
{
}
