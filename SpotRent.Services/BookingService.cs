using SpotRent.Domain.Entities;
using SpotRent.Services.Interfaces;

namespace SpotRent.Services;

public class BookingService : IBookingService
{
    public Task<IEnumerable<Booking>> GetUserBookingsAsync(int userId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Booking>> GetAllBookingsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Booking> GetBookingByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Booking> CreateBookingAsync(Booking booking)
    {
        throw new NotImplementedException();
    }

    public Task<Booking> UpdateBookingAsync(Booking booking)
    {
        throw new NotImplementedException();
    }

    public Task<bool> CancelBookingAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<decimal> CalculateBookingCostAsync(int workspaceId, DateTime startTime, DateTime endTime)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsBookingActiveAsync(int bookingId)
    {
        throw new NotImplementedException();
    }

    public Task<Booking> GetActiveBookingAsync(int userId, int workspaceId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Booking>> GetBookingsByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        throw new NotImplementedException();
    }
}