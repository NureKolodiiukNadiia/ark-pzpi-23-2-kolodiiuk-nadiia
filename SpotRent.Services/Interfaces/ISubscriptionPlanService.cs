using SpotRent.Domain.Common;
using SpotRent.Domain.Entities;

namespace SpotRent.Services.Interfaces;

public interface ISubscriptionPlanService
{

    Task<Result<IEnumerable<SubscriptionPlan>>> GetPlansAsync();

    Task<Result<SubscriptionPlan>> GetPlanByIdAsync(int id);

    Task<Result> CreateSubscriptionPlanAsync(CreateSubscriptionDto subscriptionDto);

    Task<Result> UpdateSubscriptionPlanAsync(UpdateSubscriptionPlanDto subscriptionPlanDto);

    Task<Result> DeleteSubscriptionPlanAsync(int subscriptionPlanId);
}
