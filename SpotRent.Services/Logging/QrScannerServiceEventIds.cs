using Microsoft.Extensions.Logging;

namespace SpotRent.Services.Logging;

internal static class QrScannerServiceEventIds
{
    internal static readonly EventId ErrorGeneratingQrCode = new(8000, nameof(ErrorGeneratingQrCode));

    internal static readonly EventId InvalidQrCodeDecryption = new(8001, nameof(InvalidQrCodeDecryption));

    internal static readonly EventId ErrorValidatingQrCode = new(8002, nameof(ErrorValidatingQrCode));
}
