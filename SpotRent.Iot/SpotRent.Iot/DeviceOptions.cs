using System.Text.Json;

namespace SpotRent.Iot;

public class DeviceOptions
{
    public string ApiHost { get; set; } = "https://irrigative-bessie-evidentially.ngrok-free.dev/api";

    public int DeviceId { get; set; } = 1;

    public int SpaceId { get; set; } = 1;

    public int DefaultUserId { get; set; } = 101;

    public int DefaultBookingId { get; set; } = 2001;

    public string InitialLockState { get; set; } = "locked";

    public bool RegisterOnStart { get; set; } = true;

    public static DeviceOptions Load(string path)
    {
        try
        {
            if (!File.Exists(path))
            {
                Console.WriteLine($"[WARN] Configuration file '{path}' not found. Using defaults.");
                return new DeviceOptions();
            }

            var json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<DeviceOptions>(json,
                       new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                   ?? new DeviceOptions();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[WARN] Failed to load configuration. {ex.Message}. Using defaults.");
            return new DeviceOptions();
        }
    }
}
