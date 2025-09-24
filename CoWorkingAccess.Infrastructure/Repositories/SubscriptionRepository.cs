using CoWorkingAccess.Domain.Common;
using CoWorkingAccess.Domain.Entities;
using CoWorkingAccess.Domain.Interfaces.Repositories;

namespace CoWorkingAccess.Infrastructure.Repositories;

public class SubscriptionRepository : GenericRepository<Subscription>, ISubscriptionRepository
{
    public SubscriptionRepository(CoWorkingAccessDbContext context) : base(context)
    {
    }

    public Task<Result<Subscription>> CreateAsync(Subscription entity)
    {
        throw new NotImplementedException();
    }

    public Task<Result> UpdateAsync(Subscription entity)
    {
        throw new NotImplementedException();
    }

    public Task<Result> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Result<IEnumerable<Subscription>>> GetAllAvailableSubscriptionsAsync()
    {
        throw new NotImplementedException();
    }
}
