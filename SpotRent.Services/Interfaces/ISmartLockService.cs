using SpotRent.Domain.Common;

namespace SpotRent.Services.Interfaces;

public interface ISmartLockService
{
    Task<Result> RegisterDeviceAsync(int spaceId);

    Task<Result> UpdateDeviceStatusAsync(int deviceId, bool isOnline, string statusMessage);

    Task<Result<bool>> UnlockAsync(int userId, int deviceId, string qrCode);

    Task<Result<bool>> UnlockOwnerAsync(int userId, int deviceId, string qrCode);

    Task<Result> LockAsync(int deviceId);
}
