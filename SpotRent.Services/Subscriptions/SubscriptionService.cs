using Microsoft.Extensions.Logging;
using SpotRent.Domain.Common;
using SpotRent.Domain.Entities;
using SpotRent.Infrastructure;
using SpotRent.Services.Interfaces;

namespace SpotRent.Services.Subscriptions;

public class SubscriptionService : ISubscriptionService
{
    private readonly SpotRentDbContext _context;

    private readonly ILogger<SubscriptionService> _logger;

    public SubscriptionService(SpotRentDbContext context, ILogger<SubscriptionService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<int>> SubscribeAsync(int userId, int subscriptionPlanId)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<Subscription>> GetCurrentUserSubscriptionAsync(int userId)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<SubscriptionHistory>> GetSubscriptionHistoryAsync(int userId, int page, int pageSize)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<IEnumerable<SubscriptionPlan>>> GetPlansAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<Result<SubscriptionPlan>> GetPlanByIdAsync(int id)
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

    public async Task<Result<Subscription>> ChangeSubscriptionAsync(int currSubscriptionId, int newPlanId)
    {
        throw new NotImplementedException();
    }

    public async Task<Result> CancelSubscriptionAsync(int subscriptionId)
    {
        throw new NotImplementedException();
        var subscription = await _context.Subscriptions.FindAsync(subscriptionId);
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
