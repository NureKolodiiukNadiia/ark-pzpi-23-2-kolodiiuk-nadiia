using SpotRent.Domain.Common;
using SpotRent.Domain.Entities;
using SpotRent.Domain.Enums;

namespace SpotRent.Services.Interfaces;

public interface IAccessLogService
{
    Task<Result<AccessLog>> LogAccessAsync(int userId, string deviceId, AccessType accessType, int? bookingId = null,
        bool isSuccessful = true, string errorMessage = null);

    Task<Result<IEnumerable<AccessLog>>> GetSpaceAccessLogsAsync(string deviceId);

    Task<Result<IEnumerable<AccessLog>>> GetOwnerAccessLogsAsync(int ownerId);

    Task<Result<IEnumerable<AccessLog>>> GetUserAccessLogsAsync(int userId);

    Task<Result<AccessLog>> GetLogById(int id);
}
