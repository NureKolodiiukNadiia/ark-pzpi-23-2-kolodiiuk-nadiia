namespace SpotRent.Domain.Entities;

public class Attribute
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string DataType { get; set; }
    
    public string Unit { get; set; }

    public ICollection<AttributeValue> AttributeValues { get; set; } = new List<AttributeValue>();
}
