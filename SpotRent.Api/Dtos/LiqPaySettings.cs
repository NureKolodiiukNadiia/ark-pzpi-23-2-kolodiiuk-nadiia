namespace SpotRent.Api.Dtos;

public class LiqPaySettings
{
    public string PublicKey { get; set; } = string.Empty;
    
    public string PrivateKey { get; set; } = string.Empty;
    
    public bool Sandbox { get; set; }
}
