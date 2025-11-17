using System.Diagnostics;
using Microsoft.Extensions.Logging;
using SpotRent.Domain.Common;
using SpotRent.Domain.Entities;
using SpotRent.Infrastructure;
using SpotRent.Services.Interfaces;

namespace SpotRent.Services.Bookings;

public class BookingService : IBookingService
{
    private readonly SpotRentDbContext _context;
    
    private readonly ILogger<BookingService> _logger;

    public BookingService(SpotRentDbContext context, ILogger<BookingService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<Booking>> CreateBookingAsync(Booking booking)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<IEnumerable<Booking>>> GetUserBookingsHistoryAsync(int userId)
    {
        throw new NotImplementedException();
    }

    public Task<Result<IEnumerable<Booking>>> GetUserActiveBookingsAsync(int userId)
    {
        throw new NotImplementedException();
    }

    public Task<Result<IEnumerable<Booking>>> GetOwnerBookingsAsync(int ownerId)
    {
        throw new NotImplementedException();
    }

    public Task<Result<IEnumerable<Booking>>> GetBookingsAsync(BookingFilterRequest filterRequest)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<Booking>> GetBookingByIdAsync(int id)
    {
        var booking = await _context.Bookings.FindAsync(id);

        return (booking == null)
            ? Result.Fail<Booking>("No booking with specified id")
            : Result.Success(booking);
    }

    public async Task<Result<Booking>> UpdateBookingAsync(Booking booking)
    {
        throw new NotImplementedException();
    }

    public async Task<Result> CancelBookingAsync(int id)
    {
        var booking = await _context.Bookings.FindAsync(id);
        if (booking == null)
        {
            return Result.Fail("No booking with specified id");
        }

        try
        {
            _context.Remove(booking);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            return Result.Fail($"{e.Message}");
        }

        return Result.Success();
    }
}
