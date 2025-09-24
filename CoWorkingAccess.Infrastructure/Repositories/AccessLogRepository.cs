using CoWorkingAccess.Domain.Entities;
using CoWorkingAccess.Domain.Interfaces.Repositories;

namespace CoWorkingAccess.Infrastructure.Repositories;

public class AccessLogRepository : GenericRepository<AccessLog>, IAccessLogRepository
{
    public AccessLogRepository(CoWorkingAccessDbContext context) : base(context)
    {
    }
}
