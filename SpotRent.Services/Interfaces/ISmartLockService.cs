using SpotRent.Domain.Entities;
using SpotRent.Domain.Enums;

namespace SpotRent.Services.Interfaces;

public interface ISmartLockService
{
    Task<AccessLog> LogAccessAsync(int userId, string deviceId, AccessType accessType, int? bookingId = null, bool isSuccessful = true, string errorMessage = null);

    Task<bool> ValidateAccessAsync(int userId, string deviceId);
}
