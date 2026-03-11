using Spotify.Model;

namespace Spotify.DTO;

public record PlayListResponse(Guid ID, string Name, Guid User_ID) 
{
    public static PlayListResponse FromPlayList(Playlist playlist)   
    {
        return new PlayListResponse(playlist.Id, playlist.Name, playlist.User_ID);
    }
}