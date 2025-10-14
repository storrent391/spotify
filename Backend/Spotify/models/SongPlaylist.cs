namespace Spotify.Model;

public class SongPlaylist
{
    public Guid Id { get; set; }
    
    public Guid Song_Id { get; set; }

    public Guid Playlist_Id { get; set; }
}
