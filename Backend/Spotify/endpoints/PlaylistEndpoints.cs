using Spotify.Repository;
using Spotify.Services;
using Spotify.Model;

namespace Spotify.Endpoints;

public static class PlaylistEndpoints
{
    public static void MapPlaylistEndpoints(this WebApplication app, DatabaseConnection dbConn)
    {
        // GET /playlists
        app.MapGet("/playlists", () =>
        {
            List<Playlist>  playlists = PlaylistADO.GetAll(dbConn);
            return Results.Ok(playlists);
        });

        // GET Playlists by id
        app.MapGet("/playlists/{Id}", (Guid Id) =>
        {
            Playlist playlist = PlaylistADO.GetById(dbConn, Id);

            return playlist is not null
                ? Results.Ok(playlist)
                : Results.NotFound(new { message = $"Playlist with Id {Id} not found." });
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
    }
}

// DTO pel request
public record PlaylistRequest(Guid Id, string Name, Guid User_Id);
