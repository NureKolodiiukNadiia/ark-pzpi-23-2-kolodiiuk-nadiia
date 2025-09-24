using CoWorkingAccess.Domain.Entities;
using CoWorkingAccess.Domain.Interfaces.Repositories;

namespace CoWorkingAccess.Infrastructure.Repositories;

public class DeviceRepository : GenericRepository<Device>, IDeviceRepository
{
    public DeviceRepository(CoWorkingAccessDbContext context) : base(context)
    {
    }
}