using Microsoft.Extensions.Logging;
using SpotRent.Domain.Common;
using SpotRent.Infrastructure;
using SpotRent.Services.Interfaces;

namespace SpotRent.Services.Devices;

public class QrScannerService : BaseService<QrScannerService>, IQrScannerService
{
    public QrScannerService(SpotRentDbContext context, ILogger<QrScannerService> logger) : base(context, logger)
    {
    }

    public async Task<Result<string>> GenerateQrCode(int deviceId, int bookingId)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<bool>> ValidateQrCode(string qrCode, int deviceId, int bookingId)
    {
        throw new NotImplementedException();
    }
}
