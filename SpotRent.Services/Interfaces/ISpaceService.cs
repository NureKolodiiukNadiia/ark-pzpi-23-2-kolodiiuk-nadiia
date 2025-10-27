using SpotRent.Domain.Entities;

namespace SpotRent.Services.Interfaces;

public interface ISpaceService
{
    Task<IEnumerable<Space>> GetAllSpacesAsync();

    Task<Space> GetSpaceByIdAsync(int id);

    Task<IEnumerable<Space>> GetAvailableSpacesAsync(DateTime startTime, DateTime endTime);

    Task<Space> CreateSpaceAsync(Space space);

    Task<Space> UpdateSpaceAsync(Space space);

    Task<bool> DeleteSpaceAsync(int id);

    Task<bool> IsSpaceAvailableAsync(int workspaceId, DateTime startTime, DateTime endTime);

    Task<Space> GetSpaceByDeviceIdAsync(string deviceId);
    // Task<Space?> GetSpaceByDeviceIdAsync(string deviceId);
}