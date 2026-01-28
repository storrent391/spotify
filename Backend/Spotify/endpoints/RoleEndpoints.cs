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
            List<RoleResponse> roleResponses = new List<RoleResponse>();
            foreach (Role role in roles)
            {
                roleResponses.Add(RoleResponse.FromRole(role));
            }
            return Results.Ok(roleResponses);
        });

         app.MapGet("/Permission", () =>
        { 
            List<RolePermission> rolePermissions = RolePermissionADO.GetAll(dbConn);
            List<RolePermissionResponse> rolePermissionResponses = new List<RolePermissionResponse>();
            foreach (RolePermission rolePermission in rolePermissions)
            {
                rolePermissionResponses.Add(RolePermissionResponse.FromRolePermission(rolePermission));
            }
            return Results.Ok(rolePermissionResponses);
        });

        // GET carritoProducte by id
        app.MapGet("/Roles/{id}/Permission", (Guid id) =>
        {
            List<RolePermission> rolePermissions = RolePermissionADO.GetById(dbConn, id)!;
            List<RolePermissionResponse> rolePermissionResponses = new List<RolePermissionResponse>();
            foreach (RolePermission rolePermission in rolePermissions)
            {
                rolePermissionResponses.Add(RolePermissionResponse.FromRolePermission(rolePermission));
            }
            return Results.Ok(rolePermissionResponses);

        });


    }

}

