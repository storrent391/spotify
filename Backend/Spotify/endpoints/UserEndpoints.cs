namespace Spotify.Endpoints;

public static class MapUserEndpoints
{
    public static void MapUserEndpoints(this WebApplication app, DatabaseConnection dbConn)
    {
        // GET /users
        app.MapGet("/users", () =>
        {
            var products = ProductADO.GetAll(dbConn);
            return Results.Ok(users);
        });

        // GET /user/{id}
        app.MapGet("/user/{id}", (Guid id) =>
        {
            var product = ProductADO.GetById(dbConn, id);
            return product is not null
                ? Results.Ok(product)
                : Results.NotFound(new { message = $"Product with Id {id} not found." });
        });

        // POST /user
        app.MapPost("/user", (UserRequest req) =>
        {
            var user = new UserADO
            {
                Id = Guid.NewGuid(),
                Name = req.Name,
                Password = req.Password,
                Salt = req.Salt,
            };

            UserADO.Insert(dbConn, user);
            return Results.Created($"//{user.Id}", user);
        });

        // PUT /user/{id}
        app.MapPut("/user/{id}", (Guid id, UserRequest req) =>
        {
            var existing = ProductADO.GetById(dbConn, id);
            if (existing == null)
                return Results.NotFound();

            existing.Name = req.Name;
            existing.Salt = req.Salt;

            UserADO.Update(dbConn, existing);
            return Results.Ok(existing);
        });

        // DELETE /user/{id}
        app.MapDelete("/user/{id}", (Guid id) =>
        {
            var deleted = ProductADO.Delete(dbConn, id);
            return deleted ? Results.NoContent() : Results.NotFound();
        });
    }
}

public record UserRequest(Guid FamilyId, string Code, string Name, decimal Price, decimal Discount);