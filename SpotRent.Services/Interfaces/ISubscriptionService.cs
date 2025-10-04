using SpotRent.Domain.Entities;

namespace SpotRent.Services.Interfaces;

public interface ISubscriptionService
{
    Task<IEnumerable<Subscription>> GetAvailableSubscriptionsAsync();
    
    Task<IEnumerable<Subscription>> GetAllSubscriptionsAsync();
    
    Task<Subscription> GetSubscriptionByIdAsync(int id);
    
    Task<int> AddSubscriptionAsync(Subscription subscription);
    
    Task UpdateSubscriptionAsync(Subscription subscription);
    
    Task DeleteSubscriptionAsync(int id);
}