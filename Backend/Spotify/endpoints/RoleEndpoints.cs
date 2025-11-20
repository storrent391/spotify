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
            foreach (Role role in roles)
            {
                roleResponse.Add(RoleResponse.FromRole(role));
            }
            return Results.Ok(roleResponse);
        });

        app.MapGet("/Roles/{Role_Code}/Permission", (string Role_Code) =>
{
            List<RolePermission> rolePermissions = RolePermissionADO.GetByCode(dbConn, Role_Code);

            return rolePermissions.Count > 0
                ? Results.Ok(rolePermissions)
                : Results.NotFound(new { message = $"No permissions found for Role Code {Role_Code}" });
        });
    }


}

