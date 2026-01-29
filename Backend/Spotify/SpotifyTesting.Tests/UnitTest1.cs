using System;
using Spotify.Encryption;
using Spotify.Model;
using Xunit;

namespace Spotify.Tests
{
    public class PasswordEncryptionTests
    {
        [Fact]
        public void ConvertPassword_ShouldChangePasswordAndSetSalt()
        {
            // Arrange
            var user = new User { Password = "myPassword123" };

            // Act
            PasswordEncryption.ConvertPassword(user);

            // Assert
            // Comprovem que el password s'ha canviat
            Assert.NotEqual("myPassword123", user.Password);

            // Comprovem que el salt s'ha generat
            Assert.False(string.IsNullOrEmpty(user.Salt));

            // Comprovem que el password és base64 (opcional)
            byte[] hashBytes = Convert.FromBase64String(user.Password);
            Assert.Equal(16, hashBytes.Length); // 16 bytes segons el teu Rfc2898DeriveBytes
        }
    }
}


// dotnet add package xunit
// dotnet add package xunit.runner.visualstudio
// dotnet add package Microsoft.NET.Test.Sdk