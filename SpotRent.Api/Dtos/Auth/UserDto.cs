using SpotRent.Domain.Entities;

namespace SpotRent.Api.Dtos.Auth;

public class UserDto
{
    public int Id { get; set; }

    public string Email { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public static UserDto MapUser(User spaceOwner)
    {
        return new UserDto
        {
            Id = spaceOwner.Id,
            Email = spaceOwner.Email,
            FirstName = spaceOwner.FirstName,
            LastName = spaceOwner.LastName
        };
    }
}
