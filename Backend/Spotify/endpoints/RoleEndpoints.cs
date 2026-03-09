using Spotify.Repository;
using Spotify.Model;
using Spotify.Services;
using Spotify.DTO;
using Spotify.Common;
using Spotify.Repository.Validators;

namespace Spotify.Endpoints;

public static class RoleEndpoints
{
    public static void MapRoleEndpoints(this WebApplication app, DatabaseConnection dbConn)
    {

        //Get tots els roles
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

        //Get taula RolePermisos
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


        //POST role
         app.MapPost("/Role", (RoleRequest req) =>
        {
            Guid id;
            Result result = RoleValidator.Validate(req);

            if (!result.IsOk)
            {
                return Results.BadRequest(new
                {
                    error = result.ErrorCode,
                    message = result.ErrorMessage
                });
            }

            id = Guid.NewGuid();
            Role role = req.ToRole(id);
            RoleADO.Insert(dbConn, role);

            return Results.Ok(RoleResponse.FromRole(role));
        });

        app.MapDelete("/Role/{id}", (Guid id ) => RoleADO.Delete(dbConn, id ) ? Results.NoContent() : Results.NotFound());













        //USERROLES



        // POST un nou rol a un usuari
        app.MapPost("/UserRole", (RoleUserRequest req) =>
        {
            Guid id;
            Result result = UserRoleValidator.Validate(req);

            if (!result.IsOk)
            {
                return Results.BadRequest(new
                {
                    error = result.ErrorCode,
                    message = result.ErrorMessage
                });
            }

            id = Guid.NewGuid();
            UserRole userRole = req.ToRolePermission(id);
            UserRoleADO.Insert(dbConn, userRole);

            return Results.Ok(UserRoleResponse.FromRoleUser(userRole));
        });

        //DELETE un rol de un usuari
        app.MapDelete("/User/{id}/Role", (Guid id ) => UserRoleADO.Delete(dbConn, id ) ? Results.NoContent() : Results.NotFound());

        
        
         // GET tots els roles de un usuari
        app.MapGet("/Roles/{id}/User", (Guid id) =>
        {
            List<UserRole> roleUserPermissions = UserRoleADO.GetUserRolesById(dbConn, id)!;
            List<UserRoleResponse> roleUsersResponses = new List<UserRoleResponse>();
            foreach (UserRole userRole in roleUserPermissions)
            {
                roleUsersResponses.Add(UserRoleResponse.FromRoleUser(userRole));
            }
            return Results.Ok(roleUsersResponses);

        });

    }

}

