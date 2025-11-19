
using Spotify.Services;
using Spotify.DTO;

namespace Spotify.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this WebApplication app, DatabaseConnection dbConn)
    {
        app.MapPost("/login", (LoginRequest req) =>
        {
            var service = new LoginService(dbConn);

            var result = service.Login(req.Name, req.Password);

            if (result == null)
                return Results.Unauthorized();

            return Results.Ok(result);
        });
    }
}
