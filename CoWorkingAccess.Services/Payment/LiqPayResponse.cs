namespace CoWorkingAccess.Services.Payment;

public class LiqPayResponse
{
    public string status { get; set; }

    public string order_id { get; set; }
    
    public decimal amount { get; set; }
    
    public string currency { get; set; }
    
    public string transaction_id { get; set; }
}
