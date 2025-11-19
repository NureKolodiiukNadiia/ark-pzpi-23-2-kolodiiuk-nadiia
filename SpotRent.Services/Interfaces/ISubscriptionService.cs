using SpotRent.Domain.Common;
using SpotRent.Domain.Entities;
using SpotRent.Services.Subscriptions;

namespace SpotRent.Services.Interfaces;

public interface ISubscriptionService
{
    Task<Result<int>> SubscribeAsync(int userId, int subscriptionPlanId);

    Task<Result<Subscription>> GetCurrentUserSubscriptionAsync(int userId);

    Task<Result<SubscriptionHistory>> GetSubscriptionHistoryAsync(int userId, int page, int pageSize);

    Task<Result<Subscription>> GetSubscriptionByIdAsync(int id);

    Task<Result<Subscription>> ChangeSubscriptionAsync(int currSubscriptionId, int newPlanId);

    Task<Result> CancelSubscriptionAsync(int subscriptionId);
}
