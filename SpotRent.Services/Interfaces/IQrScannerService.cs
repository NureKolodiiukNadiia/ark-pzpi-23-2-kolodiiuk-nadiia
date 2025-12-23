using SpotRent.Domain.Common;

namespace SpotRent.Services.Interfaces;

public interface IQrScannerService
{
    Task<Result<string>> GenerateQrCode(int deviceId, int bookingId);

    Task<Result<bool>> ValidateQrCode(string qrCode, int deviceId, int bookingId);

    Task<Result<string>> GenerateQrCodeOwner(int deviceId, int userId);
}
