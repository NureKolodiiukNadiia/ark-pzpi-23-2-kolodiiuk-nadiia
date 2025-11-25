using System.ComponentModel.DataAnnotations;

namespace SpotRent.Api.Dtos.Auth;

public class RefreshTokenRequest
{
    [Required]
    public string RefreshToken { get; set; }
}
