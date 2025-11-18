using Spotify.Model;

namespace Spotify.DTO;

public record RoleResponse(Guid ID, string Code, string Name) 
{
    public static RoleResponse FromRole(Role role)   
    {
        return new RoleResponse(role.Id, role.Code, role.Name);
    }
}