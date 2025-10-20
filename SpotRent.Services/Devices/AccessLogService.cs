using SpotRent.Domain.Entities;
using SpotRent.Domain.Enums;
using SpotRent.Services.Interfaces;

namespace SpotRent.Services.Devices;

public class AccessLogService : IAccessLogService
{
    public Task<IEnumerable<AccessLog>> GetUserAccessLogsAsync(int userId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<AccessLog>> GetDeviceAccessLogsAsync(string deviceId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<AccessLog>> GetAccessLogsByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        throw new NotImplementedException();
    }

    public Task<AccessLog> LogAccessAsync(int userId, string deviceId, AccessType accessType, int? bookingId = null, bool isSuccessful = true,
        string errorMessage = null)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ValidateAccessAsync(int userId, string deviceId)
    {
        throw new NotImplementedException();
    }
}