using Spotify.Model;

namespace Spotify.DTO;

public record RolePermissionRequest(Guid Permission_ID, Guid Role_ID) 
{
    public RolePermission ToRolePermission (Guid Id)   
    {
        return new RolePermission
        {
            Id = Id,
            Permission_ID = Permission_ID,
            Role_ID = Role_ID
        };
    }
}