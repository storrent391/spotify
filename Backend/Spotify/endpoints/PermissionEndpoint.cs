using Spotify.Repository;
using Spotify.Model;
using Spotify.Services;

namespace Spotify.Endpoints;

public static class PermissionEndpoints
{
        public static void MapPermissionEndpoints(this WebApplication app, DatabaseConnection dbConn)
    {
        app.MapGet("/Permissions", () =>
        {
            List<Permission> permissions = PermissionADO.GetAll(dbConn);
            return Results.Ok(permissions);
        });
    }
}

