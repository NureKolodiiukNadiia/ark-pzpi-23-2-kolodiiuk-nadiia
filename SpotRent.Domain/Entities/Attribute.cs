namespace SpotRent.Domain.Entities;

public class Attribute
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Type { get; set; }

    public ICollection<SpaceAttribute> SpaceAttributes { get; set; } = new List<SpaceAttribute>();

    public ICollection<AttributeValue> AttributeValues { get; set; } = new List<AttributeValue>();
}
