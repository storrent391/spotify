using Spotify.Model;

namespace Spotify.DTO;

public record PermissionResponse(Guid ID, string Code, string Name) 
{
    public static PermissionResponse FromPermission(Permission permission)   
    {
        return new PermissionResponse(permission.Id, permission.Code, permission.Name);
    }
}