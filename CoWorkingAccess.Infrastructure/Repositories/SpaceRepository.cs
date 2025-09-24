using CoWorkingAccess.Domain.Entities;
using CoWorkingAccess.Domain.Interfaces.Repositories;

namespace CoWorkingAccess.Infrastructure.Repositories;

public class SpaceRepository : GenericRepository<Space>, ISpaceRepository
{
    public SpaceRepository(CoWorkingAccessDbContext context) : base(context)
    {
    }
}