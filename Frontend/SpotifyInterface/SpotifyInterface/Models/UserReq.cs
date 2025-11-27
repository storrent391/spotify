// A SpotifyInterface.Models
public class UserRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Salt { get; set; } = string.Empty;
}
