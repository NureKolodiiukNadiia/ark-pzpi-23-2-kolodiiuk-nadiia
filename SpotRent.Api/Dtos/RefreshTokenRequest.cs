using System.ComponentModel.DataAnnotations;

namespace SpotRent.Api.Dtos;

public class RefreshTokenRequest
{
    [Required]
    public string RefreshToken { get; set; }
}
