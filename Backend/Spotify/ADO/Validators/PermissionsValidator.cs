using Spotify.Common;
using Spotify.DTO;
using Spotify.Repository;
using Spotify.Services;

namespace Spotify.Repository.Validators;

public static class PermissionsValidator
{
    public static Result Validate(PermissionRequest permission)
    {

        if (permission.Name == string.Empty)
        {
            return Result.Failure("El Permis es Empty", "Permis");
        }

        if (permission.Code == string.Empty)
        {
            return Result.Failure("L'Permis es Empty", "Permis");
        }

        return Result.Ok();

    }



}



