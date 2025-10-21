using SpotRent.Domain.Common;
using SpotRent.Domain.Entities;

namespace SpotRent.Services.Interfaces;

public interface ISubscriptionService
{
    Task<Result<IEnumerable<Subscription>>> GetAllSubscriptionsAsync();

    Task<Result<Subscription>> GetSubscriptionByIdAsync(int id);

    Task<Result<int>> AddSubscriptionAsync(Subscription subscription);

    Task<Result<Subscription>> UpdateSubscriptionAsync(Subscription subscription);

    Task<Result> DeleteSubscriptionAsync(int id);
}