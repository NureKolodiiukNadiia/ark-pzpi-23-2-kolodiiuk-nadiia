using CoWorkingAccess.Domain.Common;
using CoWorkingAccess.Domain.Entities;

namespace CoWorkingAccess.Domain.Interfaces.Repositories;

public interface ISubscriptionRepository : IGenericRepository<Subscription>
{
    public Task<Result<IEnumerable<Subscription>>> GetAllAvailableSubscriptionsAsync();
}