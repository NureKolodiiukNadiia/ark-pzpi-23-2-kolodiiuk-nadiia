using System.Diagnostics;
using SpotRent.Domain.Common;
using SpotRent.Domain.Entities;
using SpotRent.Infrastructure;
using SpotRent.Services.Interfaces;

namespace SpotRent.Services.Bookings;

public class BookingService : IBookingService
{
    private readonly SpotRentDbContext _context;

    public BookingService(SpotRentDbContext context)
    {
        _context = context;
    }

    public async Task<Result<IEnumerable<Booking>>> GetUserBookingsAsync(int userId)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<IEnumerable<Booking>>> GetAllBookingsAsync()
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

    public async Task<Result<Booking>> CreateBookingAsync(Booking booking)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<Booking>> UpdateBookingAsync(Booking booking)
    {
        throw new NotImplementedException();
    }

    public async Task<Result> DeleteBookingAsync(int id)
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
