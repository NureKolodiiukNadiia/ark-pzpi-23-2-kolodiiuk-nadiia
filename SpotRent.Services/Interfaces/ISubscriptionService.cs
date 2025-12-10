using SpotRent.Domain.Common;
using SpotRent.Domain.Entities;
using SpotRent.Services.Subscriptions;

namespace SpotRent.Services.Interfaces;

public interface ISubscriptionService
{
    Task<Result<SubscriptionCreationResponse>> SubscribeAsync(int userId, int subscriptionPlanId);

    Task<Result<SubscriptionDto>> GetCurrentUserSubscriptionAsync(int userId);

    Task<Result<IEnumerable<SubscriptionInfo>>> GetSubscriptionHistoryAsync(int userId);

    Task<Result<Subscription>> GetSubscriptionByIdAsync(int id);

    Task<Result> ChangeSubscriptionAsync(int currSubscriptionId, int newPlanId, int userId);

    Task<Result> CancelSubscriptionAsync(int subscriptionId, int userId);
}
