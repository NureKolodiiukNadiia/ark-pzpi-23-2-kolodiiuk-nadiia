using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SpotRent.Domain.Common;
using SpotRent.Infrastructure;
using SpotRent.Services.Interfaces;
using SpotRent.Services.Logging;

namespace SpotRent.Services.Devices;

public class QrScannerService : BaseService<QrScannerService>, IQrScannerService
{
    private readonly byte[] _key = "A1B2C3D4E5F6G7H8"u8.ToArray();

    private readonly byte[] _iv = "1A2B3C4D5E6F7G8H"u8.ToArray();

    public QrScannerService(SpotRentDbContext context, ILogger<QrScannerService> logger)
        : base(context, logger)
    {
    }

    public async Task<Result<string>> GenerateQrCode(int deviceId, int bookingId)
    {
        try
        {
            var deviceExists = await Context.Devices.AnyAsync(d => d.Id == deviceId);
            if (!deviceExists)
            {
                return Result.Fail<string>($"Device with id {deviceId} does not exist");
            }

            var booking = await Context.Bookings.FindAsync(bookingId);
            if (booking is null)
            {
                return Result.Fail<string>($"Booking with id {bookingId} does not exist");
            }

            var payload = new
            {
                DeviceId = deviceId,
                BookingId = bookingId,
                ExpirationUtc = booking.EndTime
            };

            var json = System.Text.Json.JsonSerializer.Serialize(payload);

            var encrypted = Encrypt(json);

            return Result.Success(encrypted);
        }
        catch (Exception e)
        {
            Log(LogLevel.Error, QrScannerServiceEventIds.ErrorGeneratingQrCode,
                "Error generating QR code for device {deviceId} and booking {bookingId}. Error: {e.Message}",
                deviceId, bookingId, e.Message);

            return Result.Fail<string>(
                $"Error generating QR code for device {deviceId} and booking {bookingId}. Error: {e.Message}");
        }
    }

    public async Task<Result<bool>> ValidateQrCode(string qrCode, int deviceId, int bookingId)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(qrCode))
            {
                return Result.Fail<bool>("QR code is empty");
            }

            var decrypted = Decrypt(qrCode);

            var payload = System.Text.Json.JsonSerializer.Deserialize<QrPayload>(decrypted);
            if (payload is null)
            {
                return Result.Fail<bool>("Invalid QR payload");
            }

            if (payload.DeviceId != deviceId || payload.BookingId != bookingId)
            {
                return Result.Fail<bool>("QR data does not match expected device or booking");
            }

            if (DateTime.UtcNow > payload.ExpirationUtc)
            {
                return Result.Fail<bool>("QR code is expired");
            }

            var deviceExists = await Context.Devices.AnyAsync(d => d.Id == deviceId);
            if (!deviceExists)
            {
                return Result.Fail<bool>($"Device with id {deviceId} does not exist");
            }

            var bookingExists = await Context.Bookings.AnyAsync(b => b.Id == bookingId);
            if (!bookingExists)
            {
                return Result.Fail<bool>($"Booking with id {bookingId} does not exist");
            }

            return Result.Success(true);
        }
        catch (CryptographicException e)
        {
            Log(LogLevel.Warning, QrScannerServiceEventIds.InvalidQrCodeDecryption,
                "Decryption failed for QR code. Error: {e.Message}", e.Message);

            return Result.Fail<bool>("Invalid or tampered QR code");
        }
        catch (Exception e)
        {
            Log(LogLevel.Error, QrScannerServiceEventIds.ErrorValidatingQrCode,
                "Error validating QR code for device {deviceId} and booking {bookingId}. Error: {e.Message}",
                deviceId, bookingId, e.Message);

            return Result.Fail<bool>(
                $"Error validating QR code. Error: {e.Message}");
        }
    }

    private string Encrypt(string plainText)
    {
        using var aes = Aes.Create();
        aes.Key = _key;
        aes.IV = _iv;

        using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        using var ms = new MemoryStream();
        using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
        using (var sw = new StreamWriter(cs))
        {
            sw.Write(plainText);
        }

        return Convert.ToBase64String(ms.ToArray());
    }

    private string Decrypt(string cipher)
    {
        var buffer = Convert.FromBase64String(cipher);

        using var aes = Aes.Create();
        aes.Key = _key;
        aes.IV = _iv;

        using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        using var ms = new MemoryStream(buffer);
        using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        using var sr = new StreamReader(cs);

        return sr.ReadToEnd();
    }

    private class QrPayload
    {
        public int DeviceId { get; set; }

        public int BookingId { get; set; }

        public DateTime ExpirationUtc { get; set; }
    }
}
