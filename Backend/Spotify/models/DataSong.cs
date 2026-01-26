namespace Spotify.Model;

public class DataSong
{
    public Guid ID { get; set; }
    public Guid Song_ID { get; set; }
    public Guid Playlist_ID { get; set; }
    public string Song_name { get; set; }
    public string Playlist_name { get; set; }
}
