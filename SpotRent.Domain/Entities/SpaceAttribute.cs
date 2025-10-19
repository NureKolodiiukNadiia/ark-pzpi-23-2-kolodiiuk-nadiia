namespace SpotRent.Domain.Entities;

public class SpaceAttribute
{
    public int AttributeId { get; set; }

    public int SpaceId { get; set; }

    public string Value { get; set; }

    public Space Space { get; set; }

    public Attribute Attribute { get; set; }
}