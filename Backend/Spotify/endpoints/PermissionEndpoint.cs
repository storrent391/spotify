using Spotify.Repository;
using Spotify.Model;
using Spotify.Services;
using Spotify.DTO;
namespace Spotify.Endpoints;

public static class PermissionEndpoints
{
        public static void MapPermissionEndpoints(this WebApplication app, DatabaseConnection dbConn)
    {
        app.MapGet("/Permissions", () =>
        {
            List<Permission> permissions = PermissionADO.GetAll(dbConn);
            List<PermissionResponse> permissionResponse = new List<PermissionResponse>();
            foreach(Permission permission in permissions)
            {
                permissionResponse.Add(PermissionResponse.FromPermission(permission));
            }
            return Results.Ok(permissions);
        });
    }
}

