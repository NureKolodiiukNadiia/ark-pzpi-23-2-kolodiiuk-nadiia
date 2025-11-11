using SpotRent.Domain.Common;
using SpotRent.Domain.Entities;
using SpotRent.Services.Spaces;

namespace SpotRent.Services.Interfaces;

public interface ISpaceService
{
    Task<Result<IEnumerable<Space>>> FilterSpacesAsync(Func<Space, bool> cond);

    Task<Result<Space>> GetSpaceByIdAsync(int id);

    Task<Result<IEnumerable<Space>>> GetAvailableSpacesAsync(DateTime startTime, DateTime endTime);

    Task<Result<Space>> CreateSpaceAsync(Space space);

    Task<Result<Space>> UpdateSpaceAsync(Space space);

    Task<Result> DeleteSpaceAsync(int id);

    Task<Result<bool>> IsSpaceAvailableAsync(int workspaceId, DateTime startTime, DateTime endTime);

    Task<Result<SpaceSchedule>> GetSpaceScheduleAsync(int spaceId, DateTime startDate, DateTime endDate);
}
