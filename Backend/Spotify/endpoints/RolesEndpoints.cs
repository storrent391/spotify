using Spotify.Repository;
using Spotify.Model;
using Spotify.Services;

namespace Spotify.Endpoints;

public static class RolesEndpoints
{
        public static void MapRolesEndpoints(this WebApplication app, DatabaseConnection dbConn)
    {
        app.MapGet("/Roles", () =>
        {
            List<Role> roles = RoleADO.GetAll(dbConn);
            return Results.Ok(roles);
        });
    }
}

