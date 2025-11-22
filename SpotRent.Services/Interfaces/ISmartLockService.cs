using SpotRent.Domain.Common;

namespace SpotRent.Services.Interfaces;

public interface ISmartLockService
{
    Task<Result> RegisterDeviceAsync(int deviceId);

    Task<Result> UpdateDeviceStatusAsync(int deviceId);

    Task<Result> UnlockAsync(int userId, string deviceId, string qrCode);

    Task<Result> ConfirmUnlockAsync(int deviceId);

    Task<Result> ReportFailedUnlockAttemptAsync(int deviceId, string reason);

    Task<Result<bool>> ValidateAccessAsync(int userId, string deviceId);
}
