using Spotify.Model;

namespace Spotify.DTO;

public record RolePermissionResponse(Guid ID, Guid Permission_ID, Guid Role_ID) 
{
    public static RolePermissionResponse FromRolePermission(RolePermission rolePermission)   
    {
        return new RolePermissionResponse(rolePermission.Id, rolePermission.Permission_ID, rolePermission.Role_ID);
    }
}