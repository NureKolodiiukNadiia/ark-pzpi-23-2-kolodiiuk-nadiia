using SpotRent.Domain.Entities;
using SpotRent.Domain.Enums;
using SpotRent.Services.Interfaces;

namespace SpotRent.Services;

public class SmartLockService : ISmartLockService
{
    public Task<AccessLog> LogAccessAsync(int userId, string deviceId, AccessType accessType, 
        int? bookingId = null, bool isSuccessful = true, string errorMessage = null)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ValidateAccessAsync(int userId, string deviceId)
    {
        throw new NotImplementedException();
    }
}
