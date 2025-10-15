using Spotify.Repository;
using Spotify.Model;
using Spotify.Services;
using System.Reflection.Metadata.Ecma335;


namespace Spotify.Endpoints;

public static class SongEndpoints
{
    public static void MapProductEndpoints(this WebApplication app, DatabaseConnection dbConn)
    {
        app.MapPost("/Songs/{Id}", (SongRequest req) =>
        {
            Song song = new Song
            {
                Id = Guid.NewGuid(),
                Name = req.Name,
            };

            SongADO.Insert(dbConn, song);

            return Results.Created($"/Songs/{song.Id}", song);
        });

        app.MapGet("/Songs", () =>
        {
            List<Song> songs = SongADO.GetAll(dbConn);
            return Results.Ok(songs);
        });

        app.MapGet("/Songs/{Id}", (Guid Id) =>
        {
            Song song = SongADO.GetById(dbConn, Id);

            return song is not null
            ? Results.Ok(song)
            : Results.NotFound(new { message = $"Song with Id {Id} not found" });
        });

    }
}

public record SongRequest(Guid Id, string Name);
