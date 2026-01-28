using Spotify.Repository;
using Spotify.Model;
using Spotify.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using Spotify.Common;
//using System.Reflection.Metadata.Ecma335;


namespace Spotify.Endpoints;

public static class SongEndpoints
{
    public static void MapSongEndpoints(this WebApplication app, DatabaseConnection dbConn)
    {

        //POST
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





        //GET
        app.MapGet("/Songs", (Guid reqID) =>
        {
            var perms = ValidacioPermisosADO.GetPermsById(dbConn, reqID);

            if (!perms.Contains(CommonPermissions.GetUsers))
                // return Results.StatusCode(403);
                throw new Exception($"El usuari no te permisos per verure les Songs");

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

                // SOLO UNA VEZ
                List<Task<Media?>> tasques = new();

                foreach (var image in images)
                {
                    tasques.Add(mediaService.ProcessAndInsertUploadedMedia( id, image));
                }

                Media?[] medias = await Task.WhenAll(tasques);

                foreach (Media media in medias)
                {
                    if (media != null)
                    {
                        MediaADO.Insert(dbConn, media);
                    }
                }

                return Results.Ok(new { message = "Imatge pujada correctament." });
            


        }).DisableAntiforgery();
    }             
}
public record SongRequest(Guid Id, string Name);
