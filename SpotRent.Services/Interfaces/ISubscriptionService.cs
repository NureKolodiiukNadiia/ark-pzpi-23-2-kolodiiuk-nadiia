using SpotRent.Domain.Common;
using SpotRent.Domain.Entities;
using SpotRent.Services.Subscriptions;

namespace SpotRent.Services.Interfaces;

public interface ISubscriptionService
{
    Task<Result<Subscription>> GetCurrentUserSubscriptionAsync(int userId);

    Task<Result<Subscription>> GetSubscriptionByIdAsync(int id);

    Task<Result<IEnumerable<SubscriptionPlan>>> GetPlansAsync();
    
    Task<Result<SubscriptionPlan>> GetPlanByIdAsync(int id);

    Task<Result<int>> SubscribeAsync(int userId, int subscriptionPlanId);

    Task<Result<SubscriptionHistory>> GetSubscriptionHistoryAsync(int userId, int page, int pageSize);

    Task<Result> CancelSubscriptionAsync(int subscriptionId);

    Task<Result<Subscription>> ChangeSubscriptionAsync(int currSubscriptionId, int newPlanId);
}
