using Microsoft.Extensions.Logging;
using SpotRent.Domain.Common;
using SpotRent.Infrastructure;
using SpotRent.Services.Interfaces;

namespace SpotRent.Services.Devices;

public class SmartLockService : BaseService<SmartLockService>, ISmartLockService
{
    public SmartLockService(SpotRentDbContext context, ILogger<SmartLockService> logger) : base(context, logger)
    {
    }

    public async Task<Result> RegisterDeviceAsync(int deviceId)
    {
        throw new NotImplementedException();
    }

    public async Task<Result> UpdateDeviceStatusAsync(int deviceId)
    {
        throw new NotImplementedException();
    }

    public async Task<Result> UnlockAsync(int userId, string deviceId, string qrCode)
    {
        throw new NotImplementedException();
    }

    public async Task<Result> ConfirmUnlockAsync(int deviceId)
    {
        throw new NotImplementedException();
    }

    public async Task<Result> ReportFailedUnlockAttemptAsync(int deviceId, string reason)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<bool>> ValidateAccessAsync(int userId, string deviceId)
    {
        throw new NotImplementedException();
    }
}
