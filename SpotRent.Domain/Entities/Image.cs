namespace SpotRent.Domain.Entities;

public class Image
{
    public int Id { get; set; }

    public string ImageUrl { get; set; }

    public int SpaceId { get; set; }

    public Space Space { get; set; }
}
