using System.Security.Cryptography;
using Spotify.Repository;
using Spotify.Model;

namespace Spotify.Encryption;

class PasswordEncryption
{
    public static void ConvertPassword(User user)
    {
        byte[] salt = new byte[16];
        RandomNumberGenerator.Fill(salt);
        user.Salt = Convert.ToBase64String(salt);

        using var HashPassword = new Rfc2898DeriveBytes(user.Password, salt, 10000);
        byte[] hash = HashPassword.GetBytes(16);

        user.Password = Convert.ToBase64String(hash);
    }
}