using Microsoft.EntityFrameworkCore;
using SpotRent.Domain.Entities;
using SpotRent.Infrastructure;
using SpotRent.Services.Interfaces;

namespace SpotRent.Services;

public class SubscriptionService : ISubscriptionService
{
    private readonly SpotRentDbContext _context;

    public SubscriptionService(SpotRentDbContext context)
    {
        _context = context;
    }

    public Task<IEnumerable<Subscription>> GetAvailableSubscriptionsAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Subscription>> GetAllSubscriptionsAsync()
    {
        var result = _context.Subscriptions.AsNoTracking();
        // result.OnFailure(() => throw new Exception("Failure getting subscriptions"));

        return result;
    }

    public async Task<Subscription> GetSubscriptionByIdAsync(int id)
    {
        var result = await _context.Subscriptions.FindAsync(id);
        // result.OnFailure(() => throw new Exception("Failure getting subscriptions"));

        return result;
    }

    public async Task<int> AddSubscriptionAsync(Subscription subscription)
    {
        throw new NotImplementedException();
        // var createdSubscription = await _subscriptionRepository.CreateAsync(subscription);
        // var result = await _subscriptionRepository.SaveChangesAsync();
        // result.OnFailure(() => throw new Exception("Failure adding a new subscription"));
        //
        // return createdSubscription.Id;
    }

    public async Task UpdateSubscriptionAsync(Subscription subscription)
    {
        throw new NotImplementedException();
        // await _subscriptionRepository.UpdateAsync(subscription);
        // var result = await _subscriptionRepository.SaveChangesAsync();
        // result.OnFailure(() => throw new Exception("Failure updating a subscription"));
    }

    public async Task DeleteSubscriptionAsync(int id)
    {
        throw new NotImplementedException();
        // await _subscriptionRepository.DeleteAsync(id);
        // var result = await _subscriptionRepository.SaveChangesAsync();
        // result.OnFailure(() => throw new Exception("Failure deleting a subscription"));
    }
}
