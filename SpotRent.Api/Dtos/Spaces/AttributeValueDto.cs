namespace SpotRent.Api.Dtos.Spaces;

public class AttributeValueDto
{
    public int Id { get; set; }

    public int AttributeId { get; set; }

    public string Value { get; set; }

    public int? MinValue { get; set; }

    public int? MaxValue { get; set; }
}
