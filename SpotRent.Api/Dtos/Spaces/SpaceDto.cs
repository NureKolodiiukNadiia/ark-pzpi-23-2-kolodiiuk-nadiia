using SpotRent.Api.Dtos.Auth;
using SpotRent.Domain.Entities;
using SpotRent.Domain.Enums;
using Attribute = SpotRent.Domain.Entities.Attribute;

namespace SpotRent.Api.Dtos.Spaces;

public record SpaceDto
{
    public int Id { get; init; }

    public string Name { get; init; }

    public string Description { get; init; }

    public SpaceType SpaceType { get; init; }

    public string Room { get; set; }

    public int Capacity { get; init; }

    public double AreaSqm { get; init; }

    public decimal HourlyRate { get; init; }

    public int AddressId { get; init; }

    public AddressDto Address { get; init; }

    public string ImageUrl { get; set; }

    public bool IsAvailable { get; set; } = true;

    public DateTime CreatedAt { get; init; }

    public int OwnerId { get; set; }

    public UserDto OwnerDto { get; set; }

    public IEnumerable<WorkingHoursDto> WorkingHours { get; set; }

    public IEnumerable<AttributeDto> Attributes { get; set; }

    public IEnumerable<AttributeValueDto> AttributeValues { get; set; }

    public static SpaceDto MapSpace(Space space)
    {
        if (space is null)
        {
            throw new ArgumentNullException(nameof(space));
        }

        var attributeEntities = space.AttributeValues?
            .Where(av => av.Attribute != null)
            .Select(av => av.Attribute)
            .GroupBy(a => a.Id)
            .Select(g => g.First())
            .ToList() ?? new List<Attribute>();

        return new SpaceDto
        {
            Id = space.Id,
            Name = space.Name,
            Description = space.Description,
            SpaceType = space.SpaceType,
            Room = space.Room,
            Capacity = space.Capacity,
            AreaSqm = space.AreaSqm,
            HourlyRate = space.HourlyRate,
            AddressId = space.AddressId,
            Address = AddressDto.Map(space.Address),
            ImageUrl = space.ImageUrl,
            IsAvailable = space.IsAvailable,
            CreatedAt = space.CreatedAt,
            OwnerId = space.OwnerId,
            OwnerDto = space.Owner != null ? UserDto.MapUser(space.Owner) : null,
            WorkingHours = space.WorkingHours?.Select(WorkingHoursDto.Map).ToList() ?? new List<WorkingHoursDto>(),
            AttributeValues = space.AttributeValues?.Select(av => new AttributeValueDto
            {
                Id = av.Id,
                AttributeId = av.AttributeId,
                Value = av.Value,
                MinValue = av.MinValue,
                MaxValue = av.MaxValue,
            }).ToList() ?? new List<AttributeValueDto>(),
            Attributes = attributeEntities.Select(AttributeDto.Map).Where(a => a != null).ToList()
        };
    }
}