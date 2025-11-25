using SpotRent.Services.Payment;

namespace SpotRent.Services.Bookings;

public class BookingCreationResponse
{
    public int BookingId { get; set; }
        
    public LiqPayPaymentData LiqPayPaymentData { get; set; }
}
