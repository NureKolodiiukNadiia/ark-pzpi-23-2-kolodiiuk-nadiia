using SpotRent.Domain.Common;
using SpotRent.Domain.Entities;

namespace SpotRent.Services.Interfaces;

public interface IBookingService
{
    Task<Result<IEnumerable<Booking>>> GetUserBookingsAsync(int userId);

    Task<Result<IEnumerable<Booking>>> GetAllBookingsAsync();

    Task<Result<Booking>> GetBookingByIdAsync(int id);

    Task<Result<Booking>> CreateBookingAsync(Booking booking);

    Task<Result<Booking>> UpdateBookingAsync(Booking booking);

    Task<Result> DeleteBookingAsync(int id);

    // Task<decimal> CalculateBookingCostAsync(int workspaceId, DateTime startTime, DateTime endTime);
    // Task<bool> IsBookingActiveAsync(int bookingId);
    // Task<Booking> GetActiveBookingAsync(int userId, int workspaceId);
    //
    // Task<IEnumerable<Booking>> GetBookingsByDateRangeAsync(DateTime startDate, DateTime endDate);
}
