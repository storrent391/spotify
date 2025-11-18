using Spotify.Model;

namespace Spotify.DTO;

public record PermissionRequest(string Code, string Name) 
{
    public Permission ToPermission(Guid Id)   
    {
        return new Permission
        {
            Id = Id,
            Name = Name,
            Code = Code
        };
    }
}