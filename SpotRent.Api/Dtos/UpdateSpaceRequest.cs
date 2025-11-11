namespace SpotRent.Api.Dtos;

public record UpdateSpaceRequest : CreateSpaceRequest
{
    public int Id { get; set; }
}
