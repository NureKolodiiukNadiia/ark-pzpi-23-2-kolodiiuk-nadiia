using SpotRent.Domain.Common;
using SpotRent.Domain.Entities;
using SpotRent.Services.Spaces;

namespace SpotRent.Services.Interfaces;

public interface ISpaceService
{
    Task<Result<IEnumerable<Space>>> FilterSpacesAsync(SpaceFilterRequest req);

    Task<Result<Space>> GetSpaceByIdAsync(int id);

    Task<Result<IEnumerable<Space>>> GetAvailableSpacesAsync(DateTime startTime, DateTime endTime, string city);

    Task<Result<Space>> CreateSpaceAsync(Space space);

    Task<Result> UpdateSpaceAsync(Space space, int ownerId);

    Task<Result> DeleteSpaceAsync(int id, int ownerId);

    Task<Result<bool>> IsSpaceAvailableAsync(int workspaceId, DateTime startTime, DateTime endTime);

    Task<Result<SpaceSchedule>> GetSpaceScheduleAsync(int spaceId, DateTime? startDate, DateTime? endDate);
}
