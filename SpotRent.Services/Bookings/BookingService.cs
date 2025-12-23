using System.Linq.Expressions;
using System.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;
using SpotRent.Domain.Common;
using SpotRent.Domain.Entities;
using SpotRent.Domain.Enums;
using SpotRent.Domain.Extensions;
using SpotRent.Infrastructure;
using SpotRent.Services.Interfaces;
using SpotRent.Services.Logging;
using SpotRent.Services.Payment;

namespace SpotRent.Services.Bookings;

public class BookingService : BaseService<BookingService>, IBookingService
{
    private readonly IPaymentService _paymentService;

    private readonly ISpaceService _spaceService;

    public BookingService(
        SpotRentDbContext context,
        ILogger<BookingService> logger,
        IPaymentService paymentService,
        ISpaceService spaceService)
        : base(context, logger)
    {
        _paymentService = paymentService;
        _spaceService = spaceService;
    }

    public async Task<Result<BookingCreationResponse>> CreateBookingAsync(int userId, CreateBookingRequest req)
    {
        using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        Space space;
        try
        {
            var userExists = await Context.Users.AnyAsync(u => u.Id == userId);
            if (!userExists)
            {
                return Result.Fail<BookingCreationResponse>("No user with specified id");
            }

            space = await Context.Spaces.FindAsync(req.SpaceId);
            if (space is null)
            {
                return Result.Fail<BookingCreationResponse>($"Space with id {req.SpaceId} doesn't exist");
            }

            var isSpaceAvailableRes =
                await _spaceService.IsSpaceAvailableAsync(req.SpaceId, req.StartTime, req.EndTime);
            if (!isSpaceAvailableRes.IsSuccess || !isSpaceAvailableRes.Value)
            {
                return Result.Fail<BookingCreationResponse>($"Space with id {req.SpaceId} is not available");
            }

            var subscription = await Context.Subscriptions.FirstOrDefaultAsync((s) =>
                s.IsActive() && s.UserId == userId && s.SubscriptionPlan.OwnerId == space.OwnerId);
            if (subscription != null)
            {
                var timeDiff = req.EndTime - req.StartTime;
                var hoursAfterSubscriptionUsage = subscription.HoursUsed - timeDiff.Hours;
                if (hoursAfterSubscriptionUsage < 0)
                {
                    return Result.Fail<BookingCreationResponse>("Can't be paid with subscription");
                }
            }

            var booking = await CreateBooking();

            if (booking is null)
            {
                return Result.Fail<BookingCreationResponse>("Invalid duration");
            }

            Result<LiqPayPaymentData> paymentDataResult = null;
            if (subscription == null)
            {
                paymentDataResult = await _paymentService.CreatePaymentAsync(booking.Id, booking.TotalAmount);
                if (paymentDataResult.Failure)
                {
                    return Result.Fail<BookingCreationResponse>($"{paymentDataResult.Error}");
                }
            }

            scope.Complete();

            if (subscription == null)
            {
                return Result.Success(new BookingCreationResponse
                    { BookingId = booking.Id, LiqPayPaymentData = paymentDataResult.Value });
            }

            return Result.Success(new BookingCreationResponse() { BookingId = booking.Id, LiqPayPaymentData = null });
        }
        catch (NpgsqlException e)
        {
            Log(LogLevel.Error, BookingServiceEventIds.GetBookingById,
                "DB error creating booking. Error: {error}", e.Message);

            return Result.Fail<BookingCreationResponse>($"DB error: {e.Message}.");
        }
        catch (Exception e)
        {
            Log(LogLevel.Error, BookingServiceEventIds.GetBookingById,
                "Error creating booking. Error: {error}", e.Message);

            return Result.Fail<BookingCreationResponse>($"Failure creating booking: {e.Message}");
        }

        async Task<Booking> CreateBooking()
        {
            var now = DateTime.UtcNow;
            var hours = Convert.ToDecimal((req.EndTime - req.StartTime).TotalHours);

            var booking = new Booking
            {
                UserId = userId,
                SpaceId = req.SpaceId,
                StartTime = req.StartTime,
                EndTime = req.EndTime,
                Status = BookingStatus.Pending,
                TotalAmount = space.HourlyRate * hours,
                PaymentStatus = PaymentStatus.NotPaid,
                TransactionId = 0,
                CreatedAt = now,
                UpdatedAt = now,
                CancelledAt = null
            };

            await Context.AddAsync(booking);
            await Context.SaveChangesAsync();

            return booking;
        }
    }

    public async Task<Result<IEnumerable<Booking>>> GetUserBookingsHistoryAsync(int userId)
    {
        try
        {
            var userExists = await Context.Users.AnyAsync(u => u.Id == userId);
            if (!userExists)
            {
                return Result.Fail<IEnumerable<Booking>>("No user with specified id");
            }

            var history = await Context.Bookings
                .Include(b => b.Space)
                .ThenInclude(s => s.Address)
                .Where(b => b.UserId == userId)
                .ToListAsync();

            return Result.Success<IEnumerable<Booking>>(history);
        }
        catch (NpgsqlException e)
        {
            Log(LogLevel.Error, BookingServiceEventIds.GetBookingById,
                "DB error getting bookings of user {bookingId}. Error: {error}", userId, e.Message);

            return Result.Fail<IEnumerable<Booking>>($"DB error: {e.Message}.");
        }
        catch (Exception e)
        {
            Log(LogLevel.Error, BookingServiceEventIds.GetBookingById,
                "Error getting bookings of user {userId}. Error: {error}", userId, e.Message);

            return Result.Fail<IEnumerable<Booking>>($"Failure getting bookings: {e.Message}");
        }
    }

    public async Task<Result<IEnumerable<Booking>>> GetUserActiveBookingsAsync(int userId)
    {
        try
        {
            var userExists = await Context.Users.AnyAsync(u => u.Id == userId);
            if (!userExists)
            {
                return Result.Fail<IEnumerable<Booking>>("No user with specified id");
            }

            var now = DateTime.UtcNow;
            var history = await Context.Bookings
                .Where(b => b.UserId == userId && b.EndTime >= now && b.CancelledAt == null)
                .Include(b => b.Space)
                .ThenInclude(s => s.Address)
                .AsNoTracking()
                .ToListAsync();

            return Result.Success<IEnumerable<Booking>>(history);
        }
        catch (NpgsqlException e)
        {
            Log(LogLevel.Error, BookingServiceEventIds.GetBookingById,
                "DB error getting booking {bookingId}. Error: {error}", userId, e.Message);

            return Result.Fail<IEnumerable<Booking>>($"DB error: {e.Message}.");
        }
        catch (Exception e)
        {
            Log(LogLevel.Error, BookingServiceEventIds.GetBookingById,
                "Error getting booking {userId}. Error: {error}", userId, e.Message);

            return Result.Fail<IEnumerable<Booking>>($"Failure getting booking: {e.Message}");
        }
    }

    public async Task<Result<IEnumerable<Booking>>> GetOwnerBookingsAsync(int ownerId)
    {
        try
        {
            var ownerExists = await Context.Users.AnyAsync(u => u.Id == ownerId);
            if (!ownerExists)
            {
                return Result.Fail<IEnumerable<Booking>>("No owner with specified id");
            }

            var history = await Context.Bookings
                .Include(b => b.Space)
                .ThenInclude(s => s.Address)
                .Where(b => b.Space.OwnerId == ownerId)
                .ToListAsync();

            return Result.Success<IEnumerable<Booking>>(history);
        }
        catch (NpgsqlException e)
        {
            Log(LogLevel.Error, BookingServiceEventIds.GetBookingById,
                "DB error getting bookings of owner {ownerId}. Error: {error}", ownerId, e.Message);

            return Result.Fail<IEnumerable<Booking>>($"DB error: {e.Message}.");
        }
        catch (Exception e)
        {
            Log(LogLevel.Error, BookingServiceEventIds.GetBookingById,
                "Error getting bookings of owner {ownerId}. Error: {error}", ownerId, e.Message);

            return Result.Fail<IEnumerable<Booking>>($"Failure getting bookings: {e.Message}");
        }
    }

    public async Task<Result<IEnumerable<Booking>>> GetOwnerActiveBookingsAsync(int ownerId)
    {
        try
        {
            var ownerExists = await Context.Users.AnyAsync(u => u.Id == ownerId);
            if (!ownerExists)
            {
                return Result.Fail<IEnumerable<Booking>>("No owner with specified id");
            }

            var now = DateTime.UtcNow;
            var history = await Context.Bookings
                .Include(b => b.Space)
                .ThenInclude(s => s.Address)
                .Where(b => b.Space.OwnerId == ownerId && b.EndTime >= now && b.CancelledAt == null)
                .AsNoTracking()
                .ToListAsync();

            return Result.Success<IEnumerable<Booking>>(history);
        }
        catch (NpgsqlException e)
        {
            Log(LogLevel.Error, BookingServiceEventIds.GetBookingById,
                "DB error getting bookings of owner {ownerId}. Error: {error}", ownerId, e.Message);

            return Result.Fail<IEnumerable<Booking>>($"DB error: {e.Message}.");
        }
        catch (Exception e)
        {
            Log(LogLevel.Error, BookingServiceEventIds.GetBookingById,
                "Error getting bookings of owner {ownerId}. Error: {error}", ownerId, e.Message);

            return Result.Fail<IEnumerable<Booking>>($"Failure getting bookings: {e.Message}");
        }
    }

    public async Task<Result<IEnumerable<Booking>>> GetBookingsAsync(int requesterId, BookingFilterRequest req)
    {
        try
        {
            //todo: check for permission
            var user = await Context.Users.FindAsync(req.UserId);
            Role? parsedRole;
            switch (req.Role)
            {
                case "User":
                    parsedRole = Role.User;
                    break;
                case "Owner":
                    parsedRole = Role.Owner;
                    break;
                default:
                    parsedRole = null;
                    break;
            }

            if (user is null || !parsedRole.HasValue || user.Role != parsedRole.Value)
            {
                return Result.Fail<IEnumerable<Booking>>("Unauthorized");
            }

            var filtered = Context.Bookings
                .Include(b => b.Space)
                .ThenInclude(s => s.Address)
                .Where(BuildCondition());
            req.OrderBy.Invoke(filtered);
            await filtered.Skip(req.SkipCount).Take(req.TakeCount ?? 0).ToListAsync();

            return Result.Success<IEnumerable<Booking>>(filtered);
        }
        catch (NpgsqlException e)
        {
            Log(LogLevel.Error, BookingServiceEventIds.GetBookingById,
                "DB error: {error}", e.Message);

            return Result.Fail<IEnumerable<Booking>>($"DB error: {e.Message}.");
        }
        catch (Exception e)
        {
            Log(LogLevel.Error, BookingServiceEventIds.GetBookingById,
                "Error getting bookings. Error: {error}", e.Message);

            return Result.Fail<IEnumerable<Booking>>($"Failure getting bookings: {e.Message}");
        }

        Expression<Func<Booking, bool>> BuildCondition()
        {
            return b => b.SpaceId == req.SpaceId
                        && b.StartTime == req.StartTime
                        && b.EndTime == req.EndTime
                        && b.Status == req.Status
                        && b.PaymentStatus == req.PaymentStatus;
        }
    }

    public async Task<Result<Booking>> GetBookingByIdAsync(int id, int requesterId)
    {
        try
        {
            //todo: check for permission
            var booking = await Context.Bookings
                .Include(b => b.Space)
                .ThenInclude(s => s.Address)
                .FirstOrDefaultAsync(b => b.Id == id);

            return booking == null
                ? Result.Fail<Booking>("No booking with specified id")
                : Result.Success(booking);
        }
        catch (NpgsqlException e)
        {
            Log(LogLevel.Error, BookingServiceEventIds.GetBookingById,
                "DB error getting booking {bookingId}. Error: {error}", id, e.Message);

            return Result.Fail<Booking>($"DB error: {e.Message}.");
        }
        catch (Exception e)
        {
            Log(LogLevel.Error, BookingServiceEventIds.GetBookingById,
                "Error getting booking {userId}. Error: {error}", id, e.Message);

            return Result.Fail<Booking>($"Failure getting booking: {e.Message}");
        }
    }


    public async Task<Result> CancelBookingAsync(int bookingId)
    {
        try
        {
            var subscription = await Context.Subscriptions.FindAsync(bookingId);
            if (subscription is null)
            {
                return Result.Fail<LiqPayRefundResponse>($"No booking with id {bookingId}");
            }

            var stateTransitionNotValid = subscription.Status ==
                                          (SubscriptionStatus.Cancelled | SubscriptionStatus.Expired);
            if (stateTransitionNotValid)
            {
                return Result.Fail<LiqPayRefundResponse>($"Booking with id {bookingId} can't be cancelled");
            }

            var result = await _paymentService.RefundPaymentAsync(bookingId);
            result.OnSuccess(() => Serilog.Log.Information("Success refunding payment"))
                .OnFailure(() => Serilog.Log.Error(result.Error));

            if (result.Value.Result == "error")
            {
                return Result.Fail<LiqPayRefundResponse>($"Error refunding: {result.Value.Status}");
            }

            subscription.Status = SubscriptionStatus.Cancelled;
            Context.Subscriptions.Update(subscription);
            await Context.SaveChangesAsync();

            return result.Failure
                ? Result.Fail<LiqPayRefundResponse>($"Payment refund failed. Reason: {result.Error}")
                : Result.Success(result.Value);
        }
        catch (Exception e)
        {
            return Result.Fail<LiqPayRefundResponse>($"Error refunding: {e.Message}");
        }
    }
}
