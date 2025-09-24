using CoWorkingAccess.Domain.Entities;
using CoWorkingAccess.Domain.Interfaces.Repositories;

namespace CoWorkingAccess.Infrastructure.Repositories;

public class SubscriptionPlanRepository : GenericRepository<SubscriptionPlan>, ISubscriptionPlanRepository
{
    public SubscriptionPlanRepository(CoWorkingAccessDbContext context) : base(context)
    {
    }
}