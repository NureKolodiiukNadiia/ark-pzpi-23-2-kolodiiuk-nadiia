using SpotRent.Domain.Common;
using SpotRent.Domain.Entities;
using SpotRent.Infrastructure;
using SpotRent.Services.Interfaces;

namespace SpotRent.Services.Subscriptions;

public class SubscriptionService : ISubscriptionService
{
    private readonly SpotRentDbContext _context;

    public SubscriptionService(SpotRentDbContext context)
    {
        _context = context;
    }

    public async Task<Result<IEnumerable<Subscription>>> GetAllSubscriptionsAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<Result<Subscription>> GetSubscriptionByIdAsync(int id)
    {
        var subscription = await _context.Subscriptions.FindAsync(id);

        return (subscription == null)
            ? Result.Fail<Subscription>("No subscription with specified id")
            : Result.Success(subscription);
    }

    public async Task<Result<int>> AddSubscriptionAsync(Subscription subscription)
    {
        throw new NotImplementedException();
    }

    public Task<Result<Subscription>> UpdateSubscriptionAsync(Subscription subscription)
    {
        throw new NotImplementedException();
    }

    public async Task<Result> DeleteSubscriptionAsync(int id)
    {
        var subscription = await _context.Subscriptions.FindAsync(id);
        if (subscription == null)
        {
            return Result.Fail("No subscription with specified id");
        }

        try
        {
            _context.Remove(subscription);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            return Result.Fail($"{e.Message}");
        }

        return Result.Success();
    }
}
