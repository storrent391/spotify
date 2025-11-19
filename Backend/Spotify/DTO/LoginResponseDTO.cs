namespace Spotify.DTO
{
    public class LoginResponse
    {
        public Guid UserId { get; set; }
        public string Token { get; set; }
        public List<string> Roles { get; set; }
        public List<string> Permissions { get; set; }
    }
}
