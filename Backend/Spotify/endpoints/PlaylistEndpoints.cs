using Spotify.Repository;
using Spotify.Services;
using Spotify.Model;

namespace Spotify.Endpoints;

public static class PlaylistEndpoints
{
    public static void MapProductEndpoints(this WebApplication app, DatabaseConnection dbConn)
    {
        // GET /playlists
        app.MapGet("/playlists", () =>
        {
            List<Playlist>  playlists = PlaylistADO.GetAll(dbConn);
            return Results.Ok(playlists);
        });

        // GET Playlists by id
        app.MapGet("/playlists/{id}", (Guid id) =>
        {
            playlist? playlist = PlaylistsADO.GetById(dbConn, id);

            return playlist is not null
                ? Results.Ok(playlist)
                : Results.NotFound(new { message = $"Playlist with Id {id} not found." });
        });

        // POST /playlists
        app.MapPost("/playlists", (PlaylistRequest req) =>
        {
            Playlist playlist = new Playlist
            {
                Id = Guid.NewGuid(),
                Name = req.Name,
                User_Id = req.User_Id
            };

            PlaylistADO.Insert(dbConn, playlist);

            return Results.Created($"/playlists/{playlist.Id}", playlist);
        });

        app.MapPut("/playlists/{id}", (Guid id, PlaylistRequest req) =>
        {
            var existing = PlaylistADO.GetById(dbConn, id);

            if (existing == null)
            {
                return Results.NotFound();
            }

            Playlist updated = new Playlist
            {
                Id = id,
                Name = req.Name,
                User_Id = req.User_Id
            };

            PlaylistADO.Update(dbConn, updated);

            return Results.Ok(updated);
        });

        // DELETE /playlists/{id}
        app.MapDelete("/playlists/{id}", (Guid id) => PlaylistADO.Delete(dbConn, id) ? Results.NoContent() : Results.NotFound());

        // POST  /playlists/{id}/upload

        app.MapPost("/playlists/{id}/upload", async (Guid id, IFormFile image) =>
        {
            if (image == null || image.Length == 0)
                return Results.BadRequest(new { message = "No s'ha rebut cap imatge." });

           
            Playlist? product = ProductADO.GetById(dbConn, id);
            if (product is null)
                return Results.NotFound(new { message = $"Producte amb Id {id} no trobat." });

            string filePath = await SaveImage(id,image);            

            product.ImagePath = filePath;
            ProductADO.Update(dbConn, product);

            return Results.Ok(new { message = "Imatge pujada correctament.", path = filePath });
        }).DisableAntiforgery();
    }

    
}

// DTO pel request
public record ProductRequest(string Code, string Name, decimal Price);
