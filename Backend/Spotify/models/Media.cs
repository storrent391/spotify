namespace Spotify.Model;

public class Media
{
    public Guid Id { get; set; }
    
    public Guid Song_Id { get; set; }
    public string Url { get; set; } = "";
    public string ImagePath { get; set; } = "";
    
}