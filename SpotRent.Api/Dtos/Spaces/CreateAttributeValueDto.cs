namespace SpotRent.Api.Dtos.Spaces;

public class CreateAttributeValueDto
{
    public string Value { get; set; }

    public int? MinValue { get; set; }

    public int? MaxValue { get; set; }

    public int SpaceId { get; set; }

    public int AttributeId { get; set; }
}
