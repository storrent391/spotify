using Spotify.Model;

namespace Spotify.DTO;

public record UserRoleResponse(Guid ID, Guid Role_ID,Guid User_Id) 
{
    public static UserRoleResponse FromRoleUser(UserRole userRole)   
    {
        return new UserRoleResponse(userRole.Id, userRole.Role_ID,userRole.User_Id);
    }
}