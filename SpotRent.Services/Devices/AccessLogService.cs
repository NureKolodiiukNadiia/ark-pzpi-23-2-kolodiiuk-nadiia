using Microsoft.Extensions.Logging;
using SpotRent.Domain.Common;
using SpotRent.Domain.Entities;
using SpotRent.Domain.Enums;
using SpotRent.Infrastructure;
using SpotRent.Services.Interfaces;

namespace SpotRent.Services.Devices;

public class AccessLogService : BaseService<AccessLogService>, IAccessLogService
{
    public AccessLogService(SpotRentDbContext context, ILogger<AccessLogService> logger) : base(context, logger)
    {
    }

    public async Task<Result<AccessLog>> LogAccessAsync(int userId, string deviceId, AccessType accessType,
        int? bookingId = null, bool isSuccessful = true,
        string errorMessage = null)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<IEnumerable<AccessLog>>> GetSpaceAccessLogsAsync(string deviceId)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<IEnumerable<AccessLog>>> GetOwnerAccessLogsAsync(int ownerId)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<IEnumerable<AccessLog>>> GetUserAccessLogsAsync(int userId)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<AccessLog>> GetLogById(int id)
    {
        throw new NotImplementedException();
    }
}
