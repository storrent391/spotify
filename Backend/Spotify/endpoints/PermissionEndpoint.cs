using Spotify.Repository;
using Spotify.Model;
using Spotify.Services;
using Spotify.DTO;
using Spotify.Repository.Validators;
using Spotify.Common;

namespace Spotify.Endpoints;

public static class PermissionEndpoints
{
    public static void MapPermissionEndpoints(this WebApplication app, DatabaseConnection dbConn)
    {
        app.MapGet("/Permissions", () =>
        {
            List<Permission> permissions = PermissionADO.GetAll(dbConn);
            List<PermissionResponse> permissionResponse = new List<PermissionResponse>();
            foreach (Permission permission in permissions)
            {
                permissionResponse.Add(PermissionResponse.FromPermission(permission));
            }
            return Results.Ok(permissions);
        });

        //POST Permissions
        app.MapPost("/Permission", (PermissionRequest req) =>
       {
           Guid id;
           Result result = PermissionsValidator.Validate(req);

           if (!result.IsOk)
           {
               return Results.BadRequest(new
               {
                   error = result.ErrorCode,
                   message = result.ErrorMessage
               });
           }

           id = Guid.NewGuid();
           Permission permission = req.ToPermission(id);
           PermissionADO.Insert(dbConn, permission);

           return Results.Ok(PermissionResponse.FromPermission(permission));
       });


        app.MapDelete("/Permission/{id}", (Guid id) => PermissionADO.Delete(dbConn, id) ? Results.NoContent() : Results.NotFound());






        //PERMISSIONROLE

        app.MapPost("/RolePermission", (RolePermissionRequest req) =>
        {
            Guid id;
            Result result = RolePermissionValidator.Validate(req);

            if (!result.IsOk)
            {
                return Results.BadRequest(new
                {
                    error = result.ErrorCode,
                    message = result.ErrorMessage
                });
            }

            id = Guid.NewGuid();
            RolePermission rolePermission = req.ToRolePermission(id);
            RolePermissionADO.Insert(dbConn, rolePermission);

            return Results.Ok(RolePermissionResponse.FromRolePermission(rolePermission));
        });

        //DELETE un rol de un usuari
        app.MapDelete("/Role/{id}/Permission", (Guid id) => RolePermissionADO.Delete(dbConn, id) ? Results.NoContent() : Results.NotFound());


        // GET Tots els permisos de un rol
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

