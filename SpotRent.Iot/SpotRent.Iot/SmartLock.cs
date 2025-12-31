using System.Text;
using System.Text.Json;

namespace SpotRent.Iot;

public class SmartLock
{
    private bool _locked;

    private readonly string _apiHost;

    private readonly HttpClient _httpClient;

    public SmartLock(string initialState, string apiHost, HttpClient httpClient)
    {
        _locked = initialState != "unlocked";
        _apiHost = apiHost;
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

        Console.WriteLine($"[INFO] Smart lock initialized in state: {Status}");
    }

    public string Status => _locked ? "Locked" : "Unlocked";

    public async Task<bool> LockAsync(int deviceId, int userId, string qrCode, bool isOwnerOverride)
    {
        if (_locked)
        {
            Console.WriteLine("[WARN] Lock command ignored. Already locked.");

            return false;
        }

        var success = await PostJsonAsync($"IoT/lock/{deviceId}", "{}");
        if (success)
        {
            _locked = true;
            Console.WriteLine("[INFO] Lock engaged.");
            Console.WriteLine($"[STATE] Lock state changed to: {Status}");
        }

        return success;
    }

    public async Task<bool> UnlockAsync(int deviceId, int userId, string qrCode, bool isOwnerOverride)
    {
        if (!_locked)
        {
            Console.WriteLine("[WARN] Unlock command ignored. Already unlocked.");

            return false;
        }

        var jsonPayload = SerializePayload(writer =>
        {
            writer.WriteStartObject();
            writer.WriteNumber("deviceId", deviceId);
            writer.WriteNumber("userId", userId);
            writer.WriteString("qrCode", qrCode ?? string.Empty);
            writer.WriteBoolean("isOwnerOverride", isOwnerOverride);
            writer.WriteEndObject();
        });

        var success = await PostJsonAsync("IoT/unlock", jsonPayload);
        if (success)
        {
            _locked = false;
            Console.WriteLine("[INFO] Lock opened.");
            Console.WriteLine($"[STATE] Lock state changed to: {Status}");
        }

        return success;
    }

    private async Task<bool> PostJsonAsync(string path, string jsonBody)
    {
        if (string.IsNullOrEmpty(_apiHost))
        {
            Console.WriteLine("[WARN] API host not configured, skipping HTTP request.");

            return true;
        }

        try
        {
            var url = $"{_apiHost.TrimEnd('/')}/{path.TrimStart('/')}";
            using var content = new StringContent(jsonBody ?? string.Empty, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);
            var responseBody = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"[HTTP] POST {url} status {(int)response.StatusCode} body: {responseBody}");

            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] POST failed: {ex.Message}");

            return false;
        }
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
}
