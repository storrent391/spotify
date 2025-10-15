using Spotify.Repository;
using Spotify.Model;
using Spotify.Services;
using System.Security.Cryptography.X509Certificates;
using System.IO.Pipelines;
using System.Data.Common;

namespace Spotify.Endpoints;

public static class Endpoints
{
    public static void MapProductEndpoints(this WebApplication app, DatabaseConnection dbConn)
    {
        app.MapPut("/Songs/{Id}", () =>
        {
            Song song = SongADO.Insert(dbConn, Id, Name);
            return song is not null
            ? Result.NotFound(new { message = $"Song with Id {Id} alredy exist" })
            : Result.Ok(song);
        });

        app.MapGet("/Songs", () =>
        {
            List<Song> songs = SongADO.GetAll(dbConn);
            return Request.Ok(songs);
        });

        app.MapGet("/Songs/{Id}", () =>
        {
            Song song = SongADO.GetById(dbConn, Id, Name);

            return song is not null
            ? Results.Ok(song)
            : Results.NotFound(new { message = $"Song with Id {Id} not found" });
        });

    }
}

