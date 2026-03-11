using Spotify.Model;

namespace Spotify.DTO;

public record SongPlaylistResponse(Guid ID, Guid Playlist_Id, Guid Song_Id) 
{
    public static SongPlaylistResponse FromSongPlaylist(SongPlaylist songPlaylist)   
    {
        return new SongPlaylistResponse(songPlaylist.Id, songPlaylist.Playlist_Id, songPlaylist.Song_Id);
    }
}