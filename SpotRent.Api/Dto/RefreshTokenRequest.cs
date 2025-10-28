using System.ComponentModel.DataAnnotations;

namespace SpotRent.Api.Dto;

public class RefreshTokenRequest
{
    [Required]
    public string RefreshToken { get; set; }
}
