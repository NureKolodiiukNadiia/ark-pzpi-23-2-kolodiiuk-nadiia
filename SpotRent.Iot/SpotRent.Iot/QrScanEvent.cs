namespace SpotRent.Iot;

public class QrScanEvent
{
    public int UserId { get; set; }

    public int BookingId { get; set; }

    public string QrCode { get; set; } = string.Empty;

    public int AccessType { get; set; }

    public bool IsOwner { get; set; }

    public bool ShouldUnlock { get; set; } = true;
}
