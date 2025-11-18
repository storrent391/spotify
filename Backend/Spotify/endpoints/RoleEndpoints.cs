using Spotify.Repository;
using Spotify.Model;
using Spotify.Services;
using Spotify.DTO;

namespace Spotify.Endpoints;

public static class RoleEndpoints
{
        public static void MapRoleEndpoints(this WebApplication app, DatabaseConnection dbConn)
    {
        app.MapGet("/Roles", () =>
        {
            List<Role> roles = RoleADO.GetAll(dbConn);
            List<RoleResponse> roleResponse = new List<RoleResponse>();
            foreach(Role role in roles)
            {
                roleResponse.Add(RoleResponse.FromRole(role));
            }
            return Results.Ok(roleResponse);
        });
    }
}

