using Spotify.Common;
using Spotify.DTO;
using Spotify.Model;
using Spotify.Repository;
using Spotify.Services;

namespace Spotify.Repository.Validators;

public static class RolePermissionValidator
{
    public static Result Validate(RolePermissionRequest rolePermission)
    {

        if (rolePermission.Permission_ID == Guid.Empty)
        {
            return Result.Failure("El Permission_ID es Empty", "Permission_ID");
        }

        if (rolePermission.Role_ID == Guid.Empty)
        {
            return Result.Failure("L'Role es Empty", "Role");
        }

        return Result.Ok();

    }



}



