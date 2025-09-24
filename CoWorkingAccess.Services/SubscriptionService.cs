using CoWorkingAccess.Domain.Entities;
using CoWorkingAccess.Domain.Extensions;
using CoWorkingAccess.Domain.Interfaces.Repositories;
using CoWorkingAccess.Domain.Interfaces.Services;

namespace CoWorkingAccess.Services;

public class SubscriptionService : ISubscriptionService
{
    private readonly ISubscriptionRepository _subscriptionRepository;

    public SubscriptionService(ISubscriptionRepository subscriptionRepository)
    {
        _subscriptionRepository = subscriptionRepository;
    }

    public Task<IEnumerable<Subscription>> GetAvailableSubscriptionsAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Subscription>> GetAllSubscriptionsAsync()
    {
        var result = await _subscriptionRepository.GetAllAsync();
        result.OnFailure(() => throw new Exception("Failure getting subscriptions"));

        return result.Value;
    }

    public async Task<Subscription> GetSubscriptionByIdAsync(int id)
    {
        var result = await _subscriptionRepository.GetByIdAsync(id);
        result.OnFailure(() => throw new Exception("Failure getting subscriptions"));

        return result.Value;
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
