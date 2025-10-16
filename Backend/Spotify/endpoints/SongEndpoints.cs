using Spotify.Repository;
using Spotify.Model;
using Spotify.Services;
using Microsoft.AspNetCore.Mvc;

//using System.Reflection.Metadata.Ecma335;


namespace Spotify.Endpoints;

public static class SongEndpoints
{
    public static void MapSongEndpoints(this WebApplication app, DatabaseConnection dbConn)
    {
        app.MapPost("/Songs", (SongRequest req) =>
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
        
        app.MapDelete("/Songs/{id}", (Guid Id) => SongADO.Delete(dbConn, Id) ? Results.NoContent() : Results.NotFound());

        MediaService service = new MediaService();

        app.MapPost("/songs/{id}/upload", async (Guid id, [FromForm] List<IFormFile> files) =>

        {
            if (files == null || files.Count == 0)
                return Results.BadRequest("No s'ha rebut cap fitxer.");

            Song song = SongADO.GetById(dbConn, id);
            if (song == null)
                return Results.NotFound($"No existeix cap cançó amb Id {id}");

            List<Media> addedMedia = await service.ProcessAndInsertUploadedMediaRange(dbConn, id, files);

            if (addedMedia.Count == 0)
                return Results.BadRequest("No s'ha pogut processar cap fitxer.");

            return Results.Created($"/Songs/{id}/upload", addedMedia);
        })
        .Accepts<IFormFile>("multipart/form-data")
        .DisableAntiforgery();

        
        app.MapGet("/media/{id:guid}", (Guid id) =>
        {
            Media? media = service.GetMediaById(dbConn, id);
            if (media == null)
                return Results.NotFound("Fitxer no trobat.");

            return Results.Ok(media);
        });
    }
}

public record SongRequest(Guid Id, string Name);
