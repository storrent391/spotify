using Spotify.Services;
using Spotify.Repository;
using Spotify.Model;
namespace Spotify.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this WebApplication app, DatabaseConnection dbConn)
    {

        app.MapGet("/users", () =>
        {
            List<DataUser> dataUser = UserADO.GetAll(dbConn);
            return Results.Ok(dataUser);
        });


        app.MapGet("/user/{id}", (Guid id) =>
        {
            User user = UserADO.GetById(dbConn, id);
            return user is not null
                ? Results.Ok(user)
                : Results.NotFound(new { message = $"User with Id {id} not found." });
        });

        // POST /user
        app.MapPost("/user", (UserRequest req) =>
        {
            User user = new User
            {
                Id = Guid.NewGuid(),
                Name = req.Name,
                Password = req.Password,
                
            };

            UserADO.Insert(dbConn, user);
            return Results.Created($"/user/{user.Id}", user);
        });

        app.MapPut("/user/{id}", (Guid id, UserRequest req) =>
        {
            var existing = UserADO.GetById(dbConn, id);
            if (existing == null)
                return Results.NotFound();

            existing.Name = req.Name;
            existing.Password = req.Password;
            existing.Salt = req.Salt;

            UserADO.Update(dbConn, existing);
            return Results.Ok(existing);
        });


        app.MapDelete("/user/{id}", (Guid Id) => UserADO.Delete(dbConn, Id) ? Results.NoContent() : Results.NotFound());
    }
}

public record UserRequest(Guid Id, string Name, string Password, string Salt);