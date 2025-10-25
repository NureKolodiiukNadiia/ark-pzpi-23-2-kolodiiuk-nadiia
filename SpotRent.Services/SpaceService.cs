using SpotRent.Domain.Entities;
using SpotRent.Infrastructure;
using SpotRent.Services.Interfaces;

namespace SpotRent.Services;

public class SpaceService : ISpaceService
{
    private readonly SpotRentDbContext _context;

    public SpaceService(SpotRentDbContext context)
    {
        _context = context;
    }

    public Task<IEnumerable<Space>> GetAllSpacesAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Space?> GetSpaceByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Space>> GetAvailableSpacesAsync(DateTime startTime, DateTime endTime)
    {
        throw new NotImplementedException();
    }

    public Task<Space> CreateSpaceAsync(Space space)
    {
        throw new NotImplementedException();
    }

    public Task<Space> UpdateSpaceAsync(Space space)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteSpaceAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsSpaceAvailableAsync(int workspaceId, DateTime startTime, DateTime endTime)
    {
        throw new NotImplementedException();
    }

    public Task<Space> GetSpaceByDeviceIdAsync(string deviceId)
    {
        throw new NotImplementedException();
    }
}
