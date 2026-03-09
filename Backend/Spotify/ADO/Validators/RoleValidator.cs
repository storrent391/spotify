using Spotify.Common;
using Spotify.DTO;
using Spotify.Repository;
using Spotify.Services;

namespace Spotify.Repository.Validators;

public static class RoleValidator
{
    public static Result Validate(RoleRequest role)
    {

        if (role.Name == string.Empty)
        {
            return Result.Failure("El Role es Empty", "Role");
        }

        if (role.Code == string.Empty)
        {
            return Result.Failure("L'Role es Empty", "Role");
        }

        return Result.Ok();

    }



}



