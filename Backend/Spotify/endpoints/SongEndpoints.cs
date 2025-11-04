using Spotify.Repository;
using Spotify.Model;
using Spotify.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

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

        app.MapPost("/Song/{id}/upload", async (Guid id, IFormFileCollection images) =>
        {
            if (images == null || images.Count == 0)
            return Results.BadRequest(new { message = "No s'ha rebut cap imatge." });
            Song? song = SongADO.GetById(dbConn, id);
            if (song is null)
            return Results.NotFound(new { message = $"media amb Id {id} no trobat." });
                
            MediaService mediaService = new();

            for (int i = 0; i < images.Count; i++)
            {
                Media? media = await mediaService.ProcessAndInsertUploadedMedia(dbConn, id, images[i]);

                MediaADO.Insert(dbConn, media);
            }

            return Results.Ok(new { message = "Imatge pujada correctament."});
        }).DisableAntiforgery();
    }             
}
    


public record SongRequest(Guid Id, string Name);
