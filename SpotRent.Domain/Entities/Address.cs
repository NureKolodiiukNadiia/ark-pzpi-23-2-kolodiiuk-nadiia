using System.Text.Json.Serialization;

namespace SpotRent.Domain.Entities;

public class Address
{
    public int Id { get; set; }

    public string Building { get; set; }

    public string Street { get; set; }

    public string City { get; set; }

    public string Region { get; set; }

    [JsonIgnore]
    public ICollection<Space> Spaces { get; set; } = new List<Space>();
}
