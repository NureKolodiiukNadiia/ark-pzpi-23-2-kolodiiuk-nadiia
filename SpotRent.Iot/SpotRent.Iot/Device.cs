using System.Text;
using System.Text.Json;

namespace SpotRent.Iot;

public class Device
{
    private const int DefaultAccessType = 1;

    private readonly int _deviceId;

    private readonly int _spaceId;

    private readonly string _apiHost;

    private readonly int _defaultUserId;

    private readonly int _defaultBookingId;

    private readonly bool _registerOnStart;

    private readonly HttpClient _httpClient;

    private SmartLock _smartLock;

    public Device(DeviceOptions options = null)
    {
        var config = options ?? new DeviceOptions();
        _deviceId = config.DeviceId;
        _spaceId = config.SpaceId;
        _apiHost = config.ApiHost;
        _defaultUserId = config.DefaultUserId;
        _defaultBookingId = config.DefaultBookingId;
        _registerOnStart = config.RegisterOnStart;
        _httpClient = new HttpClient();

        Console.WriteLine($"[INFO] Device initialized with ID: {_deviceId}");
    }

    public void AddSmartLock(string initialState = "locked")
    {
        _smartLock = new SmartLock(initialState, _apiHost, _httpClient);
    }

    public async Task<bool> LockAsync(int userId, string qrCode, bool isOwnerOverride)
    {
        if (_smartLock == null)
        {
            Console.WriteLine("[ERROR] Smart lock not initialized.");
            return false;
        }

        var result = await _smartLock.LockAsync(_deviceId, userId, qrCode, isOwnerOverride);
        if (result)
        {
            await UpdateDeviceStatusAsync(_smartLock.Status, true);
        }

        return result;
    }

    public async Task<bool> UnlockAsync(int userId, string qrCode, bool isOwnerOverride)
    {
        if (_smartLock == null)
        {
            Console.WriteLine("[ERROR] Smart lock not initialized.");

            return false;
        }

        var result = await _smartLock.UnlockAsync(_deviceId, userId, qrCode, isOwnerOverride);
        if (result)
        {
            await UpdateDeviceStatusAsync(_smartLock.Status, true);
        }

        return result;
    }

    public async Task<bool> RegisterDeviceAsync()
    {
        if (_spaceId <= 0)
        {
            Console.WriteLine("[WARN] Space id not configured. Skipping registration.");

            return false;
        }

        var jsonPayload = SerializePayload(writer =>
        {
            writer.WriteStartObject();
            writer.WriteNumber("spaceId", _spaceId);
            writer.WriteEndObject();
        });

        Console.WriteLine($"[INFO] Registering device...");

        return await PostJsonAsync("/IoT/register", jsonPayload);
    }

    public async Task LogEventAsync(int userId, int bookingId, int accessType,
        bool isSuccessful, string errorMessage)
    {
        if (bookingId <= 0)
        {
            Console.WriteLine("[WARN] Unable to log access event without valid booking id.");
            return;
        }

        var jsonPayload = SerializePayload(writer =>
        {
            writer.WriteStartObject();
            writer.WriteNumber("userId", userId);
            writer.WriteNumber("deviceId", _deviceId);
            writer.WriteNumber("accessType", accessType);
            writer.WriteNumber("bookingId", bookingId);
            writer.WriteBoolean("isSuccessful", isSuccessful);
            writer.WriteString("errorMessage", errorMessage ?? string.Empty);
            writer.WriteEndObject();
        });

        Console.WriteLine($"[LOG] Access event: UserId={userId}, BookingId={bookingId}, Success={isSuccessful}");

        await PostJsonAsync("/AccessLog", jsonPayload);
    }

    public async Task LogEventOwnerAsync(int userId, int accessType,
        bool isSuccessful, string errorMessage)
    {
        var jsonPayload = SerializePayload(writer =>
        {
            writer.WriteStartObject();
            writer.WriteNumber("userId", userId);
            writer.WriteNumber("deviceId", _deviceId);
            writer.WriteNumber("accessType", accessType);
            writer.WriteBoolean("isSuccessful", isSuccessful);
            writer.WriteString("errorMessage", errorMessage ?? string.Empty);
            writer.WriteEndObject();
        });

        Console.WriteLine($"[LOG] Owner access event: UserId={userId}, Success={isSuccessful}");

        await PostJsonAsync("/AccessLog/owner", jsonPayload);
    }

    public async Task UpdateDeviceStatusAsync(string statusMessage, bool isOnline)
    {
        var jsonPayload = SerializePayload(writer =>
        {
            writer.WriteStartObject();
            writer.WriteNumber("deviceId", _deviceId);
            writer.WriteBoolean("isOnline", isOnline);
            writer.WriteString("statusMessage", statusMessage ?? string.Empty);
            writer.WriteEndObject();
        });

        await PostJsonAsync("/IoT/device-status", jsonPayload);
    }

    private static string SerializePayload(Action<Utf8JsonWriter> writeAction)
    {
        using var ms = new MemoryStream();
        var options = new JsonWriterOptions { Indented = false };
        using (var writer = new Utf8JsonWriter(ms, options))
        {
            writeAction(writer);
        }

        return Encoding.UTF8.GetString(ms.ToArray());
    }

    private async Task<bool> PostJsonAsync(string path, string jsonBody)
    {
        try
        {
            var url = $"{_apiHost.TrimEnd('/')}/{path.TrimStart('/')}";
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);
            var responseBody = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"[HTTP] POST {url} status {(int)response.StatusCode}");

            return response.IsSuccessStatusCode;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"[ERROR] HTTP request failed: {ex.Message}");
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] POST failed: {ex.Message}");
            return false;
        }
    }

    public async Task RunAsync()
    {
        if (_smartLock == null)
        {
            Console.WriteLine("[ERROR] Smart lock not initialized.");

            return;
        }

        if (_registerOnStart)
        {
            await RegisterDeviceAsync();
        }

        await UpdateDeviceStatusAsync(_smartLock.Status, true);

        Console.WriteLine();
        Console.WriteLine("Commands:");
        Console.WriteLine("  <QR_VALUE>      - attempts unlock with a QR token");
        Console.WriteLine("  owner:<QR>      - owner override unlock");
        Console.WriteLine("  LOCK            - lock the device");
        Console.WriteLine("  STATUS          - prints current lock status");
        Console.WriteLine("  EXIT            - exit application");
        Console.WriteLine();

        while (true)
        {
            Console.Write("QR> ");
            var input = Console.ReadLine();
            if (input == null)
            {
                break;
            }

            var trimmed = input.Trim();
            if (string.IsNullOrEmpty(trimmed))
            {
                continue;
            }

            if (trimmed.Equals("EXIT", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("[INFO] Shutting down...");
                break;
            }

            if (trimmed.Equals("STATUS", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine($"[STATE] Current lock status: {_smartLock.Status}");
                continue;
            }

            var scan = BuildScanEvent(trimmed);
            if (scan == null)
            {
                continue;
            }

            await HandleScanAsync(scan);
        }
    }

    private QrScanEvent BuildScanEvent(string input)
    {
        if (input.Equals("LOCK", StringComparison.OrdinalIgnoreCase))
        {
            return new QrScanEvent
            {
                QrCode = "LOCK",
                ShouldUnlock = false,
                AccessType = DefaultAccessType
            };
        }

        if (input.StartsWith("owner:", StringComparison.OrdinalIgnoreCase))
        {
            var qr = input.Substring("owner:".Length).Trim();
            if (string.IsNullOrEmpty(qr))
            {
                Console.WriteLine("[WARN] Missing QR token for owner override.");
                return null;
            }

            return new QrScanEvent
            {
                QrCode = qr,
                IsOwner = true,
                ShouldUnlock = true,
                AccessType = DefaultAccessType
            };
        }

        return new QrScanEvent
        {
            QrCode = input,
            ShouldUnlock = true,
            AccessType = DefaultAccessType
        };
    }

    private async Task HandleScanAsync(QrScanEvent scan)
    {
        var userId = scan.UserId > 0 ? scan.UserId : _defaultUserId;
        var bookingId = scan.BookingId > 0 ? scan.BookingId : _defaultBookingId;
        var accessType = scan.AccessType != 0 ? scan.AccessType : DefaultAccessType;

        bool success;
        if (scan.ShouldUnlock)
        {
            success = await UnlockAsync(userId, scan.QrCode, scan.IsOwner);
        }
        else
        {
            success = await LockAsync(userId, scan.QrCode, scan.IsOwner);
        }

        var message = success ? string.Empty : (scan.ShouldUnlock ? "Unlock failed" : "Lock failed");

        if (scan.IsOwner)
        {
            await LogEventOwnerAsync(userId, accessType, success, message);
        }
        else
        {
            await LogEventAsync(userId, bookingId, accessType, success, message);
        }
    }
}
