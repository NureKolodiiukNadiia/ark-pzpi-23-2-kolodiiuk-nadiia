namespace SpotRent.Api.Dto.Space;

public record UpdateSpaceRequest : CreateSpaceRequest
{
    public int Id { get; set; }
}
