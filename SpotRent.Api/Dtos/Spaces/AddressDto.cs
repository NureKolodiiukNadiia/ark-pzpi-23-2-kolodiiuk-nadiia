using SpotRent.Domain.Entities;

namespace SpotRent.Api.Dtos.Spaces;

public class AddressDto
{
    public int Id { get; set; }

    public string Building { get; set; }

    public string Street { get; set; }

    public string City { get; set; }

    public string Region { get; set; }

    public static AddressDto Map(Address address)
    {
        if (address is null) return null;
        return new AddressDto
        {
            Id = address.Id,
            Building = address.Building,
            Street = address.Street,
            City = address.City,
            Region = address.Region
        };
    }
}
