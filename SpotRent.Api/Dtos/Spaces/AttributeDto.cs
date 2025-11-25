using Attribute = SpotRent.Domain.Entities.Attribute;

namespace SpotRent.Api.Dtos.Spaces;

public class AttributeDto
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string DataType { get; set; }

    public string Unit { get; set; }

    public static AttributeDto Map(Attribute attribute)
    {
        if (attribute is null)
        {
            return null;
        }

        return new AttributeDto
        {
            Id = attribute.Id,
            Name = attribute.Name,
            DataType = attribute.DataType,
            Unit = attribute.Unit
        };
    }
}
// psql -d atark -c "SELECT setval(pg_get_serial_sequence('attribute_value','id'), (SELECT COALESCE(MAX(id),0) FROM attribute_value));"
//