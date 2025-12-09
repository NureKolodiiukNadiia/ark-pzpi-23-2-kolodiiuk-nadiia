using SpotRent.Domain.Common;
using SpotRent.Domain.Entities;
using SpotRent.Domain.Enums;

namespace SpotRent.Services.Interfaces;

public interface IAccessLogService
{
    Task<Result<int>> LogAccessAsync(int userId, int deviceId, AccessType accessType, int bookingId,
        bool isSuccessful = true, string errorMessage = null);

    Task<Result<int>> LogOwnerAccessAsync(int userId, int deviceId, AccessType accessType,
        bool isSuccessful = true, string errorMessage = null);

    Task<Result<IEnumerable<AccessLog>>> GetSpaceAccessLogsAsync(int deviceId);

    Task<Result<IEnumerable<AccessLog>>> GetOwnerAccessLogsAsync(int ownerId);

    Task<Result<IEnumerable<AccessLog>>> GetUserAccessLogsAsync(int userId);

    Task<Result<AccessLog>> GetLogById(int id);
}
