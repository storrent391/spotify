using Spotify.Model;

namespace Spotify.DTO;

public record RoleRequest(string Code, string Name) 
{
    public Role ToRole(Guid Id)   
    {
        return new Role
        {
            Id = Id,
            Name = Name,
            Code = Code
        };
    }
}