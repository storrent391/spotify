using Spotify.Model;

namespace Spotify.DTO;

public record RoleUserRequest( Guid Role_ID,Guid User_ID) 
{
    public UserRole ToRolePermission (Guid Id)   
    {
        return new UserRole
        {
            Id = Id,
            Role_ID = Role_ID,
            User_Id = User_ID

        };
    }
}