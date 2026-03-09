using Spotify.Common;
using Spotify.DTO;
using Spotify.Repository;
using Spotify.Services;

namespace Spotify.Repository.Validators;

   public static class UserRoleValidator
{
    public static Result Validate(RoleUserRequest roleUser)
    {

        if (roleUser.Role_ID == Guid.Empty)
        {
            return Result.Failure("El Role es Empty", "User");
        }

        if (roleUser.User_ID == Guid.Empty)
        {
            return Result.Failure("L'usuari es Empty", "User");
        }
        
        return Result.Ok();
        
    }
    
    

}



