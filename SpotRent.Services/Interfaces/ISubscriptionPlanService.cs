using SpotRent.Domain.Common;
using SpotRent.Domain.Entities;
using SpotRent.Services.Subscriptions;

namespace SpotRent.Services.Interfaces;

public interface ISubscriptionPlanService
{
    Task<Result<IEnumerable<SubscriptionPlanDto>>> GetPlansAsync();

    Task<Result<SubscriptionPlanDto>> GetPlanByIdAsync(int id);

    Task<Result> CreateSubscriptionPlanAsync(CreateSubscriptionPlanDto subscriptionPlanDto);

    Task<Result> UpdateSubscriptionPlanAsync(int id, UpdateSubscriptionPlanDto subscriptionPlanDto);

    Task<Result> DeactivateSubscriptionPlanAsync(int subscriptionPlanId);

    Task<Result> DeleteSubscriptionPlanAsync(int subscriptionPlanId);
}
