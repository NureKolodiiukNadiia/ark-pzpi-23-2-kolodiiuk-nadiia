namespace SpotRent.Domain.Entities;

public class AttributeValue
{
    public int Id { get; set; }

    public string Value { get; set; }

    public int? MinValue { get; set; }

    public int? MaxValue { get; set; }

    public int AttributeId { get; set; }

    public Attribute Attribute { get; set; }
}
